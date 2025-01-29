using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(CentraleStatic), "", nameof(ItemStatic))]
public partial class CentraleStatic : ItemStatic
{
    [Export] public int electricalCost { get; set; }
    [Export] public int pollutionIndex { get; set; }
    [Export] public int durability { get; set; }

    public CentraleStatic() {}

    /// <summary>
    /// Action effectuée lorsque l'utilisateur clique sur l'objet avec le bouton gauche.
    /// </summary>
    public override void LeftClick()
    {
        // Fonction de gestion du clic gauche (à définir selon le besoin)
    }

    /// <summary>
    /// Instancie un objet de type CoalCentral à une position donnée.
    /// </summary>
    /// <param name="pos">La position où l'objet doit être instancié.</param>
    /// <returns>L'objet instancié.</returns>
    public CoalCentral instantiateFactory(Vector3 pos)
    {
        CoalCentral itemInstantiate = _prefab.Instantiate<CoalCentral>();

        itemInstantiate.GlobalPosition = pos;
        itemInstantiate.electricalCost = electricalCost;

        return itemInstantiate;
    }

    /// <summary>
    /// Action effectuée lorsque l'utilisateur clique sur l'objet avec le bouton droit.
    /// Place un objet CoalCentral près du joueur et consomme un item du hotbar.
    /// </summary>
    /// <param name="player">Le joueur qui a effectué le clic.</param>
    public override void RightClick(Player player)
    {
        player.GetParent().AddChild(instantiateFactory(player.Position + new Vector3(1, 0.1f, 1)));

        InventoryManager.Instance.hotbar.removeItem(0, 1);
        InventoryManager.Instance.hotbar.notifyInventoryUpdated();
    }
}