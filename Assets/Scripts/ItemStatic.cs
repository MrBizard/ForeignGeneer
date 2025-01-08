using Godot;
using System;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(ItemStatic), "", nameof(Resource))]
public partial class ItemStatic : Resource
{
	[Export] public int p_nbMaxStack;
	[Export] public PackedScene prefabs;
	[Export] public Material material;
	public ItemStatic()
	{
		p_nbMaxStack = 64;
	}

}
