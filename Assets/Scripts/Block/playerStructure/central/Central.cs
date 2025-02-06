using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Central : StaticBody3D, IFactory
{
    [Export] private PackedScene _centralUiPackedScene;
    [Export] private PackedScene _recipeListUiPackedScene;
    [Export] public FactoryStatic factoryStatic { get; set; }
    public Inventory input { get; set; }
    public Craft craft { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;
    public bool isOpen { get; private set; } = false; // Booléen pour suivre l'état de l'UI

    public RecipeList recipeList
    {
        get => factoryStatic?.recipeList;
        set
        {
            if (factoryStatic != null)
            {
                factoryStatic.recipeList = value;
            }
        }
    }

    private CentralUi _centralUi;

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(1);
        input.onInventoryUpdated += onInventoryUpdated;
        factoryStatic.recipeList?.init();
        PollutionManager.instance.addPolution(0);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isCrafting)
        {
            craftProgress += (float)delta / craft.recipe.duration;
            updateProgressBar(craftProgress);
        }
    }

    private void onInventoryUpdated()
    {
        if (!isCrafting && craft != null && craft.compareRecipe())
        {
            startCraft();
        }
        if (isOpen)
        {
            _centralUi?.updateUi();
        }
    }

    public void setCraft(Recipe recipe)
    {
        craft = recipe == null ? null : new Craft(recipe);
        if (craft != null)
            craft.init(input, null);
        openUi();
    }

    private void startCraft()
    {
        if (isCrafting || craft == null) return;
        if (!craft.consumeResources()) return;
        if (craft.recipe.duration <= 0) return;

        EnergyManager.instance.addGlobalElectricity(factoryStatic.electricalCost);

        isCrafting = true;
        craftProgress = 0f;

        if (isOpen)
        {
            _centralUi?.updateProgressBar(craftProgress);
            _centralUi?.updateElectricity();
        }

        craftTimer = new Timer();
        craftTimer.WaitTime = craft.recipe.duration;
        craftTimer.Timeout += onCraftFinished;
        AddChild(craftTimer);
        craftTimer.Start();
    }

    private void onCraftFinished()
    {
        isCrafting = false;
        EnergyManager.instance.removeGlobalElectricity(factoryStatic.electricalCost);

        if (isOpen)
        {
            _centralUi.updateElectricity();
            _centralUi.updateProgressBar(0f);
        }

        craftTimer?.QueueFree();

        if (craft?.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();
            if (!outputAdded) return;
        }

        onInventoryUpdated();
    }

    public void openUi()
    {
        if (isOpen)
        {
            closeUi();
        }

        if (craft == null)
        {
            UiManager.instance.openUi("RecipeListUI", this);
            _centralUi = null;
        }
        else
        {
            UiManager.instance.openUi("CentralUi", this);
            _centralUi = (CentralUi)UiManager.instance.getUi("CentralUi");
        }

        isOpen = true; // Marquer l'UI comme ouverte
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void closeUi()
    {
        if (!isOpen) return;

        _centralUi = null;
        isOpen = false; // Marquer l'UI comme fermée
    }

    public void dismantle()
    {
        if (isCrafting)
        {
            craftTimer?.Stop();
            craftTimer?.QueueFree();
        }
        input.onInventoryUpdated -= onInventoryUpdated;
        closeUi(); 
        QueueFree();
    }

    private void updateProgressBar(float progress)
    {
        if (isOpen)
        {
            _centralUi?.updateProgressBar(progress);
        }
    }
}
