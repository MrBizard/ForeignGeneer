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
    public StackItem currentItemInMouse { get; set; } // Item actuellement porté par la souris
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