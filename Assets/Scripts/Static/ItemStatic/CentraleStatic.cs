using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(CentraleStatic), "", nameof(ItemStatic))]
public partial class CentraleStatic : ItemStatic
{
    [Export] public int electricalCost { get; set; }
    [Export] public int pollutionIndex { get; set; }
    [Export] public int durability { get; set; }
    [Export] public int tier { get; set; }
    [Export] public RecipeList recipeList { get; set; }

    public CentraleStatic() {}

    /// <summary>
    /// Action effectuée lorsque l'utilisateur clique sur l'objet avec le bouton gauche.
    /// </summary>
    public override void LeftClick()
    {
        // Fonction de gestion du clic gauche (à définir selon le besoin)
    }

    /// <summary>
    /// Instancie un objet de type Central à une position donnée.
    /// </summary>
    /// <param name="pos">La position où l'objet doit être instancié.</param>
    /// <returns>L'objet instancié.</returns>
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
    /// Action effectuée lorsque l'utilisateur clique sur l'objet avec le bouton droit.
    /// Place un objet Central près du joueur et consomme un item du hotbar.
    /// </summary>
    /// <param name="player">Le joueur qui a effectué le clic.</param>
    public override void RightClick(Player player)
    {
        player.GetParent().AddChild(instantiateFactory(player.Position + new Vector3(1, 0.1f, 1)));

        InventoryManager.Instance.hotbar.removeItem(0, 1);
        InventoryManager.Instance.hotbar.notifyInventoryUpdated();
    }
}