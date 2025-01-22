using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class ResourceStatic : ItemStatic
{
	 public ResourceStatic(Texture2D inventoryIcon, int p_nbMaxStack) : base(inventoryIcon, p_nbMaxStack){}
	public ResourceType ResourceType { get; set; }

	public override void LeftClick()
	{
		GD.Print("Resource leftclick");
	}

	public override void RightClick()
	{
		GD.Print("Resource rightclick");
	}
}
