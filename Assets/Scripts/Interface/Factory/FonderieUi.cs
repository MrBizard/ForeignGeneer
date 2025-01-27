using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class FonderieUi : BaseUi
{
    private Fonderie _fonderie;
    [Export] private PackedScene _slotUi;
    private Node _inputList;
    private Node _outputContainer;
    private Label _craftText;
    private SlotUI _outputSlot;
    private List<SlotUI> _inputSlots = new List<SlotUI>();

    public override void _Ready()
    {
        base._Ready();
        _inputList = GetNode<Node>("InputList");
        _outputContainer = GetNode<Node>("OutputContainer");
        _craftText = GetNode<Label>("CraftText");
    }

    public override void initialize(Node data)
    {
        if (data is Fonderie fonderie)
        {
            _fonderie = fonderie;
            InitializeInputSlots();
            InitializeOutputSlot();
            UpdateCraftText();
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            GD.PrintErr("FonderieUi: Data is not of type Fonderie.");
        }
    }

    private void InitializeInputSlots()
    {
        foreach (var child in _inputList.GetChildren())
        {
            child.QueueFree();
        }
        _inputSlots.Clear();

        for (int i = 0; i < _fonderie.input.slots.Count; i++)
        {
            var slot = _slotUi.Instantiate<SlotUI>();
            slot.initialize(_fonderie.input.slots[i], _fonderie.input);
            slot.setBackgroundTexture(_fonderie.craft.recipe.input[i].getResource().getInventoryIcon);
            _inputSlots.Add(slot);
            _inputList.AddChild(slot);
        }
    }

    private void InitializeOutputSlot()
    {
        foreach (var child in _outputContainer.GetChildren())
        {
            child.QueueFree();
        }

        _outputSlot = _slotUi.Instantiate<SlotUI>();
        _outputSlot.initialize(_fonderie.output.slots[0], _fonderie.output, true);
        UpdateOutputSlotBackground();
        _outputContainer.AddChild(_outputSlot);
    }

    private void UpdateCraftText()
    {
        if (_fonderie.craft != null && _fonderie.craft.recipe != null)
        {
            _craftText.Text = "Résultat : " + _fonderie.craft.recipe.output.getResource().GetName();
            foreach (StackItem stack in _fonderie.craft.recipe.input)
            {
                _craftText.Text += "\n - " + stack.getStack() + " x " + stack.getResource().GetName();
            }
        }
        else
        {
            _craftText.Text = "Aucune recette en cours.";
        }
    }

    private void UpdateOutputSlotBackground()
    {
        if (_outputSlot != null && _fonderie.craft != null && _fonderie.craft.recipe != null)
        {
            _outputSlot.setBackgroundTexture(_fonderie.craft.recipe.output.getResource().getInventoryIcon);
        }
    }

    public void UpdateUi()
    {
        for (int i = 0; i < _inputSlots.Count; i++)
        {
            _inputSlots[i].initialize(_fonderie.input.slots[i], _fonderie.input);
        }

        _outputSlot.initialize(_fonderie.output.slots[0], _fonderie.output, true);
        UpdateOutputSlotBackground();
        UpdateCraftText();
    }

    public void UpdateProgressBar(float progress)
    {
        // Implémentez la mise à jour de la barre de progression ici
    }

}
