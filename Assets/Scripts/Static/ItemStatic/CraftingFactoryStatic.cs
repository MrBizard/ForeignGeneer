using Godot;
using Godot.Collections;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(CraftingFactoryStatic), "", nameof(FactoryStatic))]
public partial class CraftingFactoryStatic : FactoryStatic
{
    [Export] public Array<Recipe> recipeList { get; set; }

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