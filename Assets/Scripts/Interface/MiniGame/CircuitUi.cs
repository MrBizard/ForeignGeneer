using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.MiniGame;

public partial class CircuitUi : Control, BaseUi
{
    [Export] private InventoryBottomUi _inventoryBottomUi;
    [Export] private Button _craftButton;
    [Export] private Button _changeButton;
    [Export] private GridContainer _gridContainer;
    public override void _Ready()
    {
        base._Ready();
    }

    public void update(InterfaceType? interfaceType = null)
    {
        switch (interfaceType)
        {
            default:
                updateUi();
                break;
        }
    }

    private void updateUi()
    {
        
    }
    public void detach()
    {
        
    }

    public void onChangeButtonPressed()
    {
        
    }

    public void onCraftButtonPressed()
    {
        
    }
    public void initialize(object data)
    {
        _inventoryBottomUi.initialize(data);
    }
}