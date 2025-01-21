using System.Collections.Generic;
using Godot;

public partial class Recipe : Resource
{
    // Champs privés
    [Export] private Godot.Collections.Array<Godot.Collections.Array> _input = new();
    [Export] private Godot.Collections.Array<Godot.Collections.Array> _output = new();

    // Propriétés publiques pour l'input et l'output en StackItem
    public List<StackItem> input { get; private set; } = new();
    public StackItem output { get; private set; }

    // Propriété publique pour la durée
    [Export] public float duration { get; set; }

    // Constructeur
    public Recipe()
    {
    }

    public void initRecipe()
    {
        initInput();
        initOutput();
        
    }
    private void initInput()
    {
        foreach (var inp in _input)
        {
            int quantity = (int)inp[0];
            ItemStatic itemStatic = (ItemStatic)inp[1];
            StackItem stackItem = new StackItem(itemStatic, quantity);
            input.Add(stackItem);
        }
        GD.Print("count input" + input.Count);
    }
    private void initOutput()
    {
        if (_output.Count == 1)
        {
            int quantity = (int)_output[0][0];
            ItemStatic itemStatic = (ItemStatic)_output[0][1];
            output = new StackItem(itemStatic, quantity);
        }
    }
}