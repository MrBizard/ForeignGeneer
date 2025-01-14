using Godot;
using System;
using System.Collections;

public partial class Inventory 
{
	public int _nbCase { get; set; }
	public StackItem[] items { get; set;}
	
	public Inventory(int nbCase)
	{
		_nbCase = nbCase;
		items = new StackItem[nbCase];
		
	}

	public StackItem Add_Item(StackItem item, int CurrentCase){
		StackItem currItem = this.items[CurrentCase];
		

	}
	
	
}
