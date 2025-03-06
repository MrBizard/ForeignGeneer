namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IOutputFactory : IFactory
{
    Inventory output { get; set; }
}