using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class RecetteList : Control
{
    [Export] private PackedScene recipeUiPacked;
    private GridContainer scrollContainer;
    private IFactory factory;
    public override void _Ready()
    {
        // Récupérer le GridContainer
        scrollContainer = GetNode<GridContainer>("ScrollContainer/GridContainer");
    }

    public void Initialize(IFactory factory)
    {
        this.factory = factory;
        // Vérifier que la liste de recettes n'est pas vide
        if (factory.recipeList.recipeList == null || factory.recipeList.recipeList.Count == 0)
        {
            GD.PrintErr("La liste de recettes est vide.");
            return;
        }

        // Parcourir chaque recette dans la liste
        foreach (Recipe recipe in factory.recipeList.recipeList)
        {
            // Instancier l'interface utilisateur de la recette
            RecipeChoiceUi recipeUi = recipeUiPacked.Instantiate<RecipeChoiceUi>();
            recipeUi.init(recipe);

            // Connecter le signal RecipeClicked en utilisant Callable
            recipeUi.Connect(nameof(RecipeChoiceUi.RecipeClicked), new Callable(this, nameof(OnRecipeClicked)));

            // Ajouter l'objet au GridContainer
            scrollContainer.AddChild(recipeUi);
        }
    }

    private void OnRecipeClicked(Recipe recipe)
    {
        factory.setCraft(recipe);
        GD.Print("clique ");
    }
}