using Godot;
using Godot.Collections;

public partial class PickableResource : HarvestableResource
{
	[Export] private Array<CollisionShape3D> _collisionShapes;
	[Export] private Array<MeshInstance3D> _meshes;

	public override void _Ready()
	{
		base._Ready();
	
		_collisionShapes ??= new Array<CollisionShape3D>();
		_meshes ??= new Array<MeshInstance3D>();

		if (_meshes.Count == 0)
		{
			var mesh = GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
			if (mesh != null) _meshes.Add(mesh);
		}
	}

	public override void Harvest()
	{
		GD.Print(" apasse harvbfesqt");
		if (_meshes.Count <= 1 || !_meshes[1].Visible) return;


		if (ItemResource is ItemStatic itemStatic)
		{
			InventoryManager.Instance.addItemToInventory(new StackItem(itemStatic, 1));
		}

		_meshes[1].Visible = false;
		RespawnTimer = 20.0;
	}

	public override void ResetResource()
	{
		if (_meshes.Count > 1 && RespawnTimer <= 0 && !_meshes[1].Visible)
		{
			_meshes[1].Visible = true;
		}
	}

	public override void ProcessRespawn(double delta)
	{
		if (_meshes.Count > 1 && !_meshes[1].Visible)
		{
			base.ProcessRespawn(delta);
		}
	}
}
