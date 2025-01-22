using Godot;
using System;
using TerraBrush;

public partial class ResourceGeneration : Node3D
{
	
	[Export] public TerraBrush.TerraBrush Map;
	[Export]
	public Vector3 MapSize { get; set; }
	[Export]
	public int RedCubesCount { get; set; } = 10; // Number of red cubes to spawn

	[Export]
	public int GreenCylindersCount { get; set; } =10; // Number of green cylinders to spawn

	public override void _Ready()
	{
		 if (Map != null)
		{
			// Calculate the map size based on the heightmap resolution and terrain scale
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
	// Get the heightmap resolution (assuming it's square)
	int heightmapResolution = map.ZonesSize - 500;
	GD.Print("Heightmap Resolution: " + heightmapResolution);

	// Get the terrain scale
	Vector3 terrainScale = map.Scale;
	GD.Print("Terrain Scale: " + terrainScale);

	// Calculate the map size
	float mapWidth = heightmapResolution * terrainScale.X;
	float mapDepth = heightmapResolution * terrainScale.Z;

	Vector3 mapSize = new Vector3(mapWidth, 0, mapDepth);
	GD.Print("Calculated Map Size: " + mapSize);

	return mapSize;
}

	private void SpawnObjects()
	{
		// Spawn red cubes
		for (int i = 0; i < RedCubesCount; i++)
		{
			SpawnCube(Colors.Red);
		}

		// Spawn green cylinders
		for (int i = 0; i < GreenCylindersCount; i++)
		{
			SpawnCylinder(Colors.Green);
		}
	}

	private void SpawnCube(Color color)
	{
		// Create a new MeshInstance3D for the cube
		var cubeMesh = new BoxMesh();
		var cubeInstance = new MeshInstance3D
		{
			Mesh = cubeMesh
		};

		// Create a material for the cube
		var material = new StandardMaterial3D
		{
			AlbedoColor = color
		};
		cubeMesh.Material = material;

		// Add a collision shape to the cube
		var collisionShape = new CollisionShape3D
		{
			Shape = new BoxShape3D()
		};

		// Create a StaticBody3D for the cube
		var staticBody = new StaticBody3D();
		staticBody.AddChild(collisionShape);
		cubeInstance.AddChild(staticBody);

		// Set a random position for the cube
		cubeInstance.Position = GetRandomPosition();

		// Add the cube to the scene
		AddChild(cubeInstance);
	}

	private void SpawnCylinder(Color color)
	{
		// Create a new MeshInstance3D for the cylinder
		var cylinderMesh = new CylinderMesh();
		var cylinderInstance = new MeshInstance3D
		{
			Mesh = cylinderMesh
		};

		// Create a material for the cylinder
		var material = new StandardMaterial3D
		{
			AlbedoColor = color
		};
		cylinderMesh.Material = material;

		// Add a collision shape to the cylinder
		var collisionShape = new CollisionShape3D
		{
			Shape = new CylinderShape3D()
		};

		// Create a StaticBody3D for the cylinder
		var staticBody = new StaticBody3D();
		staticBody.AddChild(collisionShape);
		cylinderInstance.AddChild(staticBody);

		// Set a random position for the cylinder
		cylinderInstance.Position = GetRandomPosition();

		// Add the cylinder to the scene
		AddChild(cylinderInstance);
	}

	private Vector3 GetRandomPosition()
{
	// Generate a random position within the map bounds
	var random = new Random();
	float x = (float)random.NextDouble() * MapSize.X - MapSize.X / 2;
	float z = (float)random.NextDouble() * MapSize.Z - MapSize.Z / 2;

	// Sample the terrain height at the random X and Z coordinates
	float y = GetTerrainHeight(x, z);

	return new Vector3(x, y, z);
}

private float GetTerrainHeight(float x, float z)
{
	var a = new RayCast3D();
	var rayCast = new RayCast3D();
	rayCast.Position = new Vector3(x, MapSize.Y + 100, z);
	rayCast.TargetPosition = new Vector3(x, -100, z);
	rayCast.Enabled = true;
	rayCast.CollisionMask = 1; // Ensure this matches the terrain's collision layer
	rayCast.DebugShapeThickness = 1;
	

	AddChild(rayCast);
	rayCast.ForceRaycastUpdate();

	float height = 0;
	if (rayCast.IsColliding())
	{
		height = rayCast.GetCollisionPoint().Y;
		GD.Print("Terrain Height at (" + x + ", " + z + "): " + height);
	}
	else
	{
		GD.PrintErr("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"+ rayCast.Position + " " + rayCast.CollisionMask + " " + rayCast.TargetPosition);
		GD.PrintErr("RayCast3D did not detect terrain at (" + x + ", " + z + ")");
	}

	rayCast.QueueFree();

	return height;
}
}
