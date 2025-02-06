using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using Godot;
using ForeignGeneer.Assets.Scripts.Interface;
using ForeignGeneer.Assets.Scripts.manager;

public partial class CentralUi : BaseUi
{
    [Export] private PackedScene _slotUi;
    private IFactory _central;
    private SlotUI _inputSlot;
    private ProgressBar _craftProgressBar;
    private HBoxContainer _inputList;
    private VBoxContainer _mainContainer;
    private TextEdit _craftText;
    private Button _resetCraftButton;
    private Label _electricityLabel;

    public override void _Ready()
    {
        base._Ready();
        _mainContainer = GetNode<VBoxContainer>("Machine/Container");
        _inputList = GetNode<HBoxContainer>("Machine/Container/InputList");
        _craftProgressBar = GetNode<ProgressBar>("Machine/Container/ProgressBar");
        _craftText = GetNode<TextEdit>("Machine/CraftText");
        _electricityLabel = GetNode<Label>("Machine/Quantity");
        _resetCraftButton = GetNode<Button>("Machine/Button");
        _resetCraftButton.Connect("pressed", new Callable(this, nameof(onResetCraftButtonPressed)));
    }

    /// <summary>
    /// Initializes the central interface. This method should be called once to initialize the UI.
    /// </summary>
    /// <param name="data">The central node associated with this interface.</param>
    public override void initialize(Node data)
    {
        _central = (Central)data;

        if (_inputSlot == null)
        {
            _inputSlot = _slotUi.Instantiate<SlotUI>();
            _inputSlot.initialize(_central.input, 0);
            _inputSlot.setBackgroundTexture(_central.craft.recipe.input[0].getResource().getInventoryIcon);
            _inputList.AddChild(_inputSlot);
        }

        _craftText.Text = "Résultat : Énergie";
        foreach (StackItem stack in _central.craft.recipe.input)
        {
            _craftText.Text += $"\n - {stack.getStack()} x {stack.getResource().GetName()}";
        }

        updateElectricity();
    }

    /// <summary>
    /// Updates the central interface UI. This method is called on each UI update.
    /// </summary>
    public override void updateUi()
    {
        var inputStackItem = _central.input.getItem(0);
        _inputSlot.updateSlot();
    }

    public override void close()
    {
        _central.closeUi();
    }

    /// <summary>
    /// Updates the progress bar of the central.
    /// </summary>
    /// <param name="progress">The current progress (between 0 and 1).</param>
    public void updateProgressBar(float progress)
    {
        if (_craftProgressBar != null)
        {
            _craftProgressBar.Value = progress * 100;
        }
    }

    /// <summary>
    /// Updates the electricity display.
    /// </summary>
    public void updateElectricity()
    {
        if (_electricityLabel != null && _central != null)
        {
            _electricityLabel.Text = $"Puissance : {_central.factoryStatic.electricalCost} kW/s || Électricité totale : {EnergyManager.instance.getGlobalElectricity()} kW/s";
        }
    }
    /// <summary>
    /// Bouton retour
    /// </summary>
    private void onResetCraftButtonPressed()
    {
        _central.setCraft(null);
        updateUi();
    }
}
