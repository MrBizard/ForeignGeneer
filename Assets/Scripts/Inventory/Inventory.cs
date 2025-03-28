using System;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts;
using Godot;

public class Inventory : ISlotObservable
{
    public List<StackItem> slots { get; private set; }
    protected Dictionary<int, List<IObserver>> _slotObservers = new Dictionary<int, List<IObserver>>(); // Dictionnaire pour observer par slot
    public event Action onInventoryUpdated;

    public Inventory(int slotCount)
    {
        slots = new List<StackItem>(slotCount);
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(null);
        }
    }

    public void addItemToSlot(StackItem item, int slotIndex)
    {
        if (slotIndex >= slots.Count)
            return;

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
            onInventoryUpdated?.Invoke();
            notify(slotIndex);  // Notify only the updated slot
        }
    }

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
            notify(slotIndex);  // Notify only the updated slot
        }
    }

    public void deleteItem(int slotIndex)
    {
        if (slots[slotIndex] != null)
        {
            slots[slotIndex] = null;
            onInventoryUpdated?.Invoke();
            notify(slotIndex);  // Notify only the updated slot
        }
    }

    public StackItem? getItem(int slotIndex)
    {
        return slots[slotIndex];
    }

    public StackItem FindItem(ItemStatic item)
    {
        if (item == null)
        {
            return null;
        }

        foreach (var slot in slots)
        {
            if (slot != null && slot.getResource() == item)
            {
                return slot;
            }
        }

        return null;
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
                    notify(i);  // Notify only the updated slot
                    return 0;
                }
                item.setStack(remaining);
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                notify(i);  // Notify only the updated slot
                slots[i] = item;
                return 0;
            }
        }

        return item.getStack();
    }

    public void notifyInventoryUpdated()
    {
        onInventoryUpdated?.Invoke();
    }

    // Attach un observateur à un slot spécifique
    public void attach(int slotIndex, IObserver observer)
    {
        if (!_slotObservers.ContainsKey(slotIndex))
        {
            _slotObservers[slotIndex] = new List<IObserver>();
        }

        if (!_slotObservers[slotIndex].Contains(observer))
        {
            _slotObservers[slotIndex].Add(observer);
        }
    }

    // Detach un observateur d'un slot spécifique
    public void detach(int slotIndex, IObserver observer)
    {
        if (_slotObservers.ContainsKey(slotIndex))
        {
            _slotObservers[slotIndex].Remove(observer);
        }
    }

    // Notifie tous les observateurs du slot spécifié
    public void notify(int slotIndex)
    {
        if (_slotObservers.ContainsKey(slotIndex))
        {
            foreach (var observer in _slotObservers[slotIndex])
            {
                GD.Print(observer);
                observer.update(null);
            }
        }
    }
}
