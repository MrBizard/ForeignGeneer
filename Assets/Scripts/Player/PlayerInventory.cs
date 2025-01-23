using Godot;

public partial class PlayerInventory : Inventory
{
	public float health { get; set; }
	public float hunger { get; set; }
	public int hotBarSlot { get; set; }
	public int currentHotBarSlot { get; set; }
	public int itemSelect { get; set; }
	private Node sceneRoot;
	//[IMPORTANT] a utiliser si notre tablette est un item stocké dans l'inventaire du joueur.
	//public object[] tablette { get; set; }

	//sinon on utilise un bouton directement 
	public Button tabletteButton { get; set; }

	public PlayerInventory(int nbCase) : base(nbCase)
	{
		health = 100f;
		hunger = 100f;
		hotBarSlot = 0;
		currentHotBarSlot = 0;
		itemSelect = 0;
	}


	public void openTablette(Button tabletteButton){

		this.tabletteButton = tabletteButton;
		tabletteButton.Connect("pressed",new Callable(sceneRoot, nameof(openMiniGameInterface)));
	}

	

	private void openMiniGameInterface()
	{
		PackedScene minigameScene = GD.Load<PackedScene>("res://MinigameInterface.tscn");
		Node minigameInstance = minigameScene.Instantiate();
		sceneRoot.AddChild(minigameInstance);
	}



}
