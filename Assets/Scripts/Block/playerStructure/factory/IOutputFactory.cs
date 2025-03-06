namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IOutputFactory<TFactory> : IFactory<TFactory>
    where TFactory : FactoryStatic
{
    Inventory output { get; set; }
}