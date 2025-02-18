using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Central : StaticBody3D, IFactory
{
    [Export] public FactoryStatic factoryStatic { get; set; }
    [Export] public string factoryUiName{ get; set; }
    public Craft craft { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;

    private CentralUi _centralUi;

    /// <summary>
    /// Called when the node is added to the scene. Initializes the central UI and inventory.
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        factoryStatic.recipeList?.init();
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
    /// Starts the crafting process, consuming resources and initializing the timer for crafting.
    /// </summary>
    private void startCraft()
    {
        if (isCrafting || craft == null) return;

        if (!craft.consumeResources()) return;

        if (craft.recipe.duration <= 0) return;

        EnergyManager.instance.addGlobalElectricity(factoryStatic.electricalCost);

        isCrafting = true;
        craftProgress = 0f;
        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
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

    /// <summary>
    /// Called when the crafting process is finished, adding the produced resources to the inventory and updating the UI.
    /// </summary>
    private void onCraftFinished()
    {
        isCrafting = false;
        EnergyManager.instance.removeGlobalElectricity(factoryStatic.electricalCost);

        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
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
    }

    /// <summary>
    /// Opens the UI related to the central machine (either the recipe list or the crafting UI).
    /// </summary>
    public void openUi()
    {
        closeUi();
        UiManager.instance.openUi(factoryUiName, this);
        _centralUi = (CentralUi)UiManager.instance.getUi(factoryUiName);
        
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    /// <summary>
    /// Closes the UI related to the central machine.
    /// </summary>
    public void closeUi()
    {
        _centralUi = null;
        UiManager.instance.closeUi();
    }

    /// <summary>
    /// Dismantles the central machine, stopping the crafting process if active and freeing resources.
    /// </summary>
    public void dismantle()
    {
        if (isCrafting)
        {
            craftTimer?.Stop();
            craftTimer?.QueueFree();
        }
        QueueFree();
    }

    /// <summary>
    /// Updates the progress bar for crafting in the central UI.
    /// </summary>
    /// <param name="progress">The current progress value (from 0 to 1).</param>
    private void updateProgressBar(float progress)
    {
        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
        {
            _centralUi.updateProgressBar(progress);
        }
    }
}