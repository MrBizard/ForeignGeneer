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

    public void setStackItem(StackItem stackItem)
    {
        _stackItem = stackItem;
    }
    /// <summary>
    /// Met à jour l'affichage du slot en fonction de son contenu.
    /// </summary>
    private StackItem _lastStackItem;
    private int _lastStackCount;

    public void updateSlot()
    {
        if (_stackItem != null && _stackItem.getStack() > 0)
        {
            GD.Print("Mise à jour du slot avec l'item : " + _stackItem.getResource().GetName());
            _icon.Texture = _stackItem.getResource().getInventoryIcon;
            _countLabel.Text = _stackItem.getStack() > 1 ? _stackItem.getStack().ToString() : "";
        }
        else
        {
            GD.Print("Slot vide.");
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
                // Prendre l'item du slot
                InventoryManager.Instance.setCurrentItemInMouse(_stackItem);
                _inventory.deleteItem(GetIndex());
                _stackItem = null;
                _inventory.notifyInventoryUpdated();
            }
        }
        else
        {
            if (!_isOutputSlot)
            {
                if (_stackItem == null)
                {
                    // Poser l'item dans le slot vide
                    _stackItem = InventoryManager.Instance.currentItemInMouse;
                    _inventory.addItemToSlot(_stackItem, GetIndex());
                    InventoryManager.Instance.setCurrentItemInMouse(null);
                    _inventory.notifyInventoryUpdated();
                }
                else if (_stackItem.getResource() == InventoryManager.Instance.currentItemInMouse.getResource())
                {
                    // Fusionner les stacks si c'est le même item
                    InventoryManager.Instance.currentItemInMouse.setStack(_stackItem.add(InventoryManager.Instance.currentItemInMouse.getStack()));
                    if (InventoryManager.Instance.currentItemInMouse.getStack() <= 0)
                    {
                        InventoryManager.Instance.setCurrentItemInMouse(null);
                    }
                    _inventory.notifyInventoryUpdated();
                }
                else
                {
                    // Échanger les items
                    var temp = _stackItem;
                    _stackItem = InventoryManager.Instance.currentItemInMouse;
                    InventoryManager.Instance.setCurrentItemInMouse(temp);
                    _inventory.notifyInventoryUpdated();
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
    if (_stackItem != null && _stackItem.getStack() > 1) // Vérifie qu'il y a un item et qu'il peut être divisé
    {
        // Diviser le stack en deux
        int halfStack = _stackItem.getStack() / 2;
        int remainingStack = _stackItem.getStack() - halfStack;

        // Créer un nouvel item avec la moitié divisée
        StackItem splitItem = new StackItem(_stackItem.getResource(), halfStack);

        // Mettre à jour la quantité dans le slot actuel
        _stackItem.setStack(remainingStack);

        // Gérer l'item porté par la souris
        StackItem currentMouseItem = InventoryManager.Instance.currentItemInMouse;
        if (currentMouseItem == null)
        {
            // Si la souris ne porte rien, définir l'item divisé comme item actuel
            InventoryManager.Instance.setCurrentItemInMouse(splitItem);
        }
        else if (currentMouseItem.getResource() == splitItem.getResource())
        {
            // Si la souris porte le même type d'item, ajouter la moitié divisée
            int excess = currentMouseItem.add(splitItem.getStack()); // Ajoute et récupère l'excédent

            if (excess > 0)
            {
                // S'il y a un excédent, le remettre dans le slot d'origine
                _stackItem.setStack(_stackItem.getStack() + excess);
            }

            // Mettre à jour l'item porté par la souris
            InventoryManager.Instance.setCurrentItemInMouse(currentMouseItem);
        }
        else
        {
            // Si la souris porte un autre type d'item, ne rien faire (ou afficher un message)
            GD.Print("Impossible de diviser : la souris porte un autre type d'item.");
            // Annuler la division et remettre la quantité originale dans le slot
            _stackItem.setStack(_stackItem.getStack() + splitItem.getStack());
            return;
        }

        // Mettre à jour l'affichage et notifier le changement
        _inventory.notifyInventoryUpdated();
        updateSlot();
    }
    else if (_stackItem != null && _stackItem.getStack() == 1)
    {
        // Si le stack est de 1, simplement déplacer l'item vers la souris
        InventoryManager.Instance.setCurrentItemInMouse(_stackItem);
        _stackItem = null;
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

    /// <summary>
    /// Vide le slot.
    /// </summary>
    public void clearSlot()
    {
        _icon.Texture = null;
        _countLabel.Text = "";
        _stackItem = null;
    }
}