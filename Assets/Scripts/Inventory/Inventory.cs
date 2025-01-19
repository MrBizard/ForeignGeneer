using System.Collections.Generic;

/// <summary>
/// Classe représentant l'inventaire d'un joueur.
/// Gère les slots, les items et les interactions avec les items.
/// </summary>
public class Inventory
{
	public List<StackItem> slots { get; private set; }

	/// <summary>
	/// Constructeur de l'inventaire.
	/// Initialise les slots avec une taille donnée.
	/// </summary>
	/// <param name="slotCount">Nombre de slots dans l'inventaire.</param>
	public Inventory(int slotCount)
	{
		slots = new List<StackItem>(slotCount);
		for (int i = 0; i < slotCount; i++)
		{
			slots.Add(null);
		}
	}

	/// <summary>
	/// Ajoute un item à un slot spécifique de l'inventaire.
	/// </summary>
	/// <param name="item">Item à ajouter.</param>
	/// <param name="slotIndex">Index du slot cible.</param>
	/// <returns>True si l'ajout est réussi, sinon False.</returns>
	public bool addItem(StackItem item, int slotIndex)
	{
		if (slotIndex >= slots.Count)
			return false;
		if (slots[slotIndex] == null)
		{
			slots[slotIndex] = item;
			return true;
		}
		else if (slots[slotIndex].getResource() == item.getResource())
		{
			int remaining = slots[slotIndex].add(item.getStack());
			if (remaining == 0)
			{
				return true;
			}
			else
			{
				item.setStack(remaining);
				return false;
			}
		}

		return false;
	}

	/// <summary>
	/// Retire une quantité d'item d'un slot spécifique.
	/// Si le slot devient vide, il est réinitialisé à null.
	/// </summary>
	/// <param name="slotIndex">Index du slot cible.</param>
	/// <param name="amount">Quantité à retirer.</param>
	public void removeItem(int slotIndex, int amount)
	{
		if (slots[slotIndex] != null)
		{
			slots[slotIndex].subtract(amount);
			if (slots[slotIndex].isEmpty())
			{
				deleteItem(slotIndex);
			}
		}
	}
	/// <summary>
	// Supprime l'item dans le slot
	/// </summary>
	/// <param name="slotIndex">Index du slot cible.</param>
	public void deleteItem(int slotIndex)
	{
		slots[slotIndex] = null;
	}

	public StackItem getItem(int slotIndex)
	{
		return slots[slotIndex];
	}
}
