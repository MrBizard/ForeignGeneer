using ForeignGeneer.Assets.Scripts.Static.Craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IInputFactory : IFactory<CraftingFactoryStatic>
{
    public Craft craft { get; set; }
    void setCraft(Recipe recipe);
    Inventory input { get; set; }
}