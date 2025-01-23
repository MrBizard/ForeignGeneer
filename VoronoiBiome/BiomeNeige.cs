using Godot;

public partial class BiomeNeige : Biome
{
	public override void ApplyEffect(Player player)
	{
		player.Speed *= 0.5f; // Slow the player
		GD.Print("Player slowed in Snow Biome.");
	}

	public override void OnExit(Player player)
	{
		player.Speed /= 0.5f; // Reset speed
		GD.Print("Player left Snow Biome.");
	}

	public override void SpawnResources()
	{
		base.SpawnResources(); // Spawn base resources
		GD.Print("Spawning snow-specific resources.");
	}
}
