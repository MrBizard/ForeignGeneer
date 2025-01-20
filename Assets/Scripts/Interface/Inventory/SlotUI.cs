using Godot;

/// <summary>
/// Classe responsable de l'interface utilisateur d'un slot d'inventaire.
/// Gère l'affichage et les interactions avec un slot individuel.
/// </summary>
public partial class SlotUI : Control
{
	private TextureRect _icon;
	private Label _countLabel;

	private StackItem _stackItem;
	private Inventory _inventory;
	
	// Référence au PlayerInventoryManager pour accéder au currentItemInMouse
	private PlayerInventoryManager _playerInventoryManager;

	/// <summary>
	/// Initialise un slot d'inventaire avec un item, une instance d'inventaire et une référence à PlayerInventoryManager.
	/// </summary>
	/// <param name="stackItem">L'item contenu dans le slot.</param>
	/// <param name="inventory">Instance de l'inventaire associée.</param>
	/// <param name="playerInventoryManager">Référence au gestionnaire d'inventaire du joueur.</param>
	public void initialize(StackItem stackItem, Inventory inventory, PlayerInventoryManager playerInventoryManager)
	{
		_stackItem = stackItem;
		_inventory = inventory;
		_icon = GetNode<TextureRect>("Icon");
		_countLabel = GetNode<Label>("CountLabel");
		_playerInventoryManager = playerInventoryManager;
		updateSlot();
	}

	/// <summary>
	/// Met à jour l'affichage du slot en fonction de son contenu.
	/// </summary>
	private void updateSlot()
	{
		if (_stackItem != null && _stackItem.getStack() > 0)
		{
			GD.Print(_stackItem.getResource().ToString());
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

	/// <summary>
	/// Gère les interactions utilisateur avec le slot via des clics de souris.
	/// </summary>
	/// <param name="event">L'événement d'entrée utilisateur.</param>
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


	/// <summary>
	/// Gère un clic droit sur le slot (division de stack).
	/// </summary>
	private void handleRightClick()
	{
		if (_stackItem != null)
		{
			_playerInventoryManager.currentItemInMouse = _stackItem.split();
			updateSlot();
		}
	}

	public void handleLeftClick()
	{
		// Aucun Item
		if (_playerInventoryManager.currentItemInMouse == null)
		{
			if (_stackItem != null)
			{
				// Créer un item à la souris
				_playerInventoryManager.currentItemInMouse = _stackItem;
				_inventory.deleteItem(GetIndex());
				_stackItem = null;
			}
		}
		else
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
		updateSlot();
	}

}
