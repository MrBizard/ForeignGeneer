using ForeignGeneer.Assets.Scripts.block.playerStructure;
using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;

public partial class CentralInput : PlayerBaseStructure, IInputFactory<CraftingFactoryStatic>
{
    [Export]
    public CraftingFactoryStatic itemStatic
    {
        get => base.itemStatic as CraftingFactoryStatic;
        set => SetItemStatic(value);
    }
    [Export] public string factoryUiName { get; set; }
    [Export] public string recipeUiName { get; set; }
    public Inventory input { get; set; }
    public BaseCraft craft { get; set; }

    private CentralUi _centralUi;
    private Timer _craftTimer;

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(itemStatic.recipeList.Count);
        input.onInventoryUpdated += onInventoryUpdated;
        _craftTimer = new Timer();
        _craftTimer.Name = "CraftTimer";
        AddChild(_craftTimer);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
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

        craft = new FixedInputCraft(recipe, input);
        craft.craftTimer = _craftTimer;
        openUi();
    }

    private void startCraft()
    {
        if (craft.startCraft(onCraftFinished))
        {
            EnergyManager.instance.addGlobalElectricity(itemStatic.electricalCost);
        }
        updateProgressBar();
        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
        {
            _centralUi.updateElectricity();
        }
    }

    private void onCraftFinished()
    {
        GD.Print("craft finish");
        if (craft != null)
        {
            craft.stopCraft();
            EnergyManager.instance.removeGlobalElectricity(itemStatic.electricalCost);
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
            _centralUi = null;
        }
        else
        {
            UiManager.instance.openUi(factoryUiName, this);
            _centralUi = (CentralUi)UiManager.instance.getUi(factoryUiName);
        }
    }

    public override void closeUi()
    {
        _centralUi = null;
        UiManager.instance.closeUi();
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
        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
        {
            _centralUi.updateProgressBar(craft.craftProgress);
        }
    }
}