using Godot;

/// <summary>
/// Représente un processus de craft, comprenant une recette, un inventaire d'entrée et un inventaire de sortie.
/// </summary>
public class Craft
{
    /// <summary>
    /// La recette utilisée pour ce craft.
    /// </summary>
    public Recipe recipe;

    private Inventory input;
    private Inventory output;

    /// <summary>
    /// Initialise une nouvelle instance de la classe Craft avec la recette spécifiée.
    /// </summary>
    /// <param name="recipe">La recette à utiliser pour ce craft.</param>
    public Craft(Recipe recipe)
    {
        this.recipe = recipe;
    }

    /// <summary>
    /// Initialise les inventaires d'entrée et de sortie pour ce craft.
    /// </summary>
    /// <param name="input">L'inventaire d'entrée.</param>
    /// <param name="output">L'inventaire de sortie.</param>
    public void init(Inventory input, Inventory output)
    {
        this.input = input;
        this.output = output;
    }

    /// <summary>
    /// Vérifie si l'inventaire d'entrée correspond aux exigences de la recette.
    /// </summary>
    /// <returns>True si l'inventaire correspond à la recette, sinon False.</returns>
    public bool compareRecipe()
    {
        if (recipe == null || recipe.input == null || input == null)
        {
            GD.PrintErr("Recette ou inventaire invalide.");
            return false;
        }

        for (int i = 0; i < recipe.input.Count; i++)
        {
            var requiredItem = recipe.input[i];
            var slotItem = input.getItem(i);

            if (slotItem == null || slotItem.getResource() != requiredItem.getResource() || slotItem.getStack() < requiredItem.getStack())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Consomme les ressources nécessaires pour le craft.
    /// </summary>
    /// <returns>True si les ressources ont été consommées avec succès, sinon False.</returns>
    public bool consumeResources()
    {
        if (output.getItem(0) != null)
        {
            int outputStack = output.getItem(0).getStack();
            int itemMaxStack = output.getItem(0).getResource().getMaxStack;
            if (outputStack + recipe.output.getStack() > itemMaxStack)
            {
                return false;
            }
        }

        if (!compareRecipe())
        {
            return false;
        }

        for (int i = 0; i < recipe.input.Count; i++)
        {
            var requiredItem = recipe.input[i];
            var slotItem = input.getItem(i);

            if (slotItem != null && slotItem.getResource() == requiredItem.getResource())
            {
                slotItem.subtract(requiredItem.getStack());

                if (slotItem.isEmpty())
                {
                    input.deleteItem(i);
                }
            }
            else
            {
                GD.PrintErr($"Erreur : Le slot {i} ne contient pas l'item requis.");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Ajoute l'item de sortie dans l'inventaire de sortie, en respectant la limite de stack.
    /// </summary>
    /// <returns>True si l'output a été ajouté avec succès, sinon False.</returns>
    public bool addOutput()
    {
        if (recipe == null || recipe.output == null || output == null)
        {
            GD.PrintErr("Recette ou inventaire de sortie invalide.");
            return false;
        }

        var recipeItem = recipe.output;
        var outputSlotItem = output.getItem(0);

        if (outputSlotItem == null)
        {
            output.addItemToSlot(new StackItem(recipeItem.getResource(), recipeItem.getStack()), 0);
            return true;
        }
        else if (outputSlotItem.getResource() == recipeItem.getResource())
        {
            int remainingSpace = outputSlotItem.getResource().getMaxStack - outputSlotItem.getStack();
            if (remainingSpace >= recipeItem.getStack())
            {
                outputSlotItem.add(recipeItem.getStack());
                return true;
            }
            else
            {
                GD.Print("Le slot de sortie est plein. Impossible d'ajouter l'output.");
                return false;
            }
        }
        else
        {
            GD.Print("Le slot de sortie contient un item différent.");
            return false;
        }
    }
}