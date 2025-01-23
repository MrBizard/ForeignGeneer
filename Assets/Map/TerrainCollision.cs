using Godot;
using System;

public partial class TerrainCollision : StaticBody3D
{
	// Reference to the heightmap texture
	[Export] private Texture2D heightmapTexture;

	// Terrain dimensions
	[Export] private int terrainWidth = 100; 
	[Export] private int terrainDepth = 100; 
	[Export] private float terrainHeightScale = 50.0f; 

	// Collision shape resolution
	[Export] private int collisionResolution = 100; // Number of vertices along each axis

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GenerateCollisionShape();
	}

	// Generates a collision shape based on the heightmap texture
	private void GenerateCollisionShape()
	{
		if (heightmapTexture == null)
		{
			GD.PrintErr("Heightmap texture is not assigned.");
			return;
		}

		// Get the heightmap image data
		var heightmapImage = heightmapTexture.GetImage();
		if (heightmapImage == null)
		{
			GD.PrintErr("Failed to get heightmap image data.");
			return;
		}

		// Create a HeightMapShape for the collision
		var heightmapShape = new HeightMapShape3D();
		heightmapShape.MapWidth = collisionResolution;
		heightmapShape.MapDepth = collisionResolution;
		heightmapShape.MapData = new float[collisionResolution * collisionResolution];

		// Sample the heightmap and populate the collision shape data
		for (int z = 0; z < collisionResolution; z++)
		{
			for (int x = 0; x < collisionResolution; x++)
			{
				// Calculate the UV coordinates for sampling the heightmap
				float u = (float)x / (collisionResolution - 1);
				float v = (float)z / (collisionResolution - 1);

				// Sample the heightmap at the UV coordinates
				float height = heightmapImage.GetPixel(
					(int)(u * heightmapImage.GetWidth()),
					(int)(v * heightmapImage.GetHeight())
				).R; // Use the red channel for height

				// Scale the height value and store it in the collision shape data
				heightmapShape.MapData[z * collisionResolution + x] = height * terrainHeightScale;
			}
		}

		// Create a CollisionShape3D node and assign the heightmap shape
		var collisionShapeNode = new CollisionShape3D();
		collisionShapeNode.Shape = heightmapShape;

		// Add the collision shape to the static body
		AddChild(collisionShapeNode);
	}
}
