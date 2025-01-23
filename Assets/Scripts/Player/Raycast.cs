using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure;

public partial class Raycast : RayCast3D
{
    [Export] public RayCast3D ray;
    private PlayerInventoryManager _player;

    public override void _Ready()
    {
        _player = GetTree().Root.GetChild(0).GetNode<PlayerInventoryManager>("PlayerInventoryManager");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("interragir"))
        {
            getObjectInGround();
        }
    }

    private void getObjectInGround()
    {
        if (IsColliding())
        {
            switch (GetCollider())
            {
                case ItemAuSol item:
                    
                    _player.mainInventory.addItem(item.stackItem);
                    item.QueueFree();
                    break;
                case BreakableResource resource:
                    resource.IsActive = true;
                    _player.mainInventory.addOneItem(resource.item);
                    resource.item.getResource().LeftClick();
                    break;
                case IPlayerStructure structure:
                    structure.openUi();
                    break;
            }
        }
    }
}