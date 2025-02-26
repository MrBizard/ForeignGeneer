namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory<TFactory> : IPlayerStructure
    where TFactory : FactoryStatic 
{
    TFactory factoryStatic { get; set; }
    void closeUi();
    void openUi();
}