using ForeignGeneer.Assets.Scripts.Interface.MiniGame;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.Inventory;

public partial class PlayerInventoryUi : Control, BaseUi
{
    [Export]private InventoryUi _inventoryUi;
    [Export]private CircuitUi _circuitUi;
    [Export]private TabContainer _tabContainer;

    public override void _Ready()
    {
        base._Ready(); 
        
    }

    public void update(InterfaceType? interfaceType = null)
    {
        
    }

    public void detach()
    {
        
    }

    public void initialize(object data)
    {
        
    }
}