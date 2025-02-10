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
    public StaticBody3D instantiateFactory(Vector3 pos)
    {
        PackedScene scene = GD.Load<PackedScene>(_scenePath);
        if (scene == null || pos == Vector3.Zero)
        {
            return null;
        }

        var itemInstantiate = scene.Instantiate<StaticBody3D>();
        itemInstantiate.GlobalPosition = pos;
        return itemInstantiate;
    }

    /// <summary>
    /// Handles the right-click action. Instantiates a new Fonderie at the player's position.
    /// </summary>
    /// <param name="player">The player who triggered the right-click action.</param>
    public override void RightClick()
    {
        StaticBody3D instance = instantiateFactory(Player.Instance.raycast.getWorldCursorPosition());
        if (instance != null)
        {
            InventoryManager manager = InventoryManager.Instance;
            manager.hotbar.removeItem(manager.currentSlotHotbar,1);
            Player.Instance.GetParent().AddChild(instance);
        }
        
    }
    
}