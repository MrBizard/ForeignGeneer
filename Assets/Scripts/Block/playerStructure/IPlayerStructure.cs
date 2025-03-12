using ForeignGeneer.Assets.Scripts.Static.Craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure;

public interface IPlayerStructure<TItemStatic>
    where TItemStatic : ItemStatic
{
    TItemStatic itemStatic { get; set; }
    public void dismantle();
    public void openUi();
    void closeUi();
    
}