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
		// Instancier le prefab
		Fonderie itemInstantiate = _prefab.Instantiate<Fonderie>();

		// Ajouter l'objet à la scène

		// Définir la position après l'ajout à la scène
		itemInstantiate.GlobalPosition = pos;

		// Transférer les propriétés exportées de FactoryStatic vers Fonderie
		itemInstantiate.factoryStatic = this;

		// Utiliser les valeurs récupérées (par exemple, les afficher dans les logs)
		GD.Print($"Données du prefab : ElectricalCost = {electricalCost}, PollutionIndex = {pollutionIndex}");

		return itemInstantiate;
	}

	public override void RightClick(Player player)
	{
		// Instancier et ajouter la fonderie à la scène
		player.GetParent().AddChild(instantiateFactory(player.Position + new Vector3(1, 0.1f, 1)));

		// Retirer l'item de l'inventaire
		player._invManager.hotbar.removeItem(player.currentItemInHand, 1);
	}
}
