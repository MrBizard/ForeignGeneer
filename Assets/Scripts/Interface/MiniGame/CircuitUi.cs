using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.manager;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.MiniGame;

public partial class CircuitUi : Control, BaseUi
{
    [Export] private InventoryBottomUi _inventoryBottomUi;
    [Export] private Button _craftButton;
    [Export] private Button _changeButton;
    [Export] private FlowContainer _gridContainer;
    [Export] private PackedScene _circuitSlotUi;
    [Export] private SlotUI _outputSlotUi;
    private Circuit _circuit;
    public void update(InterfaceType? interfaceType = null)
    {
        switch (interfaceType)
        {
            default:
                updateUi();
                break;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        _circuit = MiniGameManager.instance._circuit;
        _inventoryBottomUi.initialize(_circuit.currentRecipe);
        initRecipe();
        _outputSlotUi.initialize(_circuit.outputInventory,0);
    }

    private void updateUi()
    {
        
    }
    public void detach()
    {
        _circuit.detach(this);
    }

    public void onChangeButtonPressed()
    {
        _circuit.interact(InteractType.Modify);
        foreach (var circuitSlotUi in _gridContainer.GetChildren())
        {
            circuitSlotUi.QueueFree();
        }
        initRecipe();
    }

    public void onCraftButtonPressed()
    {
        _circuit.interact(InteractType.Craft);
        
    }
    public void initialize(object data)
    {
    }

    private void initRecipe()
    {
        for(int i =0; i<_circuit.currentRecipe.input.Count;i++)
        {
            CircuitSlotUi circuitSlotUi = _circuitSlotUi.Instantiate<CircuitSlotUi>();
            circuitSlotUi.initialize(_circuit, i);
            _gridContainer.AddChild(circuitSlotUi);
        }
    }
}