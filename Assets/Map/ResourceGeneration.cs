using Godot;
using System;

public partial class ResourceGeneration : Node3D
{
	[Export] public TerraBrush.TerraBrush Map;
	[Export] public Vector3 MapSize { get; set; }
	[Export] public int CopperResourcesCount { get; set; } = 1000;
	[Export] public int IronResourcesCount { get; set; } = 1000;
	[Export] public int GoldResourcesCount { get; set; } = 500;

	[Export] public PackedScene CopperResourceScene { get; set; }
	[Export] public PackedScene IronResourceScene { get; set; }
	[Export] public PackedScene GoldResourceScene { get; set; }

	public override void _Ready()
	{
		if (Map != null)
		{
			MapSize = CalculateMapSize(Map);
			GD.Print("Map Size: " + MapSize);
		}
		else
		{
			GD.PrintErr("TerraBrush map is not assigned!");
		}

		SpawnObjects();
	}

	private Vector3 CalculateMapSize(TerraBrush.TerraBrush map)
	{
		int heightmapResolution = map.ZonesSize - 500;
		Vector3 terrainScale = map.Scale;
		float mapWidth = heightmapResolution * terrainScale.X;
		float mapDepth = heightmapResolution * terrainScale.Z;
		GD.Print($"Calculated map size: Width={mapWidth}, Depth={mapDepth}");
		return new Vector3(mapWidth, 0, mapDepth);
	}

	private void SpawnObjects()
	{
		for (int i = 0; i < CopperResourcesCount; i++)
		{
			SpawnResource(CopperResourceScene);
		}

		for (int i = 0; i < IronResourcesCount; i++)
		{
			SpawnResource(IronResourceScene);
		}

		for (int i = 0; i < GoldResourcesCount; i++)
		{
			SpawnResource(GoldResourceScene);
		}
	}

	private void SpawnResource(PackedScene resourceScene)
{
	if (resourceScene == null)
	{
		GD.PrintErr("Resource scene is null!");
		return;
	}

	// Instantiate the resource as a Node3D
	var resourceInstance = resourceScene.Instantiate<Node3D>();

	// Try to get the BreakableResource script from the root node
	var breakableResource = resourceInstance as BreakableResource;
	if (breakableResource == null)
	{
		GD.PrintErr("BreakableResource script not found on the root node!");
		return;
	}

	// Set a random position for the resource
	resourceInstance.Position = GetRandomPosition();

	// Add the resource to the scene
	AddChild(resourceInstance);
	GD.Print("Resource added to the scene.");
}

	private Vector3 GetRandomPosition()
	{
		var random = new Random();
		float x = (float)random.NextDouble() * MapSize.X - MapSize.X / 2;
		float z = (float)random.NextDouble() * MapSize.Z - MapSize.Z / 2;
		float y = GetTerrainHeight(x, z);
		GD.Print($"Random position generated: X={x}, Y={y}, Z={z}");
		return new Vector3(x, y, z);
	}

	private float GetTerrainHeight(float x, float z)
	{
		var rayCast = new RayCast3D();
		rayCast.Position = new Vector3(x, MapSize.Y + 100, z);
		rayCast.TargetPosition = new Vector3(x, -100, z);
		rayCast.Enabled = true;
		rayCast.CollisionMask = 1;
		AddChild(rayCast);
		rayCast.ForceRaycastUpdate();

		float height = 0;
		if (rayCast.IsColliding())
		{
			height = rayCast.GetCollisionPoint().Y;
			GD.Print($"Terrain height at ({x}, {z}): {height}");
		}
		else
		{
			GD.PrintErr($"RayCast3D did not detect terrain at ({x}, {z})");
		}

		rayCast.QueueFree();
		return height;
	}
}
