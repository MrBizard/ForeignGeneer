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

		if (currItem == null){
			this.items[CurrentCase] = item;
			return null;
		}
		var tempStackcount = currItem.add(item.stack);
		currItem.stack = tempStackcount;
		if(item.isEmpty()){
			return null;
		};
		return null; 
	}

	public StackItem Get_Item(int CurrentCase){
		return this.items[CurrentCase];
	}
	public void Remove_Item(int CurrentCase){
		this.items[CurrentCase] = null;	
	}
	public StackItem split_Item(int CurrentCase){	
		var tempStackcount = this.items[CurrentCase];
		StackItem tempStack = new StackItem(tempStackcount._resource, (int)Math.Ceiling(tempStackcount.stack/2.0));
		tempStack.substract(tempStack.stack/2);
		return tempStack;
	}

	
	
}
