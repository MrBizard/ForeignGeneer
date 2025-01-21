using Godot;

/// <summary>
/// Manages the player's inventory, including the main inventory, hotbar, 
/// and item interactions such as dragging and dropping with the mouse.
/// </summary>
public partial class PlayerInventoryManager : Control
{
	[Export] public int mainInventorySize { get; private set; } = 20;
	[Export] public int hotbarSize { get; private set; } = 5;

	public Inventory mainInventory { get; private set; }
	public Inventory hotbar { get; private set; }
	public StackItem currentItemInMouse { get; set; }

	private InventoryUi inventoryUi;
	private Control cursorItemInstance;

	[Export] public PackedScene inventoryUiPackedScene;
	[Export] public PackedScene cursorItemPackedScene;
	[Export] public ItemStatic testItem;

	public override void _Ready()
	{
		mainInventory = new Inventory(mainInventorySize);
		hotbar = new Inventory(hotbarSize);
		currentItemInMouse = null;

		mainInventory.addItemToSlot(new StackItem(testItem, 50), 10);
		mainInventory.addItemToSlot(new StackItem(testItem, 67), 7);
		hotbar.addItemToSlot(new StackItem(testItem, 67), 1);

		if (inventoryUiPackedScene != null)
		{
			inventoryUi = inventoryUiPackedScene.Instantiate<InventoryUi>();
			AddChild(inventoryUi);
		}

		cursorItemInstance = cursorItemPackedScene.Instantiate<Control>();
		cursorItemInstance.Visible = false;
		AddChild(cursorItemInstance);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventory"))
		{
			inventoryUi.toggleInventory();
		}

		if (cursorItemInstance != null)
		{
			cursorItemInstance.GlobalPosition = GetGlobalMousePosition();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
		{
			dropItemOutsideInventory();
		}
	}

	/// <summary>
	/// Starts dragging an item with the mouse and updates the cursor's visual.
	/// </summary>
	public void startDraggingItem()
	{
		if (cursorItemInstance == null)
			return;
		
		if (currentItemInMouse == null)
		{
			cursorItemInstance.Visible = false;
			return;
		}

		cursorItemInstance.Visible = true;
		cursorItemInstance.GlobalPosition = GetGlobalMousePosition();

		var sprite = cursorItemInstance.GetNode<Sprite2D>("Icon");
		var label = cursorItemInstance.GetNode<Label>("CountLabel");

		if (sprite != null)
		{
			sprite.Texture = currentItemInMouse.getResource().getInventoryIcon;
		}

		if (label != null)
		{
			label.Text = currentItemInMouse.getStack().ToString();
		}
	}

	/// <summary>
	/// Drops the item currently being carried by the mouse outside the inventory.
	/// </summary>
	public void dropItemOutsideInventory()
	{
		if (currentItemInMouse != null)
		{
			var itemDrop = currentItemInMouse.getResource().getPrefab.Instantiate<ItemAuSol>();
			itemDrop.instantiate(currentItemInMouse);
			GetParent().AddChild(itemDrop);
			itemDrop.GlobalPosition = getDropPosition();

			currentItemInMouse = null;
			
			startDraggingItem();
		}
	}

	/// <summary>
	/// Calculates the drop position for the item in the world relative to the player.
	/// </summary>
	/// <returns>World position where the item will be dropped.</returns>
	private Vector3 getDropPosition()
	{
		var player = GetParent().GetNode<Player>("Player_Character");
		return player.GlobalPosition + new Vector3(0, 0, 2);
	}
}
