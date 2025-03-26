using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.manager;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.MiniGame;

public partial class CircuitUi : Control, BaseUi
{
    [Export] private InventoryBottomUi _inventoryBottomUi;
    [Export] private Button _craftButton;
    [Export] private Button _changeButton;
    [Export] private Control _slotContainer;
    [Export] private Control _connectionLayer;
    private List<Vector2> _slotPositions = new List<Vector2>();
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
        foreach (var circuitSlotUi in _slotContainer.GetChildren())
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
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        _slotPositions.Clear();
        foreach (Node child in _slotContainer.GetChildren())
        {
            child.QueueFree();
        }

        float radius = 100; // Rayon d'une zone où placer les slots
        int maxAttempts = 10; // Nombre max de tentatives pour éviter le chevauchement
        float minDistance = 70; // Distance minimale entre les slots pour éviter le chevauchement

        foreach (var _ in _circuit.currentRecipe.input)
        {
            CircuitSlotUi circuitSlotUi = _circuitSlotUi.Instantiate<CircuitSlotUi>();

            Vector2 slotPosition;
            bool positionAccepted;

            int attempts = 0;
            do
            {
                float randomX = rng.RandfRange(-radius, radius);
                float randomY = rng.RandfRange(-radius, radius);
                slotPosition = new Vector2(randomX, randomY);

                // Vérifier si la position ne chevauche pas un autre slot
                positionAccepted = true;
                foreach (var existingPos in _slotPositions)
                {
                    if (slotPosition.DistanceTo(existingPos) < minDistance)
                    {
                        positionAccepted = false;
                        break;
                    }
                }

                attempts++;
            } while (!positionAccepted && attempts < maxAttempts);

            circuitSlotUi.Size = new Vector2(64, 64);
            circuitSlotUi.Position = slotPosition;
            _slotContainer.AddChild(circuitSlotUi);
            _slotPositions.Add(slotPosition);
        }
    }

}