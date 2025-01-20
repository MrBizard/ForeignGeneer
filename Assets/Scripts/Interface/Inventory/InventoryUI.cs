using Godot;

/// <summary>
/// Manages the inventory user interface, including the hotbar and main inventory display.
/// </summary>
public partial class InventoryUi : Panel
{
	[Export] public PackedScene slotUiPackedScene;
	public PlayerInventoryManager playerInventoryManager;
	private HBoxContainer hotbarContainer;
	private GridContainer mainInventoryContainer;
	private bool isInventoryOpen = false;

	public override void _Ready()
	{
		Visible = false;
		hotbarContainer = GetNode<HBoxContainer>("Hotbar");
		mainInventoryContainer = GetNode<GridContainer>("MainInventory");
		playerInventoryManager = GetParent<PlayerInventoryManager>();
		initializeInventoryUi();
		updateUi();
	}

	/// <summary>
	/// Initializes the inventory UI slots for both the hotbar and the main inventory.
	/// </summary>
	private void initializeInventoryUi()
	{
		for (int i = 0; i < playerInventoryManager.hotbarSize; i++)
		{
			var slot = slotUiPackedScene.Instantiate<SlotUI>();
			hotbarContainer.AddChild(slot);
			slot.initialize(playerInventoryManager.hotbar.getItem(i), playerInventoryManager.hotbar, playerInventoryManager);
		}

		for (int i = 0; i < playerInventoryManager.mainInventorySize; i++)
		{
			var slot = slotUiPackedScene.Instantiate<SlotUI>();
			mainInventoryContainer.AddChild(slot);
			slot.initialize(playerInventoryManager.mainInventory.getItem(i), playerInventoryManager.mainInventory, playerInventoryManager);
		}
	}

	/// <summary>
	/// Updates the inventory UI to reflect the current state of the player's inventories.
	/// </summary>
	public void updateUi()
	{
		for (int i = 0; i < playerInventoryManager.hotbarSize; i++)
		{
			var item = playerInventoryManager.hotbar.getItem(i);
			if (item != null)
			{
				var slot = hotbarContainer.GetChild(i) as SlotUI;
				slot?.initialize(item, playerInventoryManager.hotbar, playerInventoryManager);
			}
		}

		for (int i = 0; i < playerInventoryManager.mainInventorySize; i++)
		{
			var item = playerInventoryManager.mainInventory.getItem(i);
			if (item != null)
			{
				var slot = mainInventoryContainer.GetChild(i) as SlotUI;
				slot?.initialize(item, playerInventoryManager.mainInventory, playerInventoryManager);
			}
		}
	}

	/// <summary>
	/// Toggles the visibility of the inventory UI and updates its state.
	/// </summary>
	public void toggleInventory()
	{
		isInventoryOpen = !isInventoryOpen;
		Visible = isInventoryOpen;

		if (isInventoryOpen)
		{
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			updateUi();
		}
		else
		{
			if (playerInventoryManager.currentItemInMouse != null)
			{
				playerInventoryManager.dropItemOutsideInventory();
			}
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
	}
}
