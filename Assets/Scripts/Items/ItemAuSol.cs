using Godot;
using System;

public partial class ItemAuSol : StaticBody3D
{
	public StackItem stackItem {get;set;}

	[Export] public string name="Assignez un nom";


    public void instantiate(StackItem item)
	{
		stackItem = item;
	}
}
