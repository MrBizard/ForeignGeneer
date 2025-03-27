using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts;
using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.Interface;
using ForeignGeneer.Assets.Scripts.manager;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.MiniGame;

public partial class CircuitUi : Control, BaseUi
{
    [Export] private InventoryBottomUi _inventoryBottomUi;
    [Export] private Button _craftButton;
    [Export] private Button _changeButton;
    [Export] private Control _slotContainerPath;
    [Export] private SlotUI _outputSlotUi;

    private Circuit _circuit;
    private List<CircuitSlotUi> _slots = new();

    public override void _Ready()
    {
        base._Ready();
        _circuit = MiniGameManager.instance.circuit;
        foreach (Node child in _slotContainerPath.GetChildren())
        {
            if (child is CircuitSlotUi slot)
                _slots.Add(slot);
        }

        _inventoryBottomUi.initialize(_circuit.currentRecipe);
        _outputSlotUi.initialize(_circuit.outputInventory, 0);

        _craftButton.Pressed += OnCraftButtonPressed;
        _changeButton.Pressed += OnChangeButtonPressed;

        InitCircuit();
    }

    private void InitCircuit()
    {
        for (int i = 0; i < _circuit.currentRecipe.input.Count; i++)
        {
            var input = _circuit.currentRecipe.input[i];
            _slots[i].initialize(_circuit, i, false);
        }
    }

    private void OnCraftButtonPressed()
    {
        _circuit.interact(InteractType.Craft);
    }

    private void OnChangeButtonPressed()
    {
        _circuit.interact(InteractType.Modify);
        InitCircuit();
    }

    public void update(InterfaceType? interfaceType = null) {}

    public void detach()
    {
        _circuit.detach(this);
    }

    public void initialize(object data) {}
}
