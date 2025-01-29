using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;
using Godot;

public partial class FonderieUi : BaseUi
{
    private Fonderie _fonderie;
    [Export] private PackedScene _slotUi;
    private VBoxContainer _inputList;
    private VBoxContainer _outputContainer;
    private TextEdit _craftText;
    private SlotUI _outputSlot;
    private ProgressBar _progressBar;
    private List<SlotUI> _inputSlots = new List<SlotUI>();
    private Button _button;

    public override void _Ready()
    {
        base._Ready();
        _inputList = GetNode<VBoxContainer>("Machine/Container/InputList");
        _outputContainer = GetNode<VBoxContainer>("Machine/Container/OutputContainer");
        _craftText = GetNode<TextEdit>("Machine/CraftText");
        _progressBar = GetNode<ProgressBar>("Machine/Container/ProgressBar");
        _button = GetNode<Button>("Machine/Button");
        _button.Connect("pressed", new Callable(this, nameof(onButtonBack)));
    }

    /// <summary>
    /// Initialise l'interface avec les données de la fonderie.
    /// </summary>
    /// <param name="data">La fonderie à afficher.</param>
    public override void initialize(Node data)
    {
        if (data is Fonderie fonderie)
        {
            _fonderie = fonderie;

            if (_fonderie.input != null && _fonderie.output != null)
            {
                _fonderie.input.onInventoryUpdated += onInventoryUpdated;
                _fonderie.output.onInventoryUpdated += onInventoryUpdated;
            }

            initializeInputSlots();  // Initialise les slots d'entrée
            initializeOutputSlot();  // Initialise le slot de sortie
            updateCraftText();       // Met à jour le texte de la recette
            updateUi();              // Met à jour l'UI
        }
        else
        {
            GD.PrintErr("FonderieUi: Data is not of type Fonderie.");
        }
    }

    private void onInventoryUpdated()
    {
        updateUi();  // Met à jour l'UI lorsque l'inventaire change
        updateCraftText();  // Met à jour le texte de la recette
        updateOutputSlotBackground();  // Met à jour l'arrière-plan du slot de sortie
    }

    private void initializeInputSlots()
    {
        // On vide les anciens slots d'entrée
        foreach (var child in _inputList.GetChildren())
        {
            child.QueueFree();
        }
        _inputSlots.Clear();

        // On initialise les slots d'entrée avec l'inventaire de la fonderie
        for (int i = 0; i < _fonderie.input.slots.Count; i++)
        {
            var slot = _slotUi.Instantiate<SlotUI>();
            slot.initialize(_fonderie.input, i);
            slot.setBackgroundTexture(_fonderie.craft.recipe.input[i].getResource().getInventoryIcon);
            _inputSlots.Add(slot);
            _inputList.AddChild(slot);
        }
    }

    private void initializeOutputSlot()
    {
        // On vide les anciens enfants du container de sortie
        foreach (var child in _outputContainer.GetChildren())
        {
            child.QueueFree();
        }

        // On instancie et initialise le slot de sortie
        _outputSlot = _slotUi.Instantiate<SlotUI>();
        _outputSlot.initialize(_fonderie.output, 0, true);  // True signifie qu'il est un slot de sortie
        updateOutputSlotBackground();  // Met à jour l'arrière-plan du slot
        _outputContainer.AddChild(_outputSlot);  // Ajoute le slot au container de sortie
    }

    private void updateCraftText()
    {
        // Met à jour le texte pour la recette en cours
        if (_fonderie.craft != null && _fonderie.craft.recipe != null)
        {
            _craftText.Text = "Résultat : " + _fonderie.craft.recipe.output.getResource().GetName();
            foreach (StackItem stack in _fonderie.craft.recipe.input)
            {
                _craftText.Text += $"\n - {stack.getStack()} x {stack.getResource().GetName()}";
            }
        }
        else
        {
            _craftText.Text = "Aucune recette en cours.";
        }
    }

    private void updateOutputSlotBackground()
    {
        // Met à jour l'arrière-plan du slot de sortie avec l'icône de la ressource de sortie
        if (_outputSlot != null && _fonderie.craft != null && _fonderie.craft.recipe != null)
        {
            _outputSlot.setBackgroundTexture(_fonderie.craft.recipe.output.getResource().getInventoryIcon);
        }
    }

    /// <summary>
    /// Met à jour l'interface utilisateur.
    /// </summary>
    public override void updateUi()
    {
        if (_fonderie == null)
            return;

        // Mettre à jour les slots d'entrée
        for (int i = 0; i < _fonderie.input.slots.Count; i++)
        {
            if (_inputSlots[i] != null)
            {
                var slotItem = _fonderie.input.getItem(i);
                if (slotItem != null)
                {
                    _inputSlots[i].updateSlot();  // Met à jour le contenu du slot
                }
                else
                {
                    _inputSlots[i].clearSlot();  // Vide le slot si nécessaire
                }
            }
        }

        // Mettre à jour le slot de sortie
        if (_outputSlot != null)
        {
            var outputItem = _fonderie.output.getItem(0);
            if (outputItem != null)
            {
                _outputSlot.updateSlot();  // Met à jour le contenu du slot de sortie
            }
            else
            {
                _outputSlot.clearSlot();  // Vide le slot de sortie si nécessaire
            }
        }
    }

    /// <summary>
    /// Met à jour la barre de progression du craft.
    /// </summary>
    /// <param name="progress">La progression actuelle (entre 0 et 1).</param>
    public void updateProgressBar(float progress)
    {
        _progressBar.Value = progress * 100;
    }

    public override void _ExitTree()
    {
        if (_fonderie != null)
        {
            _fonderie.input.onInventoryUpdated -= onInventoryUpdated;
            _fonderie.output.onInventoryUpdated -= onInventoryUpdated;
        }
        base._ExitTree();
    }

    private void onButtonBack()
    {
        UiManager.instance.openUi("RecipeListUI", _fonderie);
    }
}
