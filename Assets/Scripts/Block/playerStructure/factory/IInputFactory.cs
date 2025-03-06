using ForeignGeneer.Assets.Scripts.Static.Craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IInputFactory<TFactory> : IFactory<TFactory>
    where TFactory : FactoryStatic
{
    public Craft craft { get; set; }
    void setCraft(Recipe recipe);
    Inventory input { get; set; }
}