using Godot;
using System;

public partial class Main : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var mapGenerator = GD.Load<PackedScene>("res://map_generator.tscn").Instantiate();
		AddChild(mapGenerator);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
