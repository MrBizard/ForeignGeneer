using Godot;
using System;

/// <summary>
/// Classe responsable de l'interface utilisateur de l'inventaire.
/// Permet d'afficher, mettre à jour et gérer l'état de l'inventaire à l'écran.
/// </summary>
public partial class InventoryUI : Panel
{
	private Inventory _inventory;
	private Control _mouseItemContainer;
	private Sprite2D _mouseItemSprite;
	private Label _mouseItemLabel;

	[Export] private PackedScene _slotUiPackedScene;
	[Export] private PackedScene _itemInHandPackedScene;
	private GridContainer _gridContainer;
	
	/// <summary>
	/// Initialise l'interface de l'inventaire avec une instance de l'inventaire.
	/// Crée et configure les éléments nécessaires pour l'affichage de l'item actuellement dans la main.
	/// </summary>
	/// <param name="inventory">L'inventaire à lier à l'interface.</param>
	public void initialize(Inventory inventory)
	{
		_inventory = inventory;
		Visible = false;
		_gridContainer = GetNode<GridContainer>("GridContainer");

		_mouseItemContainer = _itemInHandPackedScene.Instantiate<Control>();
		_mouseItemSprite = _mouseItemContainer.GetNode<Sprite2D>("Icon");
		_mouseItemLabel = _mouseItemContainer.GetNode<Label>("CountLabel");

		_mouseItemContainer.GlobalPosition = GetGlobalMousePosition();
		_mouseItemContainer.Visible = false;
		
		// Ajoute un CanvasLayer pour gérer l'affichage de l'item dans la main de façon indépendante
		CanvasLayer canvasLayer = new CanvasLayer();
		canvasLayer.AddChild(_mouseItemContainer);
		AddChild(canvasLayer);
	}

	/// <summary>
	/// Mise à jour de l'affichage de l'item actuellement dans la main du joueur.
	/// </summary>
	/// <param name="delta">Le deltaTime du frame, non utilisé dans cette méthode.</param>
	public override void _Process(double delta)
	{
		updateItemInHand();
	}

	/// <summary>
	/// Met à jour la position et les informations de l'item dans la main (icône, quantité).
	/// </summary>
	private void updateItemInHand()
	{
		if (_inventory.currentItemInMouse != null)
		{
			_mouseItemContainer.SetGlobalPosition(GetGlobalMousePosition());

			var texture = _inventory.currentItemInMouse.getResource().getInventoryIcon;
			_mouseItemSprite.Texture = texture;

			var stackCount = _inventory.currentItemInMouse.getStack();
			_mouseItemLabel.Text = stackCount > 1 ? stackCount.ToString() : "";

			_mouseItemContainer.Visible = true;
		}
		else
		{
			_mouseItemContainer.Visible = false;
		}
	}

	/// <summary>
	/// Gère l'entrée utilisateur, en particulier les clics de souris.
	/// Si un clic droit est détecté, appelle la méthode DropItem.
	/// </summary>
	/// <param name="inputEvent">L'événement d'entrée.</param>
	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
			{
				GD.Print("Clic droit détecté !");
				dropItem();
			}
		}
	}

	/// <summary>
	/// Permet de déposer l'item actuellement dans la main du joueur dans le monde à une position spécifiée.
	/// L'item est instancié et ajouté à la scène.
	/// </summary>
	private void dropItem()
	{
		if (_inventory.currentItemInMouse != null)
		{
			PackedScene prefab = _inventory.currentItemInMouse.getResource().getPrefab;  
			if (prefab != null)
			{
				Vector3 dropPosition = getDropPosition();

				StaticBody3D instantiatedItem = prefab.Instantiate<StaticBody3D>();
				instantiatedItem.Position = dropPosition;

				GetParent().AddChild(instantiatedItem);
			}

			_inventory.currentItemInMouse = null;
			_mouseItemContainer.Visible = false;
			updateUi();
		}
	}

	/// <summary>
	/// Calcule et renvoie la position où l'item sera déposé dans le monde.
	/// La position est relative au joueur.
	/// </summary>
	/// <returns>La position de dépôt dans le monde.</returns>
	private Vector3 getDropPosition()
	{
		var player = GetParent().GetParent().GetNode<Player>("Player_Character");
		return player.GlobalPosition + new Vector3(0, 0, 2);
	}

	/// <summary>
	/// Met à jour l'affichage de l'inventaire en réinitialisant et redessinant les éléments de l'inventaire.
	/// </summary>
	public void updateUi()
	{
		foreach (Node child in _gridContainer.GetChildren())
		{
			child.QueueFree();
		}
		
		foreach (var stackItem in _inventory.slots)
		{
			var slotUiInstance = _slotUiPackedScene.Instantiate<SlotUI>();
			slotUiInstance.initialize(stackItem, _inventory);
			_gridContainer.AddChild(slotUiInstance);
		}

		_gridContainer.QueueRedraw();
	}

	/// <summary>
	/// Bascule la visibilité de l'inventaire et ajuste le mode de la souris.
	/// </summary>
	public void toggleInventory()
	{
		if (!Visible)
		{
			updateUi();
		}

		Visible = !Visible;
		Input.MouseMode = Visible ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
	}
}
