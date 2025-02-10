using ForeignGeneer.Assets.Scripts.Interface;
using ForeignGeneer.Assets.Scripts.Interface.Inventory;
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
    public StackItem currentItemInMouse { get; private set; }

    public int currentSlotHotbar = 0;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }

        mainInventory = new Inventory(mainInventorySize);
        hotbar = new Inventory(hotbarSize);

        mainInventory.addItemToSlot(new StackItem(testItem, 50), 10);
        mainInventory.addItemToSlot(new StackItem(testItem, 67), 7);
        mainInventory.addItemToSlot(new StackItem(testItem2, 1), 1);
        mainInventory.addItemToSlot(new StackItem(testItem3, 1), 2);
        hotbar.addItemToSlot(new StackItem(testItem, 67), 1);
    }
    /// <summary>
    /// Sets the item currently held by the mouse.
    /// </summary>
    public void setCurrentItemInMouse(StackItem item)
    {
        currentItemInMouse = item;

        UiManager.instance.closeUi("itemCursorUi");

        if (item != null)
        {
            UiManager.instance.openUi("itemCursorUi", item);
        }
    }

    public void drop()
    {
        if (currentItemInMouse != null)
        {
            currentItemInMouse.getResource().Instantiate();
        }
    }
    
    /// <summary>
    /// Adds an item to a specific slot in the main inventory.
    /// </summary>
    public void addItemToMainInventory(StackItem item, int slotIndex)
    {
        mainInventory.addItemToSlot(item, slotIndex);
    }

    /// <summary>
    /// Adds an item to a specific slot in the hotbar.
    /// </summary>
    public void addItemToHotbar(StackItem item, int slotIndex)
    {
        hotbar.addItemToSlot(item, slotIndex);
    }
}