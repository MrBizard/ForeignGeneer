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

        updateSlot();
    }

    /// <summary>
    /// Met à jour l'affichage du slot en fonction de son contenu.
    /// </summary>
    public void updateSlot()
    {
        var stackItem = _inventory.getItem(_slotIndex);

        if (stackItem != null && stackItem.getStack() > 0)
        {
            GD.Print("Mise à jour du slot avec l'item : " + stackItem.getResource().GetName());
            _icon.Texture = stackItem.getResource().getInventoryIcon;
            _countLabel.Text = stackItem.getStack() > 1 ? stackItem.getStack().ToString() : "";
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
                _inventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse,_slotIndex);
                InventoryManager.Instance.setCurrentItemInMouse(temp); // Mettre à jour l'item dans la souris
                _inventory.notifyInventoryUpdated();
            }
        }
        else
        {
            GD.Print("Impossible de poser des objets dans le slot de sortie.");
        }
    }
    updateSlot();
    // Met à jour l'affichage de l'item dans la souris après chaque interaction
    InventoryManager.Instance.setCurrentItemInMouse(InventoryManager.Instance.currentItemInMouse);
}


    private void handleRightClick()
    {
        var stackItem = _inventory.getItem(_slotIndex);
        if (stackItem != null && stackItem.getStack() > 1) // Vérifie qu'il y a un item et qu'il peut être divisé
        {
            // Diviser le stack en deux
            int halfStack = stackItem.getStack() / 2;
            int remainingStack = stackItem.getStack() - halfStack;

            // Créer un nouvel item avec la moitié divisée
            StackItem splitItem = new StackItem(stackItem.getResource(), halfStack);

            // Mettre à jour la quantité dans le slot actuel
            stackItem.setStack(remainingStack);

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
                    stackItem.setStack(stackItem.getStack() + excess);
                }

                // Mettre à jour l'item porté par la souris
                InventoryManager.Instance.setCurrentItemInMouse(currentMouseItem);
            }
            else
            {
                // Si la souris porte un autre type d'item, ne rien faire (ou afficher un message)
                GD.Print("Impossible de diviser : la souris porte un autre type d'item.");
                // Annuler la division et remettre la quantité originale dans le slot
                stackItem.setStack(stackItem.getStack() + splitItem.getStack());
                return;
            }

            // Mettre à jour l'affichage et notifier le changement
            _inventory.notifyInventoryUpdated();
            updateSlot();
        }
        else if (stackItem != null && stackItem.getStack() == 1)
        {
            // Si le stack est de 1, simplement déplacer l'item vers la souris
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            _inventory.deleteItem(_slotIndex);
            _inventory.notifyInventoryUpdated();
            updateSlot();
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

    /// <summary>
    /// Vide le slot.
    /// </summary>
    public void clearSlot()
    {
        _icon.Texture = null;
        _countLabel.Text = "";
    }
}
