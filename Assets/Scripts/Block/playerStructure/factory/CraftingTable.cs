namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class CraftingTable: PlayerBaseStructure, IOutputFactory<CraftingFactoryStatic>
{
    public CraftingFactoryStatic itemStatic
    {
        get => base.itemStatic as CraftingFactoryStatic;
        set => SetItemStatic(value);
    }
    public BaseCraft craft { get; set; }
    public Inventory output { get; set; }
    
    public void setCraft(Recipe recipe)
    {
        craft = new BulkCraftWithOutput(recipe,InventoryManager.Instance.inventory,output);
        craft.startCraft(onCraftFinished);
    }

    public void onCraftFinished()
    {
        craft.stopCraft();
        craft.addOutput();
    }
    public override void openUi()
    {
        
    }

    public override void closeUi()
    {
        base.closeUi();
    }
}