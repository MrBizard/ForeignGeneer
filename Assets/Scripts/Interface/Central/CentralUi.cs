using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.block.playerStructure.central;

public partial class CentralUi : Control
{
    [Export] private PackedScene _slotUi;
    private CoalCentral _central;
    private SlotUI _inputSlot;
    private ProgressBar _craftProgressBar;
    private HBoxContainer _inputList;
    private VBoxContainer _mainContainer;
    private PlayerInventoryManager _playerInventoryManager;
    private TextEdit _craftText;
    private Button _resetCraftButton;
    private Label _electricityLabel;
    private Manager _manager;
    /// <summary>
    /// Initialise l'interface utilisateur de la centrale.
    /// </summary>
    /// <param name="central">La centrale associée à cette interface.</param>
    /// <param name="inventoryUi">L'interface d'inventaire à afficher à côté de la centrale.</param>
    public void initialize(CoalCentral central, InventoryUi inventoryUi)
    {
        _central = central;

        // Initialiser l'emplacement d'entrée
        _inputSlot = _slotUi.Instantiate<SlotUI>();
        _inputSlot.initialize(_central.input.slots[0], _central.input, _playerInventoryManager);
        _inputSlot.setBackgroundTexture(_central.craft.recipe.input[0].getResource().getInventoryIcon);
        _inputList.AddChild(_inputSlot);

        if (inventoryUi != null)
        {
            inventoryUi.Visible = true;
            inventoryUi.Position = new Vector2(Position.X - inventoryUi.Size.X - 10, Position.Y);
        }

        Input.MouseMode = Input.MouseModeEnum.Visible;

        // Mettre à jour le texte de craft
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
    public void updateUi()
    {
        var inputStackItem = _central.input.getItem(0);
        _inputSlot.initialize(inputStackItem, _central.input, _playerInventoryManager);
        _inputSlot.updateSlot();
    }

    public override void _Ready()
    {
        base._Ready();
        _mainContainer = GetNode<VBoxContainer>("Container");
        _inputList = GetNode<HBoxContainer>("Container/InputList");
        _craftProgressBar = GetNode<ProgressBar>("Container/ProgressBar");
        _playerInventoryManager = GetNode<PlayerInventoryManager>("/root/Main/PlayerInventoryManager");
        _craftText = GetNode<TextEdit>("CraftText");
        _electricityLabel = GetNode<Label>("Quantity");
        _manager = GetNode<Manager>("/root/Main/Manager");
        _resetCraftButton = GetNode<Button>("Button");
        _resetCraftButton.Connect("pressed", new Callable(this, nameof(onResetCraftButtonPressed)));
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
    /// <param name="electricity">La quantité d'électricité à afficher.</param>
    public void updateElectricity()
    {
        if (_electricityLabel != null)
        {
            GD.Print("central ",_central);
            _electricityLabel.Text = $"Puissance : {_central.electricalCost} kW/s|| Électricité totale : {_manager.getGlobalElectricity()} kW/s";
        }
    }

    /// <summary>
    /// Ferme l'interface utilisateur de la centrale.
    /// </summary>
    public void closeUi()
    {
        Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        QueueFree();
    }

    private void onResetCraftButtonPressed()
    {
        _central.setCraft(null);
        _central.openUi();
    }
}