using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.central;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class CoalCentral : StaticBody3D, ICentral
{
    [Export] private PackedScene _centralPackedScene;
    [Export] private PackedScene _recipeListUiPackedScene;
    [Export] public RecipeList recipeList { get; set; }
    public float pollutionInd { get; set; }
    public Inventory input { get; set; }
    public Craft craft { get; set; }
    public short tier { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;
    private CentralUi _centralUi;
    private RecetteList _recipeListUi;
    private Manager _manager;
    public float electricalCost = 0;

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(1);
        recipeList?.init();
        input.onInventoryUpdated += onInventoryUpdated;
        _manager = (Manager)GetNode("/root/Main/Manager");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isCrafting)
        {
            craftProgress = 1f - (float)(craftTimer.TimeLeft / craft.recipe.duration);
            _centralUi?.updateProgressBar(craftProgress);
            _centralUi?.updateElectricity();
        }
    }

    private void onInventoryUpdated()
    {
        if (!isCrafting && craft.compareRecipe())
        {
            startCraft();
        }
        _centralUi?.updateUi();
    }

    /// <summary>
    /// Définit la recette à utiliser pour le craft.
    /// </summary>
    /// <param name="recipe">La recette à utiliser.</param>
    public void setCraft(Recipe recipe)
    {
        craft = recipe == null ? null : new Craft(recipe);
        if (craft != null)
            craft.init(input, null);
        openUi();
    }

    private void startCraft()
    {
        if (isCrafting) return;

        _manager.addGlobalElectricity(electricalCost);
        if (!craft.consumeResources()) return;
        if (craft.recipe.duration <= 0) return;

        isCrafting = true;
        craftProgress = 0f;
        craftTimer = new Timer();
        craftTimer.WaitTime = craft.recipe.duration;
        craftTimer.Timeout += onCraftFinished;
        AddChild(craftTimer);
        craftTimer.Start();
    }

    private void onCraftFinished()
    {
        isCrafting = false;
        _manager.removeGlobalElectricity(electricalCost);
        craftTimer.QueueFree();

        if (craft.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();
            if (!outputAdded) return;
            startCraft();
        }

        _centralUi?.updateProgressBar(0f);
        _centralUi?.updateElectricity();
        onInventoryUpdated();
    }

    /// <summary>
    /// Ouvre l'interface utilisateur de la centrale.
    /// </summary>
    public void openUi()
    {
        closeUi();
        if (craft == null)
        {
            UiManager.instance.openUi("RecipeListUI", this);
        }
        else
        {
            UiManager.instance.openUi("CentralUi", this);
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    /// <summary>
    /// Ferme l'interface utilisateur de la centrale.
    /// </summary>
    public void closeUi()
    {
        UiManager.instance.closeUi();
    }

    /// <summary>
    /// Détruit la centrale.
    /// </summary>
    public void dismantle()
    {
        if (isCrafting)
        {
            craftTimer.Stop();
            craftTimer.QueueFree();
        }
        input.onInventoryUpdated -= onInventoryUpdated;
        QueueFree();
    }
}