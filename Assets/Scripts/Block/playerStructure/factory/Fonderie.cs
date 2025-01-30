using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class Fonderie : StaticBody3D, IFactory
{
    [Export] public int inputSlotCount { get; set; } = 2;
    [Export]public FactoryStatic factoryStatic { get; set; }
    public Craft craft { get; set; }
    public Inventory input { get; set; }
    public Inventory output { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;
    public RecipeList recipeList 
    {
        get => factoryStatic?.recipeList;
        set 
        {
            if (factoryStatic != null) 
            {
                factoryStatic.recipeList = value;
            }
        }
    }

    private FonderieUi _fonderieUi;
    public override void _Ready()
    {
        base._Ready();
        init();
    }

    /// <summary>
    /// Initialise la fonderie avec les inventaires d'entrée et de sortie et la liste des recettes.
    /// </summary>
    public void init()
    {
        input = new Inventory(inputSlotCount);
        output = new Inventory(1);
        factoryStatic.recipeList?.init();
        input.onInventoryUpdated += onInventoryUpdated;
        output.onInventoryUpdated += onInventoryUpdated;
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
    /// <param name="recipe">La recette à utiliser pour le craft.</param>
    public void setCraft(Recipe recipe)
    {
        if (recipe == null)
        {
            craft = null;
            return;
        }

        craft = new Craft(recipe);
        craft.init(input, output);
        openUi();
    }

    /// <summary>
    /// Mise à jour de l'inventaire. Démarre le craft si les conditions sont remplies.
    /// </summary>
    private void onInventoryUpdated()
    {
        if (!isCrafting && craft != null && craft.compareRecipe())
        {
            var outputSlotItem = output.getItem(0);
            if (outputSlotItem == null || outputSlotItem.getStack() < outputSlotItem.getResource().getMaxStack)
            {
                if (craft.consumeResources())
                {
                    startCraft();
                }
            }
        }
        updateUi();
    }

    /// <summary>
    /// Démarre le processus de fabrication si les ressources et les conditions sont valides.
    /// </summary>
    private void startCraft()
    {
        if (isCrafting || EnergyManager.instance.hasEnergy(factoryStatic.electricalCost))
        {
            return;
        }

        if (!craft.consumeResources())
        {
            return;
        }

        if (craft.recipe.duration <= 0)
        {
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
    }

    /// <summary>
    /// Lorsque le processus de fabrication est terminé, ajoute l'item au slot de sortie et redémarre si nécessaire.
    /// </summary>
    private void onCraftFinished()
    {
        isCrafting = false;
        craftTimer.QueueFree();

        bool outputAdded = craft.addOutput();

        var outputSlotItem = output.getItem(0);
        if (outputAdded && (outputSlotItem == null || outputSlotItem.getStack() < outputSlotItem.getResource().getMaxStack))
        {
            startCraft();
        }

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
            _fonderieUi = (FonderieUi)UiManager.instance.getUi("FonderieUI");
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
    /// Détruit la fonderie et libère les ressources.
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
    }

    public float pollutionInd { get; set; }

    /// <summary>
    /// Met à jour l'interface utilisateur si elle est ouverte.
    /// </summary>
    private void updateUi()
    {
        if (UiManager.instance.IsAnyUiOpen())
        {
            UiManager.instance.refreshCurrentUi(this);
        }
    }

    /// <summary>
    /// Met à jour la barre de progression de la fonderie si l'UI est ouverte.
    /// </summary>
    private void updateProgressBar(float progress)
    {
        if (UiManager.instance.isUiOpen("FonderieUI"))
        {
            _fonderieUi.updateProgressBar(progress);
        }
    }

}
