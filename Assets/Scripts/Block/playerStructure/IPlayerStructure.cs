namespace ForeignGeneer.Assets.Scripts.block.playerStructure;

public interface IPlayerStructure
{
    public float pollutionInd { get; set; }
    public void dismantle();
    public void openUi();
    void closeUi();
}