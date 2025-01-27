using Godot;

public partial class SlotUI : Control
{
    private TextureRect _icon;
    private Label _countLabel;
    private StackItem _stackItem;
    private Inventory _inventory;
    private TextureRect _backIcon;
    private Panel _background;
    private bool _isOutputSlot = false;

    /// <summary>
    /// Initialise le slot avec un item, un inventaire et un gestionnaire d'inventaire.
    /// </summary>
    public void initialize(StackItem stackItem, Inventory inventory, bool isOutputSlot = false)
    {
        _stackItem = stackItem;
        _inventory = inventory;
        _isOutputSlot = isOutputSlot;

        _background = GetNode<Panel>("Background");
        _backIcon = GetNode<TextureRect>("Background/BackIcon");
        _icon = GetNode<TextureRect>("Icon");
        _countLabel = GetNode<Label>("CountLabel");

        updateSlot();
    }

    /// <summary>
    /// Met à jour l'affichage du slot en fonction de son contenu.
    /// </summary>
    public void updateSlot()
    {
        if (_stackItem != null && _stackItem.getStack() > 0)
        {
            _icon.Texture = _stackItem.getResource().getInventoryIcon;
            _countLabel.Text = _stackItem.getStack() > 1 ? _stackItem.getStack().ToString() : "";
        }
        else
        {
            _icon.Texture = null;
            _countLabel.Text = "";
        }
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                handleLeftClick();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                handleRightClick();
            }
        }
    }

    private void handleLeftClick()
    {
        if (InventoryManager.Instance.currentItemInMouse == null)
        {
            if (_stackItem != null)
            {
                InventoryManager.Instance.currentItemInMouse = _stackItem;
                _inventory.deleteItem(GetIndex());
                _stackItem = null;
            }
        }
        else
        {
            if (!_isOutputSlot)
            {
                if (_stackItem == null)
                {
                    _stackItem = InventoryManager.Instance.currentItemInMouse;
                    _inventory.addItemToSlot(_stackItem, GetIndex());
                    InventoryManager.Instance.currentItemInMouse = null;
                }
                else if (_stackItem.getResource() == InventoryManager.Instance.currentItemInMouse.getResource())
                {
                    InventoryManager.Instance.currentItemInMouse.setStack(_stackItem.add(InventoryManager.Instance.currentItemInMouse.getStack()));
                    if (InventoryManager.Instance.currentItemInMouse.getStack() <= 0)
                    {
                        InventoryManager.Instance.currentItemInMouse = null;
                    }
                }
                else
                {
                    var temp = _stackItem;
                    _stackItem = InventoryManager.Instance.currentItemInMouse;
                    InventoryManager.Instance.currentItemInMouse = temp;
                }
            }
            else
            {
                GD.Print("Impossible de poser des objets dans le slot de sortie.");
            }
        }
        updateSlot();
    }

    private void handleRightClick()
    {
        if (_stackItem != null)
        {
            InventoryManager.Instance.currentItemInMouse = _stackItem.split();
            _inventory.notifyInventoryUpdated();
            updateSlot();
        }
    }

    /// <summary>
    /// Retourne l'item actuellement dans le slot.
    /// </summary>
    public StackItem getStackItem()
    {
        return _stackItem;
    }

    /// <summary>
    /// Définit la texture de fond du slot.
    /// </summary>
    public void setBackgroundTexture(Texture2D texture)
    {
        if (_backIcon != null)
        {
            _backIcon.Texture = texture;
        }
        else
        {
            GD.PrintErr("Background TextureRect is not assigned!");
        }
    }
}