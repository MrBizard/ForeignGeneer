using ForeignGeneer.Assets.Scripts.craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : PlayerStructure
{
    Craft craft { get; set; }
    FactoryStatic factoryStatic { get; set; }
    ItemStatic output { get; set; }
    short tier { get; set; }
}