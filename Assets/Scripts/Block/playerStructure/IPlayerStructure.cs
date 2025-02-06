using ForeignGeneer.Assets.Scripts.Static.Craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure;

public interface IPlayerStructure
{
    public void dismantle();
    public void openUi();
    void closeUi();
    RecipeList recipeList { get; set; } 
    void setCraft(Recipe recipe);
}