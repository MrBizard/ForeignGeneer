using Godot;
using System;

public partial class StackItem : StaticBody3D
{
	public ItemStatic resource;
	public int stack;
	public override void _EnterTree()
	{
		base._EnterTree();

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void instantiate(ItemStatic res)
	{
		resource = res;
		var mat = res.material;
		GetChild<MeshInstance3D>(0).MaterialOverride = mat;
	}
}
