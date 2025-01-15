using ForeignGeneer.Assets.Scripts.craft;
using Godot;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class Fonderie : StaticBody3D,IFactory
{
    public Craft craft { get; set; }
    public FactoryStatic factoryStatic { get; set; }
    public ItemStatic output { get; set; }
    public short tier { get; set; }
    public float pollutionInd { get; set; }
    public void dismantle()
    {
        throw new System.NotImplementedException();
    }
}