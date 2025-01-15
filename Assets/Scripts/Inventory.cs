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

	public void addItem(StackItem item, int CurrentCase){
		StackItem currItem = this.items[CurrentCase];
		if (currItem == null) return;
		if (currItem._resource != item._resource)return;
        currItem.substract(currItem.add(item.stack));
	}
	
	public StackItem split(int CurrentCase)
	{
		StackItem currItem = this.items[CurrentCase];
		if (currItem == null) return null;
		int valueSub = currItem.stack / 2;
		currItem.substract(valueSub);
        return new StackItem(currItem._resource, valueSub);
	}
	

}
