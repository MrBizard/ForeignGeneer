using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class FonderieUi : Control
{
    [Export] private PackedScene _slotUi;
    private Fonderie _fonderie;
    private List<SlotUI> _inputSlots = new();
    private SlotUI _outputSlot;
    private ProgressBar _craftProgressBar;
    private VBoxContainer _inputList;
    private VBoxContainer _outputContainer;
    private HBoxContainer _mainContainer;
    private PlayerInventoryManager _playerInventoryManager;
    private TextEdit _craftText;

    /// <summary>
    /// Initialise l'interface utilisateur de la fonderie avec la fonderie et l'interface d'inventaire fournies.
    /// </summary>
    /// <param name="fonderie">La fonderie associée à cette interface.</param>
    /// <param name="inventoryUi">L'interface d'inventaire à afficher à côté de la fonderie.</param>
    public void initialize(Fonderie fonderie, InventoryUi inventoryUi)
    {
        _fonderie = fonderie;

        // Initialiser les slots d'entrée
        for (int i = 0; i < _fonderie.input.slots.Count; i++)
        {
            var slot = _slotUi.Instantiate<SlotUI>();
            slot.initialize(_fonderie.input.slots[i], _fonderie.input, _playerInventoryManager);
            slot.setBackgroundTexture(_fonderie.craft.recipe.input[i].getResource().getInventoryIcon);
            _inputSlots.Add(slot);
            _inputList.AddChild(slot);
        }

        // Initialiser le slot de sortie
        _outputSlot = _slotUi.Instantiate<SlotUI>();
        _outputSlot.initialize(_fonderie.output.slots[0], _fonderie.output, _playerInventoryManager, true);
        _outputContainer.AddChild(_outputSlot);

        // Afficher l'inventaire à gauche de la fonderie
        if (inventoryUi != null)
        {
            inventoryUi.Visible = true;
            inventoryUi.Position = new Vector2(Position.X - inventoryUi.Size.X - 10, Position.Y);
        }

        // Activer le curseur
        Input.MouseMode = Input.MouseModeEnum.Visible;

        // Afficher les détails de la recette dans le TextEdit
        _craftText.Text = "Resultat : " + _fonderie.craft.recipe.output.getResource().GetName();
        foreach (StackItem stack in _fonderie.craft.recipe.input)
        {
            _craftText.Text += "\n - " + stack.getStack() + " x " + stack.getResource().GetName();
        }
    }

    /// <summary>
    /// Met à jour l'interface utilisateur de la fonderie.
    /// </summary>
    public void updateUi()
    {
        // Mettre à jour les slots d'entrée
        for (int i = 0; i < _inputSlots.Count; i++)
        {
            var slot = _inputSlots[i];
            var stackItem = _fonderie.input.getItem(i);
            slot.initialize(stackItem, _fonderie.input, _playerInventoryManager);
            slot.updateSlot();
        }

        // Mettre à jour le slot de sortie
        var outputStackItem = _fonderie.output.getItem(0);
        if (outputStackItem != null)
        {
            GD.Print($"Mise à jour du slot de sortie avec : {outputStackItem.getResource().GetName()} (x{outputStackItem.getStack()})");
        }
        else
        {
            GD.Print("Le slot de sortie est vide.");
        }
        _outputSlot.initialize(outputStackItem, _fonderie.output, _playerInventoryManager, true);
        _outputSlot.updateSlot();
    }

    /// <summary>
    /// Appelé lorsque le nœud est prêt. Initialise les références aux composants de l'interface.
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
        _mainContainer = GetNode<HBoxContainer>("Container");
        _inputList = GetNode<VBoxContainer>("Container/InputList");
        _outputContainer = GetNode<VBoxContainer>("Container/OutputContainer");
        _craftProgressBar = GetNode<ProgressBar>("Container/ProgressBar");
        _playerInventoryManager = GetNode<PlayerInventoryManager>("/root/Main/PlayerInventoryManager");
        _craftText = GetNode<TextEdit>("CraftText");
    }

    /// <summary>
    /// Met à jour la barre de progression de la fonderie.
    /// </summary>
    /// <param name="progress">La progression actuelle (entre 0 et 1).</param>
    public void updateProgressBar(float progress)
    {
        if (_craftProgressBar != null)
        {
            _craftProgressBar.Value = progress * 100; // Convertit la progression en pourcentage
        }
    }

    /// <summary>
    /// Ferme l'interface utilisateur de la fonderie et libère les ressources.
    /// </summary>
    public void closeUi()
    {
        Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        QueueFree();
    }
}