using Godot;

public class Craft
{
	public Recipe recipe;
	private Inventory _input;
	private Inventory _output;

	public Craft() {}

	public void init(Inventory input, Inventory output)
	{
		_input = input;
		_output = output;
	}

	/// <summary>
	/// Compare l'inventaire de la fonderie avec la recette.
	/// </summary>
	/// <param name="invFactory">L'inventaire de la fonderie.</param>
	/// <returns>True si l'inventaire correspond à la recette, sinon False.</returns>
	public bool compareRecipe()
	{
		if (recipe == null || recipe.input == null || _input == null)
		{
			GD.PrintErr("Recette ou inventaire invalide.");
			return false;
		}
		GD.Print(recipe.input.Count);
		for (int i = 0; i < recipe.input.Count; i++)
		{
			var requiredItem = recipe.input[i];
			var slotItem = _input.getItem(i); // Récupère l'item dans le slot correspondant

			if (slotItem == null || slotItem.getResource() != requiredItem.getResource() || slotItem.getStack() < requiredItem.getStack())
			{
				GD.Print($"Slot {i} ne correspond pas à la recette.");
				return false;
			}
		}

		// Tous les slots correspondent à la recette
		GD.Print("La recette correspond à l'inventaire.");
		return true;
	}

	/// <summary>
	/// Consomme les ressources nécessaires pour le craft.
	/// </summary>
	/// <returns>True si les ressources ont été consommées avec succès, sinon False.</returns>
	public bool consumeResources()
	{
		if (_output.getItem(0) != null)
		{
			int outputStack = _output.getItem(0).getStack();
			int itemMaxStack = _output.getItem(0).getResource().getMaxStack;
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
			var slotItem = _input.getItem(i); 
			if (slotItem != null && slotItem.getResource() == requiredItem.getResource())
			{
				// Supprime la quantité nécessaire de l'item
				slotItem.subtract(requiredItem.getStack());

				// Si le slot est vide après suppression, on le met à null
				if (slotItem.isEmpty())
				{
					_input.deleteItem(i);
				}
			}
			else
			{
				GD.PrintErr($"Erreur : Le slot {i} ne contient pas l'item requis.");
				return false;
			}
		}

		GD.Print("Ressources consommées avec succès.");
		return true;
	}

	/// <summary>
	/// Ajoute l'item de sortie dans l'inventaire de sortie, en respectant le stackMax.
	/// </summary>
	/// <returns>True si l'output a été ajouté avec succès, sinon False.</returns>
	public bool addOutput()
	{
		if (recipe == null || recipe.output == null || _output == null)
		{
			GD.PrintErr("Recette ou inventaire de sortie invalide.");
			return false;
		}

		var recipeItem = recipe.output;
		var outputSlotItem = _output.getItem(0);

		if (outputSlotItem == null)
		{
			// Si le slot est vide, on ajoute l'item directement
			_output.addItem(new StackItem(recipeItem.getResource(), recipeItem.getStack()), 0);
			return true;
		}
		else if (outputSlotItem.getResource() == recipeItem.getResource())
		{
			// Si le slot contient déjà le même item, on essaie d'ajouter la quantité
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
