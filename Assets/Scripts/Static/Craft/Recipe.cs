using System.Collections.Generic;
using Godot;

public partial class Recipe : Resource
{
    [Export] private Godot.Collections.Array<Godot.Collections.Array> _input = new();
    [Export] private Godot.Collections.Array<Godot.Collections.Array> _output = new();

    public List<StackItem> input { get; private set; } = new();
    public StackItem output { get; private set; }

    [Export] public float duration { get; set; }

    public Recipe()
    {
        initRecipe();
    }

    /// <summary>
    /// Initialise la recette en préparant les inputs et outputs.
    /// </summary>
    public void initRecipe()
    {
        initInput();
        initOutput();
    }

    private void initInput()
    {
        input = new List<StackItem>();
        foreach (var inp in _input)
        {
            int quantity = (int)inp[0];
            ItemStatic itemStatic = (ItemStatic)inp[1];
            StackItem stackItem = new StackItem(itemStatic, quantity);
            input.Add(stackItem);
        }
    }

    private void initOutput()
    {
        output = null;
        if (_output.Count == 1)
        {
            int quantity = (int)_output[0][0];
            ItemStatic itemStatic = (ItemStatic)_output[0][1];
            output = new StackItem(itemStatic, quantity);
        }
    }
}