using System;
using System.Collections.Generic;
using Godot;

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
		// recherche si l'item existe n'existe pas encore dans l'inventaire
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i] != null && slots[i].getResource() == item.getResource())
			{
				int remaining = slots[i].add(item.getStack());
				if (remaining == 0)
				{
					return 0; // All items were added successfully
				}
				else
				{
					item.setStack(remaining); // Update the remaining items to add
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
    /// Ajoute un seul item à l'inventaire.
    /// </summary>
    public int addOneItem(StackItem item)
	{
	    // Recherche si l'item existe déjà dans l'inventaire
	    for (int i = 0; i < slots.Count; i++)
	    {
	        if (slots[i] != null && slots[i].getResource() == item.getResource())
	        {
	            // Si le slot actuel a de la place, ajoute l'item
	            if (slots[i].getStack() < slots[i].getResource().getMaxStack)
	            {
	                int remaining = slots[i].add(1);
	                return 0;
	            }
	        }
	    }

	    // Si l'item n'existe pas encore dans l'inventaire
	    for (int i = 0; i < slots.Count; i++)
	    {
	        if (slots[i] == null)
	        {
	            // Crée une nouvelle instance de StackItem pour éviter de modifier la référence originale
	            slots[i] = new StackItem(item.getResource());
	            slots[i].setStack(1); // Initialise le slot avec 1 item
	            return 0; // L'item a été ajouté avec succès
	        }
	    }

	    // Si aucun espace n'est disponible, retourne le nombre d'items restants
	    return item.getStack();
	}

	public override string ToString()
	{
		return String.Join(", ", slots.ToString());
	}
}