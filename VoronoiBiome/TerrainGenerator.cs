using Godot;
using System;

public partial class TerrainGenerator : Node3D
{
	[Export] public PackedScene[] BiomeScenes;
	[Export] public int WorldSize = 4;

	public override void _Ready()
	{
		GenerateWorld();
	}

	private void GenerateWorld()
	{
		Random random = new Random();

		for (int x = 0; x < WorldSize; x++)
		{
			for (int z = 0; z < WorldSize; z++)
			{
				var biomeScene = BiomeScenes[random.Next(BiomeScenes.Length)];
				var biomeInstance = biomeScene.Instantiate() as Biome;

				biomeInstance.Translate(new Vector3(x * 50, 0, z * 50));
				biomeInstance.Scale = new Vector3(50, 1, 50);

				AddChild(biomeInstance);
				this.Name = biomeInstance.Name;
				biomeInstance.SpawnResources();
			}
		}
	}
}
