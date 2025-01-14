using Godot;
using System;
public partial class StackItem
{
	[Export] public ItemStatic _resource;
	[Export] public int stack;

	public StackItem(ItemStatic resource, int nb = 1)
	{
		_resource = resource;
		stack = nb;
	}
	public int add(int nb)
	{
        if (stack + nb <= _resource.p_nbMaxStack)
		{
			stack += nb;
			return 0;
		}
		else
		{
			var newStack = stack + nb -_resource.p_nbMaxStack;
			stack = _resource.p_nbMaxStack;
			return newStack;
		}
	}
	public void substract(int nb){
		stack = Math.Max(0,stack - nb);
	}

	public bool isEmpty(){
		return stack == 0;
	}

	
}
