using Godot;
using System;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(ItemStatic), "", nameof(Resource))]
public partial class ItemStatic : Resource
{
	[Export] public string p_name;
	[Export] public int p_nbMaxStack;

	public ItemStatic()
	{
		p_name = "";
		p_nbMaxStack = 64;
	}
	public ItemStatic(string name, int nbMaxStack) {p_name = name; p_nbMaxStack = nbMaxStack; }
}
