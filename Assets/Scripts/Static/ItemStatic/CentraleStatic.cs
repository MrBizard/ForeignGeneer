using Godot;
using MonoCustomResourceRegistry;
public partial class CentraleStatic : ItemStatic
{
	[Export] public int electricalCost { get; set; }
	[Export] public int pollutionIndex { get; set; }
	[Export]public int durability { get; set; }

	public CentraleStatic(){}
	public override void LeftClick()
	{
		GD.Print("Central leftclick");
	}

	public override void RightClick(Player player)
	{
		GD.Print("Central rightclick");
	}
}
