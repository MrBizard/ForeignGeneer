using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Fonderie : StaticBody3D, IFactory
{
    [Export] public RecipeList recipeList { get; set; }
    [Export] public PackedScene recipeListUiPackedScene { get; set; }
    [Export] public int inputSlotCount { get; set; } = 2;
    [Export] public PackedScene factoryUiPackedScene { get; set; }
    public float electricalCost = 0;
    public Craft craft { get; set; }
    public Inventory input { get; set; }
    public short tier { get; set; }
    public Inventory output { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer;
    public bool isCrafting { get; private set; } = false;

    private FonderieUi factoryUi;
    private RecetteList recipeListUi;
    private Manager manager;

    public override void _Ready()
    {
        base._Ready();
        if (recipeListUiPackedScene != null)
            init();
    }

    public void init()
    {
        input = new Inventory(inputSlotCount);
        output = new Inventory(1);
        recipeList?.init();
        input.onInventoryUpdated += OnInventoryUpdated;
        output.onInventoryUpdated += OnInventoryUpdated;
        manager = GetNode<Manager>("/root/Main/Manager");
    }

    public void setCraft(Recipe recipe)
    {
        if (recipe == null)
        {
            craft = null;
            GD.Print("Aucune recette définie.");
            return;
        }

        craft = new Craft(recipe);
        craft.init(input, output);
        openUi();
    }

    private void OnInventoryUpdated()
    {
        if (!isCrafting && craft != null && craft.compareRecipe())
        {
            var outputSlotItem = output.getItem(0);
            if (outputSlotItem == null || outputSlotItem.getStack() < outputSlotItem.getResource().getMaxStack)
            {
                startCraft();
            }
        }
        factoryUi?.UpdateUi();
    }

    private void startCraft()
    {
        if (isCrafting || !manager.hasEnergy(electricalCost))
        {
            return;
        }

        if (!craft.consumeResources())
        {
            GD.Print("Pas assez de ressources pour démarrer le craft.");
            return;
        }

        if (craft.recipe.duration <= 0)
        {
            GD.Print("La durée de la recette est invalide.");
            return;
        }

        isCrafting = true;
        craftProgress = 0f;
        craftTimer = new Timer();
        craftTimer.WaitTime = craft.recipe.duration;
        craftTimer.Timeout += OnCraftFinished;
        AddChild(craftTimer);
        craftTimer.Start();

        GD.Print("Craft démarré.");
    }

    private void OnCraftFinished()
    {
        isCrafting = false;
        craftTimer.QueueFree();

        if (craft.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();

            if (!outputAdded)
            {
                GD.Print("Impossible d'ajouter l'objet fabriqué à l'inventaire de sortie.");
                return;
            }
            else
            {
                GD.Print("Craft terminé avec succès.");
                startCraft();
            }
        }

        factoryUi?.UpdateProgressBar(0f);
        OnInventoryUpdated();
    }

    public void openUi()
    {
        closeUi();
        if (craft == null)
        {
            UiManager.Instance.OpenUI("RecipeListUI", this);
        }
        else
        {
            UiManager.Instance.OpenUI("FonderieUI", this);
        }
    }

    public void closeUi()
    {
        UiManager.Instance.CloseUI();
    }

    public void dismantle()
    {
        if (isCrafting)
        {
            craftTimer.Stop();
            craftTimer.QueueFree();
        }

        input.onInventoryUpdated -= OnInventoryUpdated;
        output.onInventoryUpdated -= OnInventoryUpdated;

        QueueFree();
        GD.Print("Fonderie détruite.");
    }

    public float pollutionInd { get; set; }
}
