using System;
using System.Collections.Generic;

/// <summary>
/// Classe représentant l'inventaire d'un joueur.
/// Gère les slots, les items et les interactions avec les items.
/// </summary>
public class Inventory
{
	public List<StackItem> slots { get; private set; }
	public event Action onInventoryUpdated;

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
	public bool addItemToSlot(StackItem item, int slotIndex)
	{
		if (slotIndex >= slots.Count)
			return false;

		bool success = false;

		if (slots[slotIndex] == null)
		{
			slots[slotIndex] = item;
			success = true;
		}
		else if (slots[slotIndex].getResource() == item.getResource())
		{
			int remaining = slots[slotIndex].add(item.getStack());
			if (remaining == 0)
			{
				success = true;
			}
			else
			{
				item.setStack(remaining);
			}
		}

		if (success)
		{
			onInventoryUpdated?.Invoke(); // Déclenche l'événement
		}

		return success;
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
			onInventoryUpdated?.Invoke();
		}
	}

	/// <summary>
	// Supprime l'item dans le slot
	/// </summary>
	/// <param name="slotIndex">Index du slot cible.</param>
	public void deleteItem(int slotIndex)
	{
		if (slots[slotIndex] != null)
		{
			slots[slotIndex] = null;
			onInventoryUpdated?.Invoke(); 
		}
	}

	/// <summary>
	/// Récupère l'item dans un slot spécifique.
	/// </summary>
	/// <param name="slotIndex">Index du slot cible.</param>
	/// <returns>L'item dans le slot, ou null si le slot est vide.</returns>
	public StackItem getItem(int slotIndex)
	{
		return slots[slotIndex];
	}

	public int addItem(StackItem item)
	{
		if (item == null)
			return 0;
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i] != null && slots[i].getResource() == item.getResource())
			{
				int remaining = slots[i].add(item.getStack());
				if (remaining == 0)
				{
					return 0; 
				}
				else
				{
					item.setStack(remaining); 
				}
			}
		}

		// si il n'y a pas encore de slot occupé par cette item
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i] == null)
			{
				slots[i] = item;
				return 0; // All items were added successfully
			}
		}

		// If no space is available, return the remaining items
		return item.getStack();
	}
	
	public void notifyInventoryUpdated()
	{
		onInventoryUpdated?.Invoke();
	}
	
	/// <summary>
	/// Recherche un item spécifique dans l'inventaire.
	/// </summary>
	/// <param name="item">L'item à rechercher.</param>
	/// <returns>L'item trouvé, ou null si l'item n'existe pas dans l'inventaire.</returns>
	public StackItem FindItem(ItemStatic item)
	{
		if (item == null)
		{
			return null;
		}

		// Parcours de tous les slots de l'inventaire
		foreach (var slot in slots)
		{
			if (slot != null && slot.getResource() == item)
			{
				return slot; // Retourne l'item trouvé
			}
		}

		return null; // Aucun item correspondant trouvé
	}
	public override string ToString()
	{
		return String.Join(", ", slots.ToString());
	}
}
