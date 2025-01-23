using Godot;

public partial class MapGeneration : Node3D
{
	[Export] private int width = 64; // Width of the terrain
	[Export] private int depth = 256; // Depth of the terrain
	[Export] private float heightScale = 0.1f; // Scale of the terrain height
	[Export] private float holeProbability = 0.0001f; // Probability of creating holes

	private FastNoiseLite noise;

	public override void _Ready()
	{
		// Initialize noise generator
		noise = new FastNoiseLite();
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		noise.Seed = (int)GD.Randi(); // Random seed for variety

		GenerateTerrain();
	}

	private void GenerateTerrain()
	{
		// Create a new ArrayMesh
		var mesh = new ArrayMesh();

		// Generate vertices, UVs, and vertex colors
		var vertices = new Vector3[width * depth];
		var uv = new Vector2[width * depth];
		var colors = new Color[width * depth]; // Vertex colors
		var indices = new int[(width - 1) * (depth - 1) * 6];

		// Calculate the center offset
		float centerX = (width - 1) * 0.5f;
		float centerZ = (depth - 1) * 0.5f;

		for (int z = 0; z < depth; z++)
		{
			for (int x = 0; x < width; x++)
			{
				// Generate height using Perlin noise (range [-1, 1])
				float noiseValue = noise.GetNoise2D(x, z);

				// Scale the noise value to the desired height range
				float height = (noiseValue + 1) * 0.5f * heightScale; // Convert to [0, heightScale]

				// Randomly create holes
				if (GD.Randf() < holeProbability)
				{
					height = -10.0f; // Create a hole by setting a negative height
				}

				// Center the terrain by subtracting the center offset
				vertices[z * width + x] = new Vector3(x - centerX, height, z - centerZ);

				// Set UV coordinates for texture mapping
				uv[z * width + x] = new Vector2((float)x / (width - 1), (float)z / (depth - 1));

				// Set vertex color based on height
				if (height < -1.0f) // Example condition for holes
				{
					colors[z * width + x] = new Color(0.6f, 0.3f, 0.1f); // Brown color for holes
				}
				else
				{
					colors[z * width + x] = new Color(0, 1, 0); // Green color for terrain
				}
			}
		}

		// Smooth the terrain
		for (int z = 1; z < depth - 1; z++)
		{
			for (int x = 1; x < width - 1; x++)
			{
				// Average the height with neighboring vertices
				float height = 0;
				height += vertices[z * width + x].Y;
				height += vertices[(z + 1) * width + x].Y;
				height += vertices[(z - 1) * width + x].Y;
				height += vertices[z * width + (x + 1)].Y;
				height += vertices[z * width + (x - 1)].Y;
				height /= 5; 

				vertices[z * width + x].Y = height;
			}
		}

		// Generate indices for the mesh
		int index = 0;
		for (int z = 0; z < depth - 1; z++)
		{
			for (int x = 0; x < width - 1; x++)
			{
				int topLeft = z * width + x;
				int topRight = topLeft + 1;
				int bottomLeft = (z + 1) * width + x;
				int bottomRight = bottomLeft + 1;

				// Triangle 1: Clockwise order
				indices[index++] = topLeft;
				indices[index++] = topRight;
				indices[index++] = bottomLeft;

				// Triangle 2: Clockwise order
				indices[index++] = topRight;
				indices[index++] = bottomRight;
				indices[index++] = bottomLeft;
			}
		}

		// Create a surface from the vertices, indices, UVs, and vertex colors
		var surfaceArray = new Godot.Collections.Array();
		surfaceArray.Resize((int)Mesh.ArrayType.Max);
		surfaceArray[(int)Mesh.ArrayType.Vertex] = vertices;
		surfaceArray[(int)Mesh.ArrayType.Index] = indices;
		surfaceArray[(int)Mesh.ArrayType.TexUV] = uv;
		surfaceArray[(int)Mesh.ArrayType.Color] = colors; // Add vertex colors

		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);

		// Create a MeshInstance3D for the terrain
		var mapMesh = new MeshInstance3D
		{
			Mesh = mesh,
			Name = "MapMesh"
		};

		// Create a material that uses vertex colors
		var material = new StandardMaterial3D
		{
			VertexColorUseAsAlbedo = true // Use vertex colors as the base color
		};

		// Assign the material to the mesh
		mapMesh.MaterialOverride = material;

		// Add collision shape (HeightMapShape3D)
		var heightMapShape = new HeightMapShape3D();
		heightMapShape.MapWidth = width;
		heightMapShape.MapDepth = depth;
		heightMapShape.MapData = new float[width * depth];

		// Fill the heightmap data
		for (int z = 0; z < depth; z++)
		{
			for (int x = 0; x < width; x++)
			{
				heightMapShape.MapData[z * width + x] = vertices[z * width + x].Y;
			}
		}

		var collisionShape = new CollisionShape3D
		{
			Shape = heightMapShape
		};

		var staticBody = new StaticBody3D();
		staticBody.AddChild(collisionShape);

		// Add the MeshInstance3D and StaticBody3D to the root Node3D
		AddChild(mapMesh);
		AddChild(staticBody);
	}
}
