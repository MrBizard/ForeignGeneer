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

    /// <summary>
    /// Instantiates a new Fonderie at a given position and assigns the electrical cost.
    /// </summary>
    /// <param name="pos">The position where the Fonderie will be instantiated.</param>
    /// <returns>The instantiated Fonderie object.</returns>
    public Fonderie instantiateFactory(Vector3 pos)
    {
        var itemInstantiate = _prefab.Instantiate<Fonderie>();

        itemInstantiate.GlobalPosition = pos;
        itemInstantiate.electricalCost = electricalCost;

        return itemInstantiate;
    }

    /// <summary>
    /// Handles the right-click action. Instantiates a new Fonderie at the player's position.
    /// </summary>
    /// <param name="player">The player who triggered the right-click action.</param>
    public override void RightClick(Player player)
    {
        player.GetParent().AddChild(instantiateFactory(player.Position + new Vector3(1, 0.1f, 1)));
    }
}