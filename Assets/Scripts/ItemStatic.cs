using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(ItemStatic), "", nameof(Resource))]
public partial class ItemStatic : Resource
{
	[Export] public int p_nbMaxStack { get; set; }
	[Export] public PackedScene prefabs { get; set; }
	[Export] public Material material { get; set; }
	[Export] public Texture2D inventoryIcon;

	public ItemStatic(Texture2D inventoryIcon, int p_nbMaxStack)
	{
		this.p_nbMaxStack = p_nbMaxStack;
		this.inventoryIcon = inventoryIcon;
	}

	public virtual void LeftClick()
	{
		GD.Print("Left Clicked on ItemStatic");
	}

	public virtual void RightClick()
	{
		GD.Print("Right Clicked on ItemStatic");
	}
}
