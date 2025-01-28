using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Fonderie : StaticBody3D, IFactory
{
    [Export] public RecipeList recipeList { get; set; }
    [Export] public int inputSlotCount { get; set; } = 2;
    public float electricalCost = 0;
    public Craft craft { get; set; }
    public Inventory input { get; set; }
    public short tier { get; set; }
    public Inventory output { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer;
    public bool isCrafting { get; private set; } = false;

    private Manager _manager;

    public override void _Ready()
    {
        base._Ready();
        init();
    }

    /// <summary>
    /// Initialise la fonderie.
    /// </summary>
    public void init()
    {
        input = new Inventory(inputSlotCount);
        output = new Inventory(1);
        recipeList?.init();
        input.onInventoryUpdated += onInventoryUpdated;
        output.onInventoryUpdated += onInventoryUpdated;
        _manager = GetNode<Manager>("/root/Main/Manager");
    }

    public override void _Process(double delta)
    {
        if (isCrafting)
        {
            craftProgress += (float)delta / craft.recipe.duration;
            updateProgressBar(craftProgress);
        }
    }
    /// <summary>
    /// Définit la recette à utiliser pour le craft.
    /// </summary>
    /// <param name="recipe">La recette à utiliser.</param>
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

    private void onInventoryUpdated()
    {
        if (!isCrafting && craft != null && craft.compareRecipe())
        {
            var outputSlotItem = output.getItem(0);
            if (outputSlotItem == null || outputSlotItem.getStack() < outputSlotItem.getResource().getMaxStack)
            {
                startCraft();
            }
        }
        updateUi();
    }

    private void startCraft()
    {
        if (isCrafting || !_manager.hasEnergy(electricalCost))
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
        craftTimer.Timeout += onCraftFinished;
        AddChild(craftTimer);
        craftTimer.Start();
        updateProgressBar(0f);
        updateUi();
        GD.Print("Craft démarré.");
    }

    private void onCraftFinished()
    {
        isCrafting = false;
        craftTimer.QueueFree();
        bool outputAdded = craft.addOutput();
        GD.Print("Craft terminé. Output ajouté : " + outputAdded);
        updateUi(); 
    }

    /// <summary>
    /// Ouvre l'interface utilisateur de la fonderie.
    /// </summary>
    public void openUi()
    {
        closeUi();
        if (craft == null)
        {
            UiManager.instance.openUi("RecipeListUI", this);
        }
        else
        {
            UiManager.instance.openUi("FonderieUI", this);
        }
    }

    /// <summary>
    /// Ferme l'interface utilisateur de la fonderie.
    /// </summary>
    public void closeUi()
    {
        UiManager.instance.closeUi();
    }

    /// <summary>
    /// Détruit la fonderie.
    /// </summary>
    public void dismantle()
    {
        if (isCrafting)
        {
            craftTimer.Stop();
            craftTimer.QueueFree();
        }

        input.onInventoryUpdated -= onInventoryUpdated;
        output.onInventoryUpdated -= onInventoryUpdated;

        QueueFree();
        GD.Print("Fonderie détruite.");
    }

    public float pollutionInd { get; set; }

    private void updateUi()
    {
        if (UiManager.instance.IsAnyUiOpen())
        {
            UiManager.instance.refreshCurrentUi(this);
        }
    }

    private void updateProgressBar(float progress)
    {
        if (UiManager.instance.isUiOpen("FonderieUI"))
        {
            FonderieUi fonderieUi = UiManager.instance.currentOpenUi as FonderieUi;
            fonderieUi?.updateProgressBar(progress);
        }
    }
}