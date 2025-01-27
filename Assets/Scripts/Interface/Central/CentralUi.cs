using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.block.playerStructure.central;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class CentralUi : BaseUi
{
    [Export] private PackedScene _slotUi;
    private CoalCentral _central;
    private SlotUI _inputSlot;
    private ProgressBar _craftProgressBar;
    private HBoxContainer _inputList;
    private VBoxContainer _mainContainer;
    private TextEdit _craftText;
    private Button _resetCraftButton;
    private Label _electricityLabel;
    private Manager _manager;

    public override void _Ready()
    {
        base._Ready();
        _mainContainer = GetNode<VBoxContainer>("Machine/Container");
        _inputList = GetNode<HBoxContainer>("Machine/Container/InputList");
        _craftProgressBar = GetNode<ProgressBar>("Machine/Container/ProgressBar");
        _craftText = GetNode<TextEdit>("Machine/CraftText");
        _electricityLabel = GetNode<Label>("Machine/Quantity");
        _manager = GetNode<Manager>("/root/Main/Manager");
        _resetCraftButton = GetNode<Button>("Machine/Button");
        _resetCraftButton.Connect("pressed", new Callable(this, nameof(onResetCraftButtonPressed)));
    }

    /// <summary>
    /// Initialise l'interface utilisateur de la centrale.
    /// </summary>
    /// <param name="data">La centrale associée à cette interface.</param>
    public override void initialize(Node data)
    {
        _central = (CoalCentral)data;

        _inputSlot = _slotUi.Instantiate<SlotUI>();
        _inputSlot.initialize(_central.input.slots[0], _central.input);
        _inputSlot.setBackgroundTexture(_central.craft.recipe.input[0].getResource().getInventoryIcon);
        _inputList.AddChild(_inputSlot);

        Input.MouseMode = Input.MouseModeEnum.Visible;

        _craftText.Text = "Résultat : Énergie";
        foreach (StackItem stack in _central.craft.recipe.input)
        {
            _craftText.Text += "\n - " + stack.getStack() + " x " + stack.getResource().GetName();
        }
        updateElectricity();
    }

    /// <summary>
    /// Met à jour l'interface utilisateur de la centrale.
    /// </summary>
    public override void updateUi()
    {
        var inputStackItem = _central.input.getItem(0);
        _inputSlot.initialize(inputStackItem, _central.input);
        _inputSlot.updateSlot();
    }

    /// <summary>
    /// Met à jour la barre de progression de la centrale.
    /// </summary>
    /// <param name="progress">La progression actuelle (entre 0 et 1).</param>
    public void updateProgressBar(float progress)
    {
        if (_craftProgressBar != null)
        {
            _craftProgressBar.Value = progress * 100;
        }
    }

    /// <summary>
    /// Met à jour l'affichage de l'électricité.
    /// </summary>
    public void updateElectricity()
    {
        if (_electricityLabel != null && _central != null)
        {
            _electricityLabel.Text = $"Puissance : {_central.electricalCost} kW/s || Électricité totale : {_manager.getGlobalElectricity()} kW/s";
        }
    }

    private void onResetCraftButtonPressed()
    {
        _central.setCraft(null);
        updateUi();
    }
}