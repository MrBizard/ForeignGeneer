using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure;

public partial class raycast : RayCast3D
{
	[Export] public RayCast3D ray;
	public PlayerInventoryManager player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player=GetTree().Root.GetChild(0).GetNode<PlayerInventoryManager>("PlayerInventoryManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("interragir")){
			getObjectInGround();
		}
	}

		private void getObjectInGround(){

			if (IsColliding())
			{
				switch (GetCollider())
				{
					case ItemAuSol item:
						player.mainInventory.addItem(item.stackItem);
						item.QueueFree();
						break;
					case BreakableResource resource:
						resource.IsActive = true;
						player.mainInventory.addOneItem(resource.item);
						resource.item.getResource().LeftClick();
						break;
					case PlayerStructure structure:
						structure.openUi();
						break;
				}
			}
		}
		
}

