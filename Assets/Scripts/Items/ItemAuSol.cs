using Godot;
using System;

public partial class ItemAuSol : StaticBody3D
{
	public StackItem stackItem;
	
	public void instantiate(StackItem item)
	{
		stackItem = item;
	}
}
