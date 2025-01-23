using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class FactoryStatic : ItemStatic
{
	[Export] public int electricalCost { get; set; }
	[Export] public int pollutionIndex { get; set; }

	public override void LeftClick()
	{
		GD.Print("Factory Left Clicked");
	}

	public Fonderie instantiateFactory(Vector3 pos)
	{
		Fonderie itemInstantiate = _prefab.Instantiate<Fonderie>();

		itemInstantiate.GlobalPosition = pos;

		itemInstantiate.factoryStatic = this;

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
