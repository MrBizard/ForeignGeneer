using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using Godot;
using ForeignGeneer.Assets.Scripts;

namespace ForeignGeneer.Assets.Scripts.Interface.Factory;

public partial class CraftingTableUi : Control,BaseUi
{
    [Export] private SlotUI _slot;
    [Export] private GridContainer _gridContainer;
    [Export] private ProgressBar _progressBar;
    [Export] private PackedScene _recipeUiPacked;
    
    private CraftingTable _craftingTable;
    public void initialize(object data)
    {
        _craftingTable = data as CraftingTable;
        initOutputSlot();
    }
    
    public void updateUi()
    {
        updateProgressBar();
        updateOutputSlot();
    }

    private void initRecipeList()
    {
        foreach (Recipe recipe in _craftingTable.itemStatic.recipeList)
        {
            RecipeChoiceUi recipeUi = _recipeUiPacked.Instantiate<RecipeChoiceUi>();
            recipeUi.init(recipe);
            recipeUi.Connect(nameof(RecipeChoiceUi.RecipeClicked), new Callable(this, nameof(onRecipeClicked)));
            _gridContainer.AddChild(recipeUi);
        }
    }
    private void updateOutputSlot()
    {
        if (_slot != null)
        {
            if (_craftingTable.output.getItem(0) != null)
            {
                _slot.updateUi();  
            }
            else
            {
                _slot.clearSlot(); 
            }
        }
    }
    private void initOutputSlot()
    {
        _slot.initialize(_craftingTable.output, 0);
    }
    public void close()
    {
        _craftingTable.interact(InteractType.Close);
    }

    private void updateProgressBar()
    {
        _progressBar.Value = _craftingTable.craft.craftProgress * 100;
    }

    private void onRecipeClicked(Recipe recipe)
    {
        _craftingTable.setCraft(recipe);
    }
    public void update(InterfaceType? interfaceType)
    {
        switch (interfaceType)
        {
            case InterfaceType.Progress:
                updateProgressBar();
                break;
        }
    }
}