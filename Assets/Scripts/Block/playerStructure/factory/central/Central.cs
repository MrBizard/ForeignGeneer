using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using Godot;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Central : StaticBody3D, IFactory<FactoryStatic>
{
    [Export] public string factoryUiName { get; set; }
    [Export] public float productionInterval { get; set; } = 2.0f;
    [Export] public FactoryStatic factoryStatic { get; set; } 

    private CentralUi _centralUi;
    private Timer _productionTimer;
    private float _productionProgress = 0f; 

    [Signal]
    public delegate void ProgressUpdatedEventHandler(float progress);

    public override void _Ready()
    {
        base._Ready();

        _productionTimer = new Timer();
        _productionTimer.Name = "ProductionTimer";
        _productionTimer.WaitTime = productionInterval;
        _productionTimer.OneShot = false; 
        _productionTimer.Timeout += onProductionTimerTimeout;
        AddChild(_productionTimer);

        _productionTimer.Start();

        ProgressUpdated += updateProgressBar;
    }

    private void onProductionTimerTimeout()
    {
        EnergyManager.instance.addGlobalElectricity(factoryStatic.electricalCost);

        _productionProgress = 0f;
        EmitSignal(nameof(ProgressUpdated), _productionProgress);

    }

    public void openUi()
    {
        closeUi();
        UiManager.instance.openUi(factoryUiName, this);
        _centralUi = (CentralUi)UiManager.instance.getUi(factoryUiName);
        EmitSignal(nameof(ProgressUpdated), _productionProgress);
    }

    public void closeUi()
    {
        _centralUi = null;
        UiManager.instance.closeUi();
    }

    public void dismantle()
    {
        _productionTimer.Stop();
        QueueFree();
    }

    private void updateProgressBar(float progress)
    {
        if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
        {
            _centralUi.updateProgressBar(progress);
        }
    }
}