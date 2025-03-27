using Godot;
using System;

public partial class LOD : MeshInstance3D
{
	[Export]
	private NodePath[] lodMeshPaths;
	private MeshInstance3D[] lodMeshes;

	[Export]
	private float[] distances = new float[] { 5f, 15f, 30f };

	private Node3D player;
	private int currentLOD = 0;

	public override void _Ready()
	{
		lodMeshes = new MeshInstance3D[lodMeshPaths.Length];
		for (int i = 0; i < lodMeshPaths.Length; i++)
		{
			lodMeshes[i] = GetNode<MeshInstance3D>(lodMeshPaths[i]);
			if (lodMeshes[i] == null)
			{
				GD.PrintErr($"LOD mesh {i} not found");
			}
		}

		foreach (var mesh in lodMeshes)
		{
			if (mesh != null) mesh.Visible = false;
		}

		if (lodMeshes.Length > 0 && lodMeshes[0] != null)
		{
			lodMeshes[0].Visible = true;
			currentLOD = 0;
		}

		player = FindPlayer();

		if (player == null)
		{

			Callable.From(FindPlayer).CallDeferred();
		}
	}

	private Node3D FindPlayer()
	{

		if (player != null) return player;

/*
		var playerGroup = GetTree().GetNodesInGroup("player");
		if (playerGroup.Count > 0)
		{
			return playerGroup[0] as Node3D;
		}*/

		var main = GetTree().CurrentScene;
		if (main != null)
		{
			var possiblePlayer = main.GetNodeOrNull<Node3D>("Personnage");
			if (possiblePlayer != null) return possiblePlayer;
		}

		return GetTree().Root.FindChild("Personnage", true, false) as Node3D;
	}

	public override void _Process(double delta)
	{
		if (player == null)
		{
			player = FindPlayer();
			if (player == null) return;
		}

		if (lodMeshes.Length == 0) return;

		float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
		int newLOD = CalculateLODLevel(distance);

		if (newLOD != currentLOD)
		{
			SwitchLOD(newLOD);
		}
	}

	private int CalculateLODLevel(float distance)
	{
		for (int i = 0; i < distances.Length; i++)
		{
			if (distance < distances[i])
			{
				return i;
			}
		}
		return distances.Length - 1;
	}

	private void SwitchLOD(int newLOD)
	{
		if (newLOD < 0 || newLOD >= lodMeshes.Length || lodMeshes[newLOD] == null)
			return;

		if (currentLOD >= 0 && currentLOD < lodMeshes.Length && lodMeshes[currentLOD] != null)
			lodMeshes[currentLOD].Visible = false;

		lodMeshes[newLOD].Visible = true;
		currentLOD = newLOD;
	}
}
