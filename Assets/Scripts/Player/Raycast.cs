using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure;

public partial class Raycast : RayCast3D
{
    public override void _Ready()
    {
        base._Ready();
    }

    /// <summary>
    /// Détecte et interagit avec l'objet touché par le raycast.
    /// </summary>
    public void InteractWithObject()
    {
        TargetPosition = new Vector3(0, -2 ,0);
        if (IsColliding())
        {
            switch (GetCollider())
            {
                case ItemAuSol item:
                    InventoryManager.Instance.mainInventory.addItem(item.stackItem);
                    item.QueueFree();
                    break;
                case BreakableResource resource:
                    resource.IsActive = true;
                    InventoryManager.Instance.mainInventory.addOneItem(resource.item);
                    resource.item.getResource().LeftClick();
                    break;
                case PlayerBaseStructure structure:
                    structure.openUi();
                    break;
            }
        }
    }
    /// <summary>
    /// Récupère la position mondiale ciblée par le raycast.
    /// </summary>
    /// <returns>La position mondiale du point de collision, ou Vector3.Zero si aucun objet n'est touché.</returns>
    public Vector3 getWorldCursorPosition()
    {
        TargetPosition = new Vector3(0, -10 ,0);
        if (IsColliding())
        {
            return GetCollisionPoint();
        }

        return Vector3.Zero;
    }
}