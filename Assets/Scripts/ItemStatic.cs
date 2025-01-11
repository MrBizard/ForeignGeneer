using Godot;
using System;

using MonoCustomResourceRegistry;

[RegisteredType(nameof(ItemStatic), "", nameof(Resource))]
public abstract partial class ItemStatic : Resource
{
	[Export] public int p_nbMaxStack  { get; set; }
	[Export] public PackedScene prefabs  { get; set; }
	[Export] public Material material  { get; set; }
	[Export] public Texture2D inventoryIcon;
	public ItemStatic(Texture2D inventoryIcon)
	{
		p_nbMaxStack = 64;
		this.inventoryIcon = inventoryIcon;
	}

	
	public abstract void LeftClick();
	public abstract void RightClick();


}
