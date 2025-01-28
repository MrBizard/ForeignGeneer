using Godot;

public partial class InventoryManager : Node
{
    public static InventoryManager Instance { get; private set; }

    [Export] public int HotbarSize { get; private set; } = 5;
    [Export] private ItemStatic testItem;
    [Export] private ItemStatic testItem2;
    [Export] private ItemStatic testItem3;
    [Export] public int mainInventorySize { get; private set; } = 20;
    [Export] public int hotbarSize { get; private set; } = 5;

    public Inventory mainInventory { get; private set; }
    public Inventory hotbar { get; private set; }
    public StackItem currentItemInMouse { get; private set; } // Item actuellement porté par la souris

    [Export] private PackedScene slotBasePackedScene; // Scène PackedScene pour SlotBase
    private Control slotBaseInstance; // Instance de SlotBase qui suit la souris

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree(); // Assure qu'il n'y a qu'une seule instance
        }

        mainInventory = new Inventory(mainInventorySize);
        hotbar = new Inventory(hotbarSize);

        // Exemple d'initialisation (à supprimer en production)
        mainInventory.addItemToSlot(new StackItem(testItem, 50), 10);
        mainInventory.addItemToSlot(new StackItem(testItem, 67), 7);
        mainInventory.addItemToSlot(new StackItem(testItem2, 1), 1);
        mainInventory.addItemToSlot(new StackItem(testItem3, 1), 2);
        hotbar.addItemToSlot(new StackItem(testItem, 67), 1);
    }

    public override void _Process(double delta)
    {
        // Si un item est porté par la souris, mettez à jour la position de SlotBase
        if (currentItemInMouse != null && slotBaseInstance != null)
        {
            slotBaseInstance.Position = GetViewport().GetMousePosition();
        }
    }

    /// <summary>
    /// Définit l'item actuellement porté par la souris.
    /// </summary>
    public void setCurrentItemInMouse(StackItem item)
    {
        currentItemInMouse = item;
        
        if (slotBaseInstance != null)
        {
            slotBaseInstance.QueueFree();
            slotBaseInstance = null;
        }

        if (item != null)
        {
            if (slotBasePackedScene == null)
            {
                GD.PrintErr("slotBasePackedScene n'est pas assigné !");
                return;
            }

            // Instancier SlotBase
            slotBaseInstance = slotBasePackedScene.Instantiate<Control>();
            AddChild(slotBaseInstance);

            // Configurer l'icône et la quantité
            Sprite2D icon = slotBaseInstance.GetNode<Sprite2D>("Icon");
            Label countLabel = slotBaseInstance.GetNode<Label>("CountLabel");

            if (icon == null || countLabel == null)
            {
                GD.PrintErr("Les nœuds Icon ou CountLabel sont manquants dans SlotBase !");
                return;
            }

            icon.Texture = item.getResource().getInventoryIcon; // Assurez-vous que `getInventoryIcon` existe
            countLabel.Text = item.getStack().ToString();
        }
    }

    /// <summary>
    /// Ajoute un item à un slot spécifique de l'inventaire principal.
    /// </summary>
    public void addItemToMainInventory(StackItem item, int slotIndex)
    {
        mainInventory.addItemToSlot(item, slotIndex);
    }

    /// <summary>
    /// Ajoute un item à un slot spécifique de la hotbar.
    /// </summary>
    public void addItemToHotbar(StackItem item, int slotIndex)
    {
        hotbar.addItemToSlot(item, slotIndex);
    }
}