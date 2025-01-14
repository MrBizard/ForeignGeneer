using Godot;
using MonoCustomResourceRegistry;
public partial class CentraleStatic : ItemStatic
{
	public int EnergyProd { get; set; }
	public int PollutionIndex { get; set; }


	 public CentraleStatic(Texture2D inventoryIcon, int p_nbMaxStack) : base(inventoryIcon, p_nbMaxStack)
	{
		EnergyProd = 100; 
		PollutionIndex = 10;  
	}

	public override void LeftClick()
	{
		GD.Print("Central leftclick");
	}

	public override void RightClick()
	{
		GD.Print("Central rightclick");
	}
}
