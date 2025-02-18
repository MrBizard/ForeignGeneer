using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Fonderie : StaticBody3D, IInputFactory, IOutputFactory
{
    [Export] public int inputSlotCount { get; set; } = 2;
    [Export] public FactoryStatic factoryStatic { get; set; }
    [Export] public string factoryUiName { get; set; }
    [Export] public string recipeUiName { get; set; }
    public Craft craft { get; set; }
    public Inventory input { get; set; }
    public Inventory output { get; set; }
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

    private Timer _craftTimer;
    private FonderieUi _fonderieUi;

    public override void _Ready()
    {
        base._Ready();
        init();

        _craftTimer = new Timer();
        _craftTimer.Name = "CraftTimer";
        AddChild(_craftTimer);
    }

    public void init()
    {
        input = new Inventory(inputSlotCount);
        output = new Inventory(1);
        factoryStatic.recipeList?.init();
        input.onInventoryUpdated += onInventoryUpdated;
        output.onInventoryUpdated += onInventoryUpdated;
    }

    public override void _Process(double delta)
    {
        if (craft != null && craft.isCrafting)
        {
            craft.updateCraftProgress(delta);
            updateProgressBar();
        }
    }

    public void setCraft(Recipe recipe)
    {
        if (recipe == null)
        {
            craft = null;
            return;
        }

        craft = new Craft(recipe, input, output);
        craft.craftTimer = _craftTimer;
        craft.startCraft(onCraftFinished);
        openUi();
    }

    private void onInventoryUpdated()
    {
        if (craft != null && craft.canContinue())
        {
            startCraft();
        }
        updateUi();
    }

    private void startCraft()
    {
        if (EnergyManager.instance.isDown() || craft.isCrafting)
        {
            return;
        }
        craft.startCraft(onCraftFinished);
        updateProgressBar();
        updateUi();
    }

    private void onCraftFinished()
    {
        craft.stopCraft();
        craft.addOutput();
        updateProgressBar();
        if (craft.canContinue())
            startCraft();
        updateUi();
    }

    public void openUi()
    {
        closeUi();
        if (craft == null)
        {
            UiManager.instance.openUi(recipeUiName, this);
            _fonderieUi = null;
        }
        else
        {
            UiManager.instance.openUi(factoryUiName, this);
            _fonderieUi = (FonderieUi)UiManager.instance.getUi(factoryUiName);
        }
    }

    public void closeUi()
    {
        _fonderieUi = null;
        UiManager.instance.closeUi();
    }

    public void dismantle()
    {
        craft?.stopCraft();
        input.onInventoryUpdated -= onInventoryUpdated;
        output.onInventoryUpdated -= onInventoryUpdated;
        QueueFree();
    }

    private void updateUi()
    {
        if (UiManager.instance.isAnyUiOpen())
        {
            UiManager.instance.refreshCurrentUi(this);
        }
    }

    private void updateProgressBar()
    {
        if (UiManager.instance.isUiOpen(factoryUiName))
        {
            _fonderieUi?.updateProgressBar(craft.craftProgress);
        }
    }
}