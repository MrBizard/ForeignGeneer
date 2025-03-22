using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;

public partial class Fonderie : PlayerBaseStructure, IInputFactory<CraftingFactoryStatic>, IOutputFactory<CraftingFactoryStatic>
{
    [Export] public string factoryUiName { get; set; }
    [Export] public string recipeUiName { get; set; }
    [Export]
    public CraftingFactoryStatic itemStatic
    {
        get => base.itemStatic as CraftingFactoryStatic;
        set => SetItemStatic(value);
    }

    public BaseCraft craft { get; set; }
    public Inventory input { get; set; }
    public Inventory output { get; set; }

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
        input = new Inventory(itemStatic.recipeList.Count);
        output = new Inventory(1);
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

    private void onInventoryUpdated()
    {
        if (craft != null && craft.canContinue())
        {
            startCraft();
        }
        updateUi();
    }

    public void setCraft(Recipe recipe)
    {
        if (recipe == null)
        {
            craft = null;
            return;
        }

        craft = new FixedInputOutputCraft(recipe, input, output);
        craft.craftTimer = _craftTimer;
        craft.startCraft(onCraftFinished);
        openUi();
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
        if (craft != null)
        {
            craft.stopCraft();
            craft.addOutput();
            updateProgressBar();
            if (craft.canContinue())
                startCraft();
        }

        updateUi();
    }

    public override void openUi()
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

    public override void closeUi()
    {
        _fonderieUi = null;
        UiManager.instance.closeUi();
    }

    public void dismantle()
    {
        craft?.stopCraft();
        input.onInventoryUpdated -= onInventoryUpdated;
        output.onInventoryUpdated -= onInventoryUpdated;
        base.dismantle();
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