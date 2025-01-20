using Godot;
using System;

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
		if(IsColliding()){
			// si il detecte un objet avec le script ItemAuSol
			if (GetCollider() is ItemAuSol item){
				if (Input.IsActionPressed("interragir")){

					player.mainInventory.addItem(item.stackItem,1);
					item.QueueFree();
				}
			}
		}
	}
}
