using Godot;
using System;
public partial class StackItem : StaticBody3D
{
	public ItemStatic _resource;
	public int stack;

	public StackItem(ItemStatic resource, int nb = 1)
	{
		_resource = resource;
		stack = nb;
	}
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
		_resource = res;
		var mat = res.material;
		GetChild<MeshInstance3D>(0).MaterialOverride = mat;
	}
	
	public void add(int nb)
	{
		if (stack + nb <= _resource.p_nbMaxStack)
		{
			stack += nb;
		}
		else
		{
			var newStack = new StackItem(_resource, _resource.p_nbMaxStack - stack);
			AddChild(newStack);
			stack = nb - (_resource.p_nbMaxStack - stack);
		}
	}
	public void substract(int nb){
		stack = Math.Max(0,stack - nb);
	}
	
	
}
