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
	private int _nbSlotCount {get; set;}
	/// <summary>
	/// Initialise un slot d'inventaire avec un item et une instance d'inventaire.
	/// </summary>
	/// <param name="stackItem">L'item contenu dans le slot.</param>
	/// <param name="inventory">Instance de l'inventaire associée.</param>
	public void initialize(StackItem stackItem, Inventory inventory)
	{
		_stackItem = stackItem;
		_inventory = inventory;
		_countLabel = GetNode<Label>("CountLabel");
		_icon = GetNode<TextureRect>("Icon");
		updateSlot();
	}

	/// <summary>
	/// Met à jour l'affichage du slot en fonction de son contenu.
	/// </summary>
	private void updateSlot()
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

	/// <summary>
	/// Gère les interactions utilisateur avec le slot via des clics de souris.
	/// </summary>
	/// <param name="event">L'événement d'entrée utilisateur.</param>
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
				handleLeftClick();
			else if (mouseEvent.ButtonIndex == MouseButton.Right)
				handleRightClick();
		}
	}

	/// <summary>
	/// Gère un clic droit sur le slot (division de stack).
	/// </summary>
	private void handleRightClick()
	{
		if (_stackItem != null)
		{
			_inventory.currentItemInMouse = _stackItem.split();
			updateSlot();
		}
	}

	/// <summary>
	/// Gère un clic gauche sur le slot (ramassage, placement ou échange d'items).
	/// </summary>
	private void handleLeftClick()
	{
		// Aucun Item
		if (_inventory.currentItemInMouse == null)
		{
			GD.Print(_stackItem.getStack().ToString());
			_inventory.currentItemInMouse = _stackItem;

			_inventory.deleteItem(GetIndex());
			_stackItem = null;
		}
		else
		{
			if (_stackItem == null)
			{
				_stackItem = _inventory.currentItemInMouse;
				GD.Print(_inventory.currentItemInMouse.getStack());
				_inventory.addItem(_stackItem, GetIndex());
				_inventory.currentItemInMouse = null;
			}
			else if (_stackItem.getResource() == _inventory.currentItemInMouse.getResource())
			{
				_inventory.currentItemInMouse.setStack(_stackItem.add(_inventory.currentItemInMouse.getStack()));
				if (_inventory.currentItemInMouse.getStack() <= 0)
				{
					_inventory.currentItemInMouse = null;
				}

			}
			else
			{
				// Différents items : échange
				var temp = _stackItem;
				_stackItem = _inventory.currentItemInMouse;
				_inventory.currentItemInMouse = temp;
				GD.Print("Items swapped between hand and slot.");
			}
		}

		updateSlot();
	}
}
