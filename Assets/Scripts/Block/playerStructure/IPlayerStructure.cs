using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.Static.Craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure;

public interface IPlayerStructure<TItemStatic>: IInteractable
    where TItemStatic : ItemStatic
{
    TItemStatic itemStatic { get; }
    public void dismantle();
    
}