using System;
using ForeignGeneer.Assets.Scripts.Interface;
using Godot;

public partial class SlotUI : Control
{
    private TextureRect _icon;
    private Label _countLabel;
    private Inventory _inventory;
    private TextureRect _backIcon;
    private Panel _background;
    private bool _isOutputSlot = false;
    private int _slotIndex;
    private Timer _hoverTimer;
    private bool _isHovered = false;

    public override void _Ready()
    {
        _hoverTimer = new Timer();
        _hoverTimer.WaitTime = 0.5f; // Temps avant affichage
        _hoverTimer.OneShot = true;
        _hoverTimer.Connect("timeout", new Callable(this, nameof(showItemDescription)));
        AddChild(_hoverTimer);
    }
    
    /// <summary>
    /// Initialise le slot avec un inventaire et un index.
    /// </summary>
    public void initialize(Inventory inventory, int slotIndex, bool isOutputSlot = false)
    {
        _inventory = inventory;
        _slotIndex = slotIndex;
        _isOutputSlot = isOutputSlot;

        _background = GetNode<Panel>("Background");
        _backIcon = GetNode<TextureRect>("Background/BackIcon");
        _icon = GetNode<TextureRect>("Icon");
        _countLabel = GetNode<Label>("CountLabel");
        
        updateUi();
    }

    /// <summary>
    /// Met à jour l'affichage du slot en fonction de son contenu.
    /// </summary>
    public void updateUi(int updateType = 0)
    {
        var stackItem = _inventory.getItem(_slotIndex);

        if (stackItem != null && stackItem.getStack() > 0)
        {
            _icon.Texture = stackItem.getResource().getInventoryIcon;
            _countLabel.Text = stackItem.getStack() > 1 ? stackItem.getStack().ToString() : "";
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
        var stackItem = _inventory.getItem(_slotIndex);
        if (stackItem != null)
        {
            // Prendre l'item du slot
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            _inventory.deleteItem(_slotIndex);
            _inventory.notifyInventoryUpdated();
        }
    }
    else
    {
        if (!_isOutputSlot)
        {
            var stackItem = _inventory.getItem(_slotIndex);
            if (stackItem == null)
            {
                // Poser l'item dans le slot vide
                _inventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, _slotIndex);
                InventoryManager.Instance.setCurrentItemInMouse(null); // Mettre à jour l'item dans la souris
                _inventory.notifyInventoryUpdated();
            }
            else if (stackItem.getResource() == InventoryManager.Instance.currentItemInMouse.getResource())
            {
                // Fusionner les stacks si c'est le même item
                InventoryManager.Instance.currentItemInMouse.setStack(stackItem.add(InventoryManager.Instance.currentItemInMouse.getStack()));
                if (InventoryManager.Instance.currentItemInMouse.getStack() <= 0)
                {
                    InventoryManager.Instance.setCurrentItemInMouse(null);
                }
                _inventory.notifyInventoryUpdated();
            }
            else
            {
                // Échanger les items
                var temp = stackItem;
                _inventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, _slotIndex);
                InventoryManager.Instance.setCurrentItemInMouse(temp); // Mettre à jour l'item dans la souris
                _inventory.notifyInventoryUpdated();
            }
        }
    }
    updateUi();
    // Met à jour l'affichage de l'item dans la souris après chaque interaction
    InventoryManager.Instance.setCurrentItemInMouse(InventoryManager.Instance.currentItemInMouse);
}


    private void handleRightClick()
    {
        var stackItem = _inventory.getItem(_slotIndex);
        if (stackItem != null && stackItem.getStack() > 0)
        {
            StackItem splitItem = stackItem.split();
            
            StackItem currentMouseItem = InventoryManager.Instance.currentItemInMouse;
            if (currentMouseItem == null)
            {
                InventoryManager.Instance.setCurrentItemInMouse(splitItem);
            }
            else if (currentMouseItem.getResource() == splitItem.getResource())
            {
                int excess = currentMouseItem.add(splitItem.getStack()); 

                if (excess > 0)
                {
                    stackItem.setStack(stackItem.getStack() + excess);
                }

                InventoryManager.Instance.setCurrentItemInMouse(currentMouseItem);
            } 

            _inventory.notifyInventoryUpdated();
            updateUi();
        }
        else if (stackItem != null && stackItem.getStack() == 1)
        {
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            _inventory.deleteItem(_slotIndex);
            _inventory.notifyInventoryUpdated();
            updateUi();
        }
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
    private void _on_mouse_entered()
    {
        _isHovered = true;
        _hoverTimer.Start();
    }

    private void _on_mouse_exited()
    {
        _isHovered = false;
        _hoverTimer.Stop();
        hideItemDescription();
    }

    private void showItemDescription()
    {
        if (_isHovered && _inventory.getItem(_slotIndex) != null)
        {
            UiManager.instance.openUi("overlayItemInformation", _inventory.getItem(_slotIndex));
        }
    }

    private void hideItemDescription()
    {
        UiManager.instance.closeUi("overlayItemInformation");
    }
    
    /// <summary>
    /// Vide le slot.
    /// </summary>
    public void clearSlot()
    {
        _icon.Texture = null;
        _countLabel.Text = "";
    }
    
}
