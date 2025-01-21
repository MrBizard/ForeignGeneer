
using System.Collections.Generic;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : PlayerStructure
{
    Craft craft { get; set; }
    FactoryStatic factoryStatic { get; set; }
    Inventory input { get; set; }
    short tier { get; set; }
}