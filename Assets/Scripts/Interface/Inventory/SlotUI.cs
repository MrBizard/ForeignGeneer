using Godot;

public partial class SlotUI : Control
{
    private TextureRect _icon;
    private Label _countLabel;
    private StackItem _stackItem;
    private Inventory _inventory;
    private PlayerInventoryManager _playerInventoryManager;
    private TextureRect _backIcon;
    private Panel _background;
    private bool _isOutputSlot = false;

    /// <summary>
    /// Initialise le slot avec un item, un inventaire et un gestionnaire d'inventaire.
    /// </summary>
    public void initialize(StackItem stackItem, Inventory inventory, PlayerInventoryManager playerInventoryManager, bool isOutputSlot = false)
    {
        _stackItem = stackItem;
        _inventory = inventory;
        _playerInventoryManager = playerInventoryManager;
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
        _playerInventoryManager.startDraggingItem();
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
        if (_playerInventoryManager.currentItemInMouse == null)
        {
            if (_stackItem != null)
            {
                _playerInventoryManager.currentItemInMouse = _stackItem;
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
                    _stackItem = _playerInventoryManager.currentItemInMouse;
                    _inventory.addItem(_stackItem, GetIndex());
                    _playerInventoryManager.currentItemInMouse = null;
                }
                else if (_stackItem.getResource() == _playerInventoryManager.currentItemInMouse.getResource())
                {
                    _playerInventoryManager.currentItemInMouse.setStack(_stackItem.add(_playerInventoryManager.currentItemInMouse.getStack()));
                    if (_playerInventoryManager.currentItemInMouse.getStack() <= 0)
                    {
                        _playerInventoryManager.currentItemInMouse = null;
                    }
                }
                else
                {
                    var temp = _stackItem;
                    _stackItem = _playerInventoryManager.currentItemInMouse;
                    _playerInventoryManager.currentItemInMouse = temp;
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
            _playerInventoryManager.currentItemInMouse = _stackItem.split();
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