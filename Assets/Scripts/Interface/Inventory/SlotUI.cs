using Godot;

public partial class SlotUI : Control
{
	private TextureRect icon;
	private Label countLabel;

	private StackItem stackItem;
	private Inventory inventory;
	private PlayerInventoryManager playerInventoryManager;

	private bool isOutputSlot = false; // Indique si ce slot est un slot de sortie

	/// <summary>
	/// Initialise un slot d'inventaire avec un item, une instance d'inventaire et une référence à PlayerInventoryManager.
	/// </summary>
	/// <param name="stackItem">L'item contenu dans le slot.</param>
	/// <param name="inventory">Instance de l'inventaire associée.</param>
	/// <param name="playerInventoryManager">Référence au gestionnaire d'inventaire du joueur.</param>
	public void initialize(StackItem stackItem, Inventory inventory, PlayerInventoryManager playerInventoryManager, bool isOutputSlot = false)
	{
		this.stackItem = stackItem;
		this.inventory = inventory;
		this.playerInventoryManager = playerInventoryManager;
		this.isOutputSlot = isOutputSlot; // Définit si ce slot est un slot de sortie

		icon = GetNode<TextureRect>("Icon");
		countLabel = GetNode<Label>("CountLabel");

		updateSlot();
	}

	/// <summary>
	/// Met à jour l'affichage du slot en fonction de son contenu.
	/// </summary>
	public void updateSlot()
	{
		if (stackItem != null && stackItem.getStack() > 0)
		{
			icon.Texture = stackItem.getResource().getInventoryIcon;
			countLabel.Text = stackItem.getStack() > 1 ? stackItem.getStack().ToString() : "";
		}
		else
		{
			icon.Texture = null;
			countLabel.Text = "";
		}
		playerInventoryManager.startDraggingItem();
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
	/// Gère un clic gauche sur le slot (déplacement ou échange d'items).
	/// </summary>
	private void handleLeftClick()
	{
		if (playerInventoryManager.currentItemInMouse == null)
		{
			if (stackItem != null)
			{
				// Prendre l'item du slot
				playerInventoryManager.currentItemInMouse = stackItem;
				inventory.deleteItem(GetIndex());
				stackItem = null;
			}
		}
		else
		{
			if (!isOutputSlot) // Empêche l'ajout d'items dans le slot de sortie
			{
				if (stackItem == null)
				{
					// Poser l'item dans le slot
					stackItem = playerInventoryManager.currentItemInMouse;
					inventory.addItem(stackItem, GetIndex());
					playerInventoryManager.currentItemInMouse = null;
				}
				else if (stackItem.getResource() == playerInventoryManager.currentItemInMouse.getResource())
				{
					// Fusionner les stacks si les items sont identiques
					playerInventoryManager.currentItemInMouse.setStack(stackItem.add(playerInventoryManager.currentItemInMouse.getStack()));
					if (playerInventoryManager.currentItemInMouse.getStack() <= 0)
					{
						playerInventoryManager.currentItemInMouse = null;
					}
				}
				else
				{
					// Échanger les items
					var temp = stackItem;
					stackItem = playerInventoryManager.currentItemInMouse;
					playerInventoryManager.currentItemInMouse = temp;
				}
			}
			else
			{
				GD.Print("Impossible de poser des objets dans le slot de sortie.");
			}
		}
		updateSlot();
	}

	/// <summary>
	/// Gère un clic droit sur le slot (division de stack).
	/// </summary>
	private void handleRightClick()
	{
		if (stackItem != null)
		{
			playerInventoryManager.currentItemInMouse = stackItem.split();
			inventory.notifyInventoryUpdated();
			updateSlot();
		}
	}

	/// <summary>
	/// Retourne l'item actuellement dans le slot.
	/// </summary>
	/// <returns>L'item du slot.</returns>
	public StackItem getStackItem()
	{
		return stackItem;
	}
}
