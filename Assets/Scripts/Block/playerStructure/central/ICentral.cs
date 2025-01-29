using Godot;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.central;

public interface ICentral : IPlayerStructure, IRecipeUser
{
    Inventory input { get; set; }
    Craft craft { get; set; }
    public float craftProgress { get; }
    public Timer craftTimer { get; set;}
    public bool isCrafting { get;}
}