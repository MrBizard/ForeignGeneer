using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.Interface;
using ForeignGeneer.Assets.Scripts.Interface.Inventory;
using Godot;
using Godot.Collections;

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
    public PreviewObject currentPreview;
    public List<Inventory> inventory = new List<Inventory>();

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
        
        inventory.Add(mainInventory);
        inventory.Add(hotbar);
        
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
    private Vector3 FindNearestFreeSpot(Vector3 origin)
    {
        Node3D player = Player.Instance;
        if (player == null)
        {
            GD.PrintErr("Player not found!", player);
            return origin;
        }
        return origin; 
    }

    private Vector3 AdjustHeightToGround(Vector3 position)
    {
        Node3D player = Player.Instance; 
        if (player == null)
        {
            GD.PrintErr("Player not found!");
            return position;
        }

        PhysicsDirectSpaceState3D spaceState = player.GetWorld3D().DirectSpaceState;
    
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(position + new Vector3(0, 2, 0), position - new Vector3(0, 5, 0));
    
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0)
        {
            return (Vector3)result["position"];
        }
    
        return position;
    }

    public int addItemToInventory(StackItem stack)
    {
        int looseItem = 0;
        foreach (Inventory inv in inventory)
        { 
            looseItem = inv.addItem(stack);
            if (looseItem <= 0)
                return 0;
        }
        return looseItem;
    }

    public StackItem getCurrentItem()
    {
        return hotbar.getItem(currentSlotHotbar);
    }
    public StackItem FindItem(ItemStatic item)
    {
        foreach (Inventory inv in inventory)
        {
            StackItem foundItem = inv.FindItem(item);
            if (foundItem != null)
            {
                return foundItem;
            }
        }
        return null;
    }
    public void addCurrentItemToHotbar()
    {
        if (currentSlotHotbar + 1 < hotbarSize)
        {
            currentSlotHotbar++;
        }
        else
        {
            currentSlotHotbar = 0;
        }
        
        StopPreview();
    }
    public void removeCurrentItemToHotbar()
    {
        if (currentSlotHotbar - 1 >= 0)
        {
            currentSlotHotbar--;
        }
        else
        {
            currentSlotHotbar = hotbarSize-1;
        }

        StopPreview();
    }
    
    private bool isItemAuSol(string path)
    {
        PackedScene scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            return false;
        }

        var instance = scene.Instantiate();
        bool isItemAuSol = instance is ItemAuSol;
        instance.QueueFree();
        return isItemAuSol;
    }

    /// <summary>
    /// Désactive la prévisualisation.
    /// </summary>
    public void StopPreview()
    {
        if (currentPreview != null)
        {
            currentPreview.Destroy();
            currentPreview = null;
        }
    }
}