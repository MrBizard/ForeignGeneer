using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.block.playerStructure;
using Godot;

public abstract partial class PlayerBaseStructure : StaticBody3D, IPlayerStructure<ItemStatic>
{
    private ItemStatic _itemStatic;

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
        else
        {
            GD.PrintErr("ItemStatic is null in dismantle.");
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
}