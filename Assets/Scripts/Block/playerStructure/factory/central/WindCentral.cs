using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using Godot;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class WindCentral : StaticBody3D, IFactory<FactoryStatic>
{
    [Export] public string factoryUiName { get; set; }
    [Export] public FactoryStatic factoryStatic { get; set; } 

    private WindUi _centralUi;
    private float coef = 0.2f;
    public float powerGenerated;
    public override void _Ready()
    {
        base._Ready();
        powerGenerated = factoryStatic.electricalCost *
                               (1 + coef * Mathf.Log(1 + GlobalPosition.Y));
        powerGenerated = Mathf.Round(powerGenerated * 10)/10;
        EnergyManager.instance.addGlobalElectricity(powerGenerated);
    }

    public void openUi()
    {
        closeUi();
        UiManager.instance.openUi(factoryUiName, this);
        _centralUi = (WindUi)UiManager.instance.getUi(factoryUiName);
    }

    public void closeUi()
    {
        _centralUi = null;
        UiManager.instance.closeUi();
    }

    public void dismantle()
    {
        QueueFree();
    }
    
}