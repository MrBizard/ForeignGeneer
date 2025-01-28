using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;

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
        GD.Print("progress : ", _progressBar);
        _button.Connect("pressed", new Callable(this,nameof(onButtonBack) ));
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

            initializeInputSlots();
            initializeOutputSlot();
            updateCraftText();
            updateUi();
        }
        else
        {
            GD.PrintErr("FonderieUi: Data is not of type Fonderie.");
        }
    }

    private void onInventoryUpdated()
    {
        updateUi();
        updateCraftText();
        updateOutputSlotBackground();
    }

    private void initializeInputSlots()
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

    private void initializeOutputSlot()
    {
        foreach (var child in _outputContainer.GetChildren())
        {
            child.QueueFree(); // Supprime les anciens enfants
        }

        _outputSlot = _slotUi.Instantiate<SlotUI>(); // Instancie le nouveau slot
        _outputSlot.initialize(_fonderie.output.slots[0], _fonderie.output, true); // Initialise le slot
        updateOutputSlotBackground(); // Met à jour l'arrière-plan du slot
        _outputContainer.AddChild(_outputSlot); // Ajoute le slot à l'interface
    }

    private void updateCraftText()
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

    private void updateOutputSlotBackground()
    {
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
                    _inputSlots[i].updateSlot();
                }
                else
                {
                    _inputSlots[i].clearSlot();
                }
            }
        }
        GD.Print("outputSlot : ",_outputSlot);
        // Mettre à jour le slot de sortie
        if (_outputSlot != null)
        {
            var outputItem = _fonderie.output.getItem(0);
            _outputSlot.setStackItem(outputItem);
            if (outputItem != null)
            {
                GD.Print("Mise à jour du slot de sortie avec l'item : " + outputItem.getResource().GetName());
                _outputSlot.updateSlot();
            }
            else
            {
                GD.Print("Slot de sortie vide.");
                _outputSlot.clearSlot();
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