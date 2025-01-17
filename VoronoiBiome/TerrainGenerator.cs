using Godot;
using Godot.Collections;

public partial class TerrainGenerator : MeshInstance3D
{
	private FastNoiseLite noise = new FastNoiseLite();
	private Array<Vector3> vertices = new Godot.Collections.Array<Vector3>();
	private Array<int> indices = new Godot.Collections.Array<int>();

	public override void _Ready()
	{
		// Configure the noise generator
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth;
		noise.Frequency = 0.1f; // Adjust frequency for scale
		noise.Seed = (int)GD.Randi();
		
		var material = new StandardMaterial3D();
		material.AlbedoColor = Colors.Green; // Choose a visible color
		Mesh.SurfaceSetMaterial(0, material);
		// Calculate normals
		var normals = new Godot.Collections.Array<Vector3>();
		for (int i = 0; i < vertices.Count; i++)
		{
			normals.Add(Vector3.Up); // Simple upward normals for a flat mesh
		}

 		normals.Add(Vector3.Up);

		GenerateTerrain();
		
	
	}

	private void GenerateTerrain()
	{
		// Create a grid size (e.g., 10x10 plane)
		int width = 50;
		int depth = 50;
		float spacing = 1.0f;

		// Create vertex and index arrays


		// Generate vertices
		for (int x = 0; x <= width; x++)
		{
			for (int z = 0; z <= depth; z++)
			{
				float height = noise.GetNoise2D(x * 0.1f, z * 0.1f) * 5.0f; // Scale noise
				vertices.Add(new Vector3(x * spacing, height, z * spacing));
			}
		}

		// Generate indices for a triangle grid
		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < depth; z++)
			{
				int topLeft = x * (depth + 1) + z;
				int topRight = topLeft + 1;
				int bottomLeft = (x + 1) * (depth + 1) + z;
				int bottomRight = bottomLeft + 1;

				// First triangle
				indices.Add(topLeft);
				indices.Add(bottomLeft);
				indices.Add(topRight);

				// Second triangle
				indices.Add(topRight);
				indices.Add(bottomLeft);
				indices.Add(bottomRight);
			}
		}

		// Create a new ArrayMesh
		var arrayMesh = new ArrayMesh();
		var surfaceArrays = new Godot.Collections.Array();
		surfaceArrays.Resize((int)ArrayMesh.ArrayType.Max);

		surfaceArrays[(int)ArrayMesh.ArrayType.Vertex] = vertices;
		surfaceArrays[(int)ArrayMesh.ArrayType.Index] = indices;

		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArrays);
		Mesh = arrayMesh;
  		this.Transform = new Transform3D(Basis.Identity, new Vector3(-25, 0, -25));	}
}
