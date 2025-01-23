using Godot;
using System;

public partial class BiomeSulfur : Biome
{
	public override void ApplyEffect(Player player)
	{
		//player.healthBar.Value -= 0.05f;
		GD.Print("Player DAMAGED in the sulfur");
	}

	public override void SpawnResources()
	{
		GD.Print("Spawning snow resources.");
	}
}
