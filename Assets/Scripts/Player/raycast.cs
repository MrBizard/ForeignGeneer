using Godot;
using System;

public partial class raycast : RayCast3D
{
	[Export] public RayCast3D ray;
	[Export] public Player player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(IsColliding()){
			Object collider = GetCollider();
			GD.Print(collider);
			if (collider is ItemAuSol item){
				player.inv.addItem(item.stackItem,1);
				GD.Print(player.inv.slots[1].getResource());
				GD.Print(item.stackItem.getStack());
				item.QueueFree();
			}
		}
	}
}
