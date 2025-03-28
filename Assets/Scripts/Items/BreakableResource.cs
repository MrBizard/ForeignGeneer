using Godot;

public partial class BreakableResource : HarvestableResource
{
	[Export] public StackItem Item { get; set; }
	[Export] public ResourceStatic ResourceStatic { get; set; }

	private CollisionShape3D _collisionShape;

	public override void _Ready()
	{
		base._Ready();
		_collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		ItemResource = ResourceStatic; 
	}

	public override void Harvest()
	{
		if (IsActive) return;

		IsActive = true;
		Visible = false;
		_collisionShape.Disabled = true;

		if (Item != null)
		{
			InventoryManager.Instance.addItemToInventory(Item);
		}
	}

	public override void ResetResource()
	{
		IsActive = false;
		Visible = true;
		_collisionShape.Disabled = false;
		RespawnTimer = 10.0;
	}
}
