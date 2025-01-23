using Godot;
using MonoCustomResourceRegistry;
[RegisteredType(nameof(CentraleStatic), "", nameof(ItemStatic))]

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

	public CoalCentral instantiateFactory(Vector3 pos)
	{
		CoalCentral itemInstantiate = _prefab.Instantiate<CoalCentral>();


		itemInstantiate.GlobalPosition = pos;

		itemInstantiate.electricalCost = electricalCost;

		GD.Print($"Données du prefab : ElectricalCost = {electricalCost}, PollutionIndex = {pollutionIndex}");

		return itemInstantiate;
	}

	public override void RightClick(Player player)
	{
		player.GetParent().AddChild(instantiateFactory(player.Position + new Vector3(1, 0.1f, 1)));

		player._invManager.hotbar.removeItem(player.currentItemInHand, 1);
		player._invManager.hotbar.notifyInventoryUpdated();
	}
}
