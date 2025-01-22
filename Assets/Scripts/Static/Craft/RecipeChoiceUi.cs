using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class RecipeChoiceUi : Node
{
    [Signal]
    public delegate void RecipeClickedEventHandler(Recipe recipe);

    public Recipe recipe { get; set; }

    public override void _Ready()
    {
        Button button = GetNode<Button>("Button");
        button.Connect("pressed", new Callable(this, nameof(onButtonPressed)));
    }

    /// <summary>
    /// Initialise l'interface avec une recette.
    /// </summary>
    /// <param name="recipe">La recette à afficher.</param>
    public void init(Recipe recipe)
    {
        this.recipe = recipe;

        Label nameItem = GetNode<Label>("Background/NameItem");
        TextureRect textureItem = GetNode<TextureRect>("Background/TextureItem");

        nameItem.Text = recipe.output.getResource().GetName();
        textureItem.Texture = recipe.output.getResource().getInventoryIcon;
    }

    private void onButtonPressed()
    {
        EmitSignal(nameof(RecipeClicked), recipe);
    }
}