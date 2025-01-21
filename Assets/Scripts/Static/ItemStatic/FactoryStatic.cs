using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class FactoryStatic : ItemStatic
{
	[Export] public int ElectricalCost { get; set; }
	[Export] public int PollutionIndex { get; set; }
	

	// Override des comportements spécifiques
	public override void LeftClick()
	{
		GD.Print("Factory Left Clicked");
	}

	public override void RightClick()
	{
		GD.Print("Factory Right Clicked");
	}
}
