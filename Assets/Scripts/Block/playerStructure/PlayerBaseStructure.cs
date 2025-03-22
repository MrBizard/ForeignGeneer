using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts;
using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.block.playerStructure;
using ForeignGeneer.Assets.Scripts.manager;
using Godot;

public partial class PlayerBaseStructure : StaticBody3D,IObservable, IPlayerStructure<ItemStatic>
{
    private ItemStatic _itemStatic;
    private List<IObserver> _observers = new List<IObserver>();
    public virtual ItemStatic itemStatic => _itemStatic;

    protected void SetItemStatic(ItemStatic value)
    {
        _itemStatic = value;
    }

    public virtual void dismantle()
    {
        if (_itemStatic != null)
        {
            InventoryManager.Instance.addItemToInventory(new StackItem(_itemStatic, 1));
            QueueFree();
        }
    }

    public virtual void openUi() { }

    public virtual void closeUi() { }
    public virtual void interact(InteractType interactType)
    {
        switch (interactType)
        {
            case InteractType.Dismantle:
                dismantle();
                break;
            case InteractType.Interact:
                openUi();
                break;
            case InteractType.Open:
                openUi();
                break;
            case InteractType.Close:
                closeUi();
                break;
        }
    }

    public void attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            EnergyManager.instance.attach(observer);
        }
    }

    public void detach(IObserver observer)
    {
        _observers.Remove(observer);
        EnergyManager.instance.detach(observer);
    }
    
    public void notify(InterfaceType? interfaceType = null)
    {
        foreach (var observer in _observers)
        {
            observer.update(interfaceType);
        }
    }
}