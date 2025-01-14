using Godot;
using System;

public partial class InventoryLayout : Node
{
	[Export] public Player player;
	private Inventory inv;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		inv = player.inv;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
