using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class RecetteList : Control,BaseUi
{
	[Export] private PackedScene _recipeUiPacked;
	private GridContainer _scrollContainer;
	private IInputFactory<CraftingFactoryStatic> _recipeUser;

	public override void _Ready()
	{
		_scrollContainer = GetNode<GridContainer>("ScrollContainer/GridContainer");
	}

	/// <summary>
	/// Initialise la liste des recettes.
	/// </summary>
	/// <param name="data">L'usine associée à cette liste de recettes.</param>
	public void initialize(Object data)
	{
		_recipeUser = (IInputFactory<CraftingFactoryStatic>)data;

		if (_recipeUser.factoryStatic.recipeList == null || _recipeUser.factoryStatic.recipeList.recipeList.Count == 0)
		{
			return;
		}

		foreach (Recipe recipe in _recipeUser.factoryStatic.recipeList.recipeList)
		{
			RecipeChoiceUi recipeUi = _recipeUiPacked.Instantiate<RecipeChoiceUi>();
			recipeUi.init(recipe);
			recipeUi.Connect(nameof(RecipeChoiceUi.RecipeClicked), new Callable(this, nameof(onRecipeClicked)));
			_scrollContainer.AddChild(recipeUi);
		}
	}

	public void updateUi(int updateType = 0)
	{
		
	}

	public void close()
	{
		_recipeUser.closeUi();
	}

	private void onRecipeClicked(Recipe recipe)
	{
		_recipeUser.setCraft(recipe);
	}
}
