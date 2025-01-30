using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.central;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Central : StaticBody3D, ICentral
{
    [Export] private PackedScene _centralUiPackedScene;
    [Export] private PackedScene _recipeListUiPackedScene; 
    [Export] public FactoryStatic centralStatic;
    public Inventory input { get; set; }
    public Craft craft { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;
    
    public RecipeList recipeList 
    {
        get => centralStatic?.recipeList;
        set 
        {
            if (centralStatic != null) 
            {
                centralStatic.recipeList = value;
            }
        }
    }
    private CentralUi _centralUi;
    private RecetteList _recipeListUi;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the central UI and inventory.
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(1);
        input.onInventoryUpdated += onInventoryUpdated;
        centralStatic.recipeList?.init();
        PollutionManager.instance.addPolution(0);
    }

    /// <summary>
    /// Called every frame to update the crafting progress if a craft is in progress.
    /// </summary>
    /// <param name="delta">The time elapsed since the last frame.</param>
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isCrafting)
        {
            craftProgress += (float)delta / craft.recipe.duration;
            updateProgressBar(craftProgress);
        }
    }

    /// <summary>
    /// Callback for when the inventory has been updated.
    /// If the recipe can be crafted, it starts the crafting process.
    /// </summary>
    private void onInventoryUpdated()
    {
        if (!isCrafting && craft.compareRecipe())
        {
            startCraft();
        }
        _centralUi?.updateUi();
    }
    
    /// <summary>
    /// Sets the recipe to be used for crafting and opens the crafting UI.
    /// </summary>
    /// <param name="recipe">The recipe to use for crafting.</param>
    public void setCraft(Recipe recipe)
    {
        craft = recipe == null ? null : new Craft(recipe);
        if (craft != null)
            craft.init(input, null);
        openUi();
    }

    /// <summary>
    /// Starts the crafting process, consuming resources and initializing the timer for crafting.
    /// </summary>
    private void startCraft()
    {
        if (isCrafting) return;
        
        if (!craft.consumeResources()) return;

        if (craft.recipe.duration <= 0) return;

        isCrafting = true;
        craftProgress = 0f; 
        _centralUi?.updateProgressBar(craftProgress); 
        _centralUi?.updateElectricity();
        craftTimer = new Timer();
        craftTimer.WaitTime = craft.recipe.duration;
        craftTimer.Timeout += onCraftFinished;
        EnergyManager.instance.addGlobalElectricity(centralStatic.electricalCost);
        AddChild(craftTimer);
        craftTimer.Start();
    }
    
    /// <summary>
    /// Called when the crafting process is finished, adding the produced resources to the inventory and updating the UI.
    /// </summary>
    private void onCraftFinished()
    {
        isCrafting = false;
        
        EnergyManager.instance.removeGlobalElectricity(centralStatic.electricalCost);
        _centralUi?.updateElectricity();
        craftTimer.QueueFree(); 

        if (craft.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();
            if (!outputAdded) return;
        }

        _centralUi?.updateProgressBar(0f); 
        onInventoryUpdated(); 
    }

    /// <summary>
    /// Opens the UI related to the central machine (either the recipe list or the crafting UI).
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
            _centralUi = (CentralUi)UiManager.instance.getUi("CentralUi");
        }
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    /// <summary>
    /// Closes the UI related to the central machine.
    /// </summary>
    public void closeUi()
    {
        UiManager.instance.closeUi();
    }

    /// <summary>
    /// Dismantles the central machine, stopping the crafting process if active and freeing resources.
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

    /// <summary>
    /// Updates the progress bar for crafting in the central UI.
    /// </summary>
    /// <param name="progress">The current progress value (from 0 to 1).</param>
    private void updateProgressBar(float progress)
    {
        if (UiManager.instance.isUiOpen("CentralUi"))
        {
            if (_centralUi is CentralUi)
            {
                _centralUi.updateProgressBar(progress);
            }
        }
    }
}