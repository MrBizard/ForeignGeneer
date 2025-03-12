namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IOutputFactory<TFactory> : IPlayerStructure<TFactory>
    where TFactory : FactoryStatic
{
    Inventory output { get; set; }
}