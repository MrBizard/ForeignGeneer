using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	[Export]
	public Array<ItemStatic> items = new Array<ItemStatic>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
