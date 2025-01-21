namespace ForeignGeneer.Assets.Scripts.block.playerStructure;

public interface PlayerStructure
{
    public float pollutionInd { get; set; }
    public void dismantle();
    public void openUi();
}