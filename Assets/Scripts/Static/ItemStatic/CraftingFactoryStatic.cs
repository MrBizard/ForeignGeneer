using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(CraftingFactoryStatic), "", nameof(FactoryStatic))]
public partial class CraftingFactoryStatic : FactoryStatic
{
    [Export] public RecipeList recipeList { get; set; } // Liste de recettes

    /// <summary>
    /// Starts the crafting process.
    /// </summary>
    public void StartCrafting()
    {
        if (recipeList != null)
        {
            GD.Print("Crafting started with recipe list");
            // Logique de production ici
        }
    }
}