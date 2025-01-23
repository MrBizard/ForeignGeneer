using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class RecetteList : Control
{
    [Export] private PackedScene _recipeUiPacked;
    private GridContainer _scrollContainer;
    private IRecipeUser _recipeUser;

    public override void _Ready()
    {
        _scrollContainer = GetNode<GridContainer>("ScrollContainer/GridContainer");
    }

    /// <summary>
    /// Initialise la liste des recettes.
    /// </summary>
    /// <param name="factory">L'usine associée à cette liste de recettes.</param>
    public void initialize(IRecipeUser recipeUser)
    {
        _recipeUser = recipeUser;

        if (_recipeUser.recipeList.recipeList == null || _recipeUser.recipeList.recipeList.Count == 0)
        {
            return;
        }

        foreach (Recipe recipe in _recipeUser.recipeList.recipeList)
        {
            RecipeChoiceUi recipeUi = _recipeUiPacked.Instantiate<RecipeChoiceUi>();
            recipeUi.init(recipe);
            recipeUi.Connect(nameof(RecipeChoiceUi.RecipeClicked), new Callable(this, nameof(onRecipeClicked)));
            _scrollContainer.AddChild(recipeUi);
        }
    }

    private void onRecipeClicked(Recipe recipe)
    {
        _recipeUser.setCraft(recipe);
    }
}