using ForeignGeneer.Assets.Scripts.block.playerStructure;
using Godot;

public abstract partial class PlayerBaseStructure : StaticBody3D, IPlayerStructure<ItemStatic>
{
    public ItemStatic itemStatic { get; set; }

    public virtual void dismantle()
    {
        QueueFree();
    }

    public virtual void openUi()
    {
    }

    public virtual void closeUi()
    {
    }
}