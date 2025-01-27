using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class RecetteList : BaseUi
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
    /// <param name="data">L'usine associée à cette liste de recettes.</param>
    public override void initialize(Node data)
    {
        _recipeUser = (IRecipeUser)data;

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

    public override void updateUi()
    {
        throw new NotImplementedException();
    }

    private void onRecipeClicked(Recipe recipe)
    {
        _recipeUser.setCraft(recipe);
    }
}