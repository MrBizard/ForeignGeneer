using Godot;
using System;

public partial class Inventory
{
	public int _nbCase { get; set; }
	public StackItem[] items { get; set;}
	
	public Inventory(int nbCase)
	{
		_nbCase = nbCase;
		items = new StackItem[nbCase];
		
	}
	
	
}
