using Godot;
using System.Collections.Generic;

public partial class BiomeGenerator : Node3D
{
	private List<Vector3> seedPoints = new List<Vector3>();
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public void GenerateSeedPoints(int count, Vector3 regionSize)
	{
		rng.Randomize();
		for (int i = 0; i < count; i++)
		{
			Vector3 seed = new Vector3(
				rng.RandfRange(0, regionSize.X),
				rng.RandfRange(0, regionSize.Y),
				rng.RandfRange(0, regionSize.Z)
			);
			seedPoints.Add(seed);
		}
	}
	private Dictionary<Vector3, List<Vector3>> voronoiCells = new Dictionary<Vector3, List<Vector3>>();

public void ComputeVoronoi(Vector3 regionSize, float resolution)
{
	foreach (var seed in seedPoints)
	{
		voronoiCells[seed] = new List<Vector3>();
	}

	for (float x = 0; x < regionSize.X; x += resolution)
	{
		for (float y = 0; y < regionSize.Y; y += resolution)
		{
			for (float z = 0; z < regionSize.Z; z += resolution)
			{
				Vector3 point = new Vector3(x, y, z);
				Vector3 closestSeed = GetClosestSeed(point);
				voronoiCells[closestSeed].Add(point);
			}
		}
	}
}

private Vector3 GetClosestSeed(Vector3 point)
{
	Vector3 closest = Vector3.Zero;
	float minDistance = float.MaxValue;

	foreach (var seed in seedPoints)
	{
		float distance = point.DistanceTo(seed);
		if (distance < minDistance)
		{
			minDistance = distance;
			closest = seed;
		}
	}
	return closest;
}
}
	
