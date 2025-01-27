using Godot;

public partial class InventoryUi : Control
{
    [Export] public PackedScene slotUiPackedScene;
    private HBoxContainer hotbarContainer;
    private GridContainer mainInventoryContainer;

    public override void _Ready()
    {
        hotbarContainer = GetNode<HBoxContainer>("Hotbar");
        mainInventoryContainer = GetNode<GridContainer>("MainInventory");

        InitializeInventoryUi();
        UpdateUi();
    }

    /// <summary>
    /// Initialise l'UI de l'inventaire.
    /// </summary>
    private void InitializeInventoryUi()
    {
        for (int i = 0; i < InventoryManager.Instance.HotbarSize; i++)  
        {
            var slot = slotUiPackedScene.Instantiate<SlotUI>();
            hotbarContainer.AddChild(slot);
            slot.initialize(InventoryManager.Instance.hotbar.getItem(i), InventoryManager.Instance.hotbar, InventoryManager.Instance);
        }

        for (int i = 0; i < InventoryManager.Instance.mainInventorySize; i++)
        {
            var slot = slotUiPackedScene.Instantiate<SlotUI>();
            mainInventoryContainer.AddChild(slot);
            slot.initialize(InventoryManager.Instance.mainInventory.getItem(i), InventoryManager.Instance.mainInventory, InventoryManager.Instance);
        }
    }

    /// <summary>
    /// Met à jour l'UI de l'inventaire.
    /// </summary>
    public void UpdateUi()
    {
        for (int i = 0; i < InventoryManager.Instance.HotbarSize; i++)
        {
            var item = InventoryManager.Instance.hotbar.getItem(i);
            if (item != null)
            {
                var slot = hotbarContainer.GetChild(i) as SlotUI;
                slot?.initialize(item, InventoryManager.Instance.hotbar, InventoryManager.Instance);
            }
        }

        for (int i = 0; i < InventoryManager.Instance.mainInventorySize; i++)
        {
            var item = InventoryManager.Instance.mainInventory.getItem(i);
            if (item != null)
            {
                var slot = mainInventoryContainer.GetChild(i) as SlotUI;
                slot?.initialize(item, InventoryManager.Instance.mainInventory, InventoryManager.Instance);
            }
        }
    }
}