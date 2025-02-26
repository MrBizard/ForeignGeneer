namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IOutputFactory : IFactory<CraftingFactoryStatic>
{
    Inventory output { get; set; }
}