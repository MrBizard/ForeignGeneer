using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	//[Export]
	//public Array<ItemStatic> items = new Array<ItemStatic>();
	[Export] public ItemStatic item;
	[Export] public Node ingot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (item != null && Input.IsActionJustPressed("T"))
		{
			GD.Print("ca marche");
			StaticBody3D a = null;

			if (item.prefabs.CanInstantiate())
			{
				a = item.prefabs.Instantiate<StaticBody3D>();
				a.Name = "ingotIron";
				Node3D last = null;
				if (ingot.GetChildCount() > 0)
					last = (Node3D)ingot.GetChild(-1);
				if (last != null)
				{
					a.Position = last.Position + new Vector3(1, 0, 0);
				}
				else
				{
					a.Position = new Vector3(0, 2, 0);

				}
				
				StackItem script = a as StackItem;
				script.instantiate(item);
				ingot.AddChild(a);
			}
			GD.Print(a.Position);
		}
	}
}
