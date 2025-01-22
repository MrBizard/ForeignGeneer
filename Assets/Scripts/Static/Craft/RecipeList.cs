using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class RecipeList : Resource
{
    [Export] public Array<Recipe> recipeList { get; set; }

    /// <summary>
    /// Initialise toutes les recettes dans la liste.
    /// </summary>
    public void init()
    {
        foreach (Recipe recipe in recipeList)
        {
            recipe.initRecipe();
        }
    }
}