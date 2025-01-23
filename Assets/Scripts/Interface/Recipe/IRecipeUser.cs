using Godot;
using ForeignGeneer.Assets.Scripts.Static.Craft;


public interface IRecipeUser
{
        RecipeList recipeList { get; set; } 
        void setCraft(Recipe recipe);
}
