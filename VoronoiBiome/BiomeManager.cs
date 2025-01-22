using Godot;
using System.Collections.Generic;

public partial class BiomeManager : Node3D
{
	private List<Biome> biomes = new List<Biome>();
	private Player player;

	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
		foreach (var child in GetChildren())
		{
			if (child is Biome biome)
			{
				biomes.Add(biome);
			}
		}
	}

	public override void _Process(double delta)
	{
		foreach (var biome in biomes)
		{
			if (IsPlayerInBiome(player, biome))
			{
				biome.ApplyEffect(player);
			}
			else
			{
				biome.OnExit(player);
			}
		}
	}

	private bool IsPlayerInBiome(Player player, Biome biome)
	{
		float dx = player.Position.X - biome.Position.X;
		float dz = player.Position.Z - biome.Position.Z;
		return (dx * dx + dz * dz) <= biome.Radius * biome.Radius;
	}
}
