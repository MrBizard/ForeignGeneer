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
            ItemStatic itemToDrop = currentItemInMouse.getResource();
            Vector3 dropPosition = FindDropPosition();
        
            if (dropPosition != Vector3.Zero)
            {
                ItemAuSol droppedItem = itemToDrop.instantiate(dropPosition);
                GetTree().CurrentScene.AddChild(droppedItem);
            }
        }
    }
    private Vector3 FindDropPosition()
    {
        Node3D player = GetTree().CurrentScene.GetNode<Node3D>("Player");
        if (player == null)
        {
            GD.PrintErr("Player not found!");
            return Vector3.Zero;
        }

        Vector3 start = player.GlobalPosition + new Vector3(0, 1, 0); // Position de départ du raycast
        Vector3 direction = -player.GlobalTransform.Basis.Z.Normalized() * 2; // Devant le joueur

        PhysicsDirectSpaceState3D spaceState = player.GetWorld3D().DirectSpaceState;
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(start, start + direction);
        query.CollideWithAreas = false;

        var result = spaceState.IntersectRay(query);

        if (result.Count == 0) // Si pas d'obstacle, on place l'item ici
        {
            return start + direction;
        }
        else // Sinon, essayer autour
        {
            return FindNearestFreeSpot(player.GlobalPosition);   
        }
    }
    private Vector3 FindNearestFreeSpot(Vector3 origin)
    {
        Node3D player = GetTree().CurrentScene.GetNode<Node3D>("Player"); // Assurez-vous que le chemin est correct
        if (player == null)
        {
            GD.PrintErr("Player not found!");
            return origin;
        }

        PhysicsDirectSpaceState3D spaceState = player.GetWorld3D().DirectSpaceState; // Récupération correcte

        float radius = 2.0f;
        int angleStep = 30;  

        for (int angle = 0; angle < 360; angle += angleStep)
        {
            float rad = Mathf.DegToRad(angle);
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
            Vector3 checkPos = origin + offset + new Vector3(0, 1, 0);

            PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(checkPos, checkPos - new Vector3(0, 2, 0));
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                return (Vector3)result["position"];
            }
        }
    
        return origin; 
    }

    private Vector3 AdjustHeightToGround(Vector3 position)
    {
        Node3D player = GetTree().CurrentScene.GetNode<Node3D>("Player"); 
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
    
        return position; // Si aucun sol trouvé, garde la position initiale
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