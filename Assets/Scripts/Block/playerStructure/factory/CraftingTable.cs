using ForeignGeneer.Assets.Scripts.Static.Craft.craft;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class CraftingTable: PlayerBaseStructure,IInputFactory<CraftingFactoryStatic>, IOutputFactory<CraftingFactoryStatic>
{
    public CraftingFactoryStatic itemStatic { get; }
    public BaseCraft craft { get; set; }
    public Inventory output { get; set; }
    public Inventory input { get; set; }
    public void setCraft(Recipe recipe)
    {
        craft = new InventoryCraft(recipe,input,output);
    }
    
    public override void openUi()
    {
        
    }

    public override void closeUi()
    {
        base.closeUi();
        
    }
}