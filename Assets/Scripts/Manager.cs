using Godot;

public partial class Manager : Node
{
	[Export] public ItemStatic item;
	[Export] public Node ingot;
	private InventoryUI _inventoryUi;
	[Export] public PackedScene inventoryUiPackedScene;

	private StaticBody3D _newIngot;
	public override void _Ready()
	{
		_newIngot = null;
		//Crée l'inventaire
		//Inventory inv= new Inventory(28); 

		//Instancie l'interface de l'inventaire
		_inventoryUi = inventoryUiPackedScene.Instantiate<InventoryUI>();
		AddChild(_inventoryUi);
		Player player = GetParent().GetNode<Player>("Player_Character");
		_inventoryUi.initialize(player.inv);
		player.inv.addItem(new StackItem(item, 77), 6);
		player.inv.addItem(new StackItem(item, 50), 9);
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("inventory"))
		{
			_inventoryUi.toggleInventory();
		}
		if (item != null && Input.IsActionJustPressed("T"))
		{
			GD.Print("ca marche");
			

			if (item.getPrefab.CanInstantiate())
			{
				_newIngot = item.getPrefab.Instantiate<StaticBody3D>();
				_newIngot.Name = "ingotIron";
				Node3D last = null;
				if (ingot.GetChildCount() > 0)
					last = (Node3D)ingot.GetChild(-1);
				if (last != null)
				{
					_newIngot.Position = last.Position + new Vector3(1, 0, 0);
				}
				else
				{
					_newIngot.Position = new Vector3(0, 2, 0);

				}
				ingot.AddChild(_newIngot);
			}
		}
	}
}
