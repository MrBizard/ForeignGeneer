using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class FactoryStatic : ItemStatic
{
    [Export] public int electricalCost { get; set; }
    [Export] public int pollutionIndex { get; set; }
    [Export] public int tier { get; set; }
    [Export] public RecipeList recipeList { get; set; }
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
        PackedScene scene = GD.Load<PackedScene>(_scenePath);
        if (scene == null)
        {
            GD.PrintErr("Impossible de charger la scène : " + _scenePath);
            return null;
        }

        var itemInstantiate = scene.Instantiate<Fonderie>();
        itemInstantiate.GlobalPosition = pos;
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