using Godot;

public partial class InventoryUi : Control
{
    [Export] public PackedScene slotUiPackedScene;
    public PlayerInventoryManager playerInventoryManager;
    private HBoxContainer hotbarContainer;
    private GridContainer mainInventoryContainer;
    private bool _isInventoryVisible = false; // Nouvelle variable pour gérer la visibilité

    public override void _Ready()
    {
        Visible = _isInventoryVisible; // Synchroniser avec la variable
        hotbarContainer = GetNode<HBoxContainer>("Hotbar");
        mainInventoryContainer = GetNode<GridContainer>("MainInventory");
        playerInventoryManager = GetParent<PlayerInventoryManager>();
        initializeInventoryUi();
        updateUi();
    }

    /// <summary>
    /// Initialise l'UI de l'inventaire.
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
    /// Met à jour l'UI de l'inventaire.
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
    /// Bascule la visibilité de l'inventaire.
    /// </summary>
    public void toggleInventory()
    {
        _isInventoryVisible = !_isInventoryVisible; // Mettre à jour la variable
        Visible = _isInventoryVisible; // Synchroniser avec la propriété Visible
        GD.Print("Inventory visibility toggled to: " + _isInventoryVisible);

        if (_isInventoryVisible)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            playerInventoryManager.MouseFilter = MouseFilterEnum.Stop;
            updateUi();
        }
        else
        {
            if (playerInventoryManager.currentItemInMouse != null)
            {
                playerInventoryManager.dropItemOutsideInventory();
            }
            playerInventoryManager.MouseFilter = MouseFilterEnum.Ignore;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    /// <summary>
    /// Définit la visibilité de l'inventaire.
    /// </summary>
    /// <param name="visible">True pour afficher, False pour masquer.</param>
    public void setInventoryVisible(bool visible)
    {
        _isInventoryVisible = visible;
        Visible = _isInventoryVisible;

        if (_isInventoryVisible)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            playerInventoryManager.MouseFilter = MouseFilterEnum.Stop;
            updateUi();
        }
        else
        {
            if (playerInventoryManager.currentItemInMouse != null)
            {
                playerInventoryManager.dropItemOutsideInventory();
            }
            playerInventoryManager.MouseFilter = MouseFilterEnum.Ignore;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}