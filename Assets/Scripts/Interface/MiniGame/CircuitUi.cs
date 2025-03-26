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
    [Export] private Control _slotContainer;
    [Export] private Control _connectionLayer;
    [Export] private PackedScene _circuitSlotUi;
    [Export] private SlotUI _outputSlotUi;

    private Circuit _circuit;
    private List<Vector2> _slotPositions = new();
    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        base._Ready();
        _rng.Randomize();
        _circuit = MiniGameManager.instance.circuit;

        _inventoryBottomUi.initialize(_circuit.currentRecipe);
        _outputSlotUi.initialize(_circuit.outputInventory, 0);

        _craftButton.Pressed += OnCraftButtonPressed;
        _changeButton.Pressed += OnChangeButtonPressed;

        InitCircuit();
    }

    private void InitCircuit()
    {
        ClearCircuit();
        PlaceSlots();
        UpdateContainerSize();
    }

    private void PlaceSlots()
    {
        _slotPositions.Clear();
        Vector2 containerSize = _slotContainer.Size;
        float margin = 10; // La marge entre les slots

        if (_circuit.slotPositions == null || _circuit.slotPositions.Count != _circuit.currentRecipe.input.Count)
        {
            _circuit.slotPositions = new List<Vector2>(new Vector2[_circuit.currentRecipe.input.Count]);
        }

        for (int i = 0; i < _circuit.currentRecipe.input.Count; i++)
        {
            var input = _circuit.currentRecipe.input[i];
            CircuitSlotUi slot = _circuitSlotUi.Instantiate<CircuitSlotUi>();
            slot.initialize(_circuit, i, false);

            int itemCount = input.Stack;
            Vector2 slotSize = GetSlotSize(itemCount);

            slot.CustomMinimumSize = slotSize;
            slot.Size = slotSize;

            Vector2 position;

            if (_circuit.slotPositions[i] != Vector2.Zero)
            {
                position = _circuit.slotPositions[i];
            }
            else
            {
                position = FindValidPosition(slotSize, containerSize, margin);
                _circuit.slotPositions[i] = position; // Sauvegarde de la position
            }

            slot.Position = position;
            _slotContainer.AddChild(slot);
            _slotPositions.Add(position);
        }

        UpdateContainerSize();
    }

    private Vector2 FindValidPosition(Vector2 slotSize, Vector2 containerSize, float margin)
    {
        Vector2 position;
        bool validPosition;
        int attempts = 0;

        do
        {
            validPosition = true;
            position = new Vector2(
                _rng.RandfRange(margin, containerSize.X - slotSize.X - margin),
                _rng.RandfRange(margin, containerSize.Y - slotSize.Y - margin)
            );

            foreach (var existing in _slotPositions)
            {
                if (Mathf.Abs(position.X - existing.X) < slotSize.X + margin &&
                    Mathf.Abs(position.Y - existing.Y) < slotSize.Y + margin) // Ajout de marge dans le test
                {
                    validPosition = false;
                    break;
                }
            }

            attempts++;
        } while (!validPosition && attempts < 50);

        return position;
    }

    private void ClearCircuit()
    {
        foreach (Node child in _slotContainer.GetChildren())
            child.QueueFree();

        _slotPositions.Clear();
        _circuit.slotPositions.Clear();
    }

    private void UpdateContainerSize()
    {
        float maxWidth = 0;
        float maxHeight = 0;

        foreach (Node child in _slotContainer.GetChildren())
        {
            if (child is CircuitSlotUi slot)
            {
                maxWidth = Mathf.Max(maxWidth, slot.Position.X + slot.Size.X);
                maxHeight = Mathf.Max(maxHeight, slot.Position.Y + slot.Size.Y);
            }
        }

        _slotContainer.CustomMinimumSize = new Vector2(maxWidth + 20, maxHeight + 20); // Ajout d'une petite marge
    }

    private Vector2 GetSlotSize(int itemCount)
    {
        float baseSize = 32;
        float scaleFactor = 2.0f;
        float minSize = 32;
        float maxSize = 128;
        float size = Mathf.Clamp(baseSize * Mathf.Pow(scaleFactor, itemCount - 1), minSize, maxSize);

        return new Vector2(size, size);
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

    public void update(InterfaceType? interfaceType = null)
    {
    }

    public void detach()
    {
        _circuit.detach(this);
    }

    public void initialize(object data)
    {
    }

    // Mise à jour de la texture du slot en fonction du nombre d'items
    public void UpdateSlotTexture(CircuitSlotUi slot, int currentStack, int maxStack)
    {
        if (slot != null)
        {
            // Calculer la proportion de l'item dans le slot
            float ratio = (float)currentStack / maxStack;

            // Modifier la taille de la texture en fonction de l'état du slot
            slot.icon.Scale = new Vector2(ratio, 1f); // Ajuste l'échelle de la texture

            // Si le nombre d'items est suffisant, cacher l'icône de background
            if (currentStack >= maxStack)
            {
                slot.background.Visible = false; // Cacher l'icône de background
            }
            else
            {
                slot.background.Visible = true; // Afficher l'icône de background
            }
        }
    }
}
