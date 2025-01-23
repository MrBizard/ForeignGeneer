using System;
using Godot;

public partial class Biome : Node3D
{
	[Export] public float Radius = 50f;
	[Export] public float Height = 100f;
	[Export] public Material BiomeMaterial;
	[Export] public string BiomeEffect;

	public override void _Ready()
	{
		SpawnResources();
	}

	public virtual void ApplyEffect(Player player)
	{
		GD.Print($"Applying {BiomeEffect} to player.");
	}

	public virtual void OnExit(Player player)
	{
		GD.Print($"Player left {Name}.");
	}

	public virtual void SpawnResources()
	{
		var resourceScene = GD.Load<PackedScene>("res://scenes/resources/Resource.tscn");
		for (int i = 0; i < 10; i++) 
		{
			var resource = resourceScene.Instantiate<StaticBody3D>();
			resource.Position = GetRandomPointInBiome();
			GetNode<Node3D>("ResourceSpawnArea").AddChild(resource);
		}
	}

	private Vector3 GetRandomPointInBiome()
	{
		Random random = new Random();
		double angle = random.NextDouble() * 2 * Math.PI;
		double distance = Math.Sqrt(random.NextDouble()) * Radius;

		float x = Position.X + (float)(Math.Cos(angle) * distance);
		float z = Position.Z + (float)(Math.Sin(angle) * distance);
		float y = Position.Y; 

		return new Vector3(x, y, z);
	}
}
