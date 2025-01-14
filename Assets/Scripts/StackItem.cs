using Godot;
using System;
public partial class StackItem 
{
	public ItemStatic _resource;
	public int stack;

	public StackItem(ItemStatic resource, int nb = 1)
	{
		_resource = resource;
		stack = nb;
	}


	
	public int Add(int nb)
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
	public int substract(int nb){
		stack = Math.Max(0,stack - nb);
		return stack;
	}

	public bool isEmpty(){
		return stack == 0;
	}
	
	
}
