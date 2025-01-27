using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.central;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class CoalCentral : StaticBody3D, ICentral
{
    [Export] private PackedScene _centralPackedScene;
    [Export] private PackedScene _recipeListUiPackedScene;
    [Export] public RecipeList recipeList { get; set; }
    public float pollutionInd { get; set; }
    public Inventory input { get; set; }
    public Craft craft { get; set; }
    
    public short tier { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer { get; set; }
    public bool isCrafting { get; private set; } = false;
    private CentralUi _centralUi;
    private RecetteList _recipeListUi;
    private Manager _manager;
    public float electricalCost = 0;

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(1);
        recipeList?.init();
        input.onInventoryUpdated += onInventoryUpdated;
        _manager = (Manager)GetNode("/root/Main/Manager");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (isCrafting)
        {
            craftProgress = 1f - (float)(craftTimer.TimeLeft / craft.recipe.duration);
            _centralUi?.updateProgressBar(craftProgress);
            _centralUi?.updateElectricity(); // Mettre à jour l'affichage de l'électricité
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("inventory")) // Remplacez "inventory" par l'action correspondant à la touche Tab
        {
            if (_centralUi != null || _recipeListUi != null)
            {
                closeUi();
            }
        }
    }

    private void onInventoryUpdated()
    {
        if (!isCrafting && craft.compareRecipe())
        {
            startCraft();
        }
        _centralUi?.updateUi();
    }

    public void setCraft(Recipe recipe)
    {
        craft = recipe == null ? null : new Craft(recipe);
        if (craft != null)
            craft.init(input, null);
        openUi();
    }

    private void startCraft()
    {
        if (isCrafting) return;

        _manager.addGlobalElectricity(electricalCost);
        if (!craft.consumeResources()) return;
        if (craft.recipe.duration <= 0) return;

        isCrafting = true;
        craftProgress = 0f;
        craftTimer = new Timer();
        craftTimer.WaitTime = craft.recipe.duration;
        craftTimer.Timeout += onCraftFinished;
        AddChild(craftTimer);
        craftTimer.Start();
    }

    private void onCraftFinished()
    {
        isCrafting = false;
        _manager.removeGlobalElectricity(electricalCost);
        craftTimer.QueueFree();

        if (craft.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();
            if (!outputAdded) return;
            startCraft();  // Répétez le craft si nécessaire
        }

        // Retirer l'électricité lorsque le craft s'arrête
        _centralUi?.updateProgressBar(0f);
        _centralUi?.updateElectricity();
        onInventoryUpdated();
    }

    public void closeUi()
    {
        UiManager.Instance.CloseUI();
    }

    public void dismantle()
    {
        // Non implémenté pour l'instant
        throw new System.NotImplementedException();
    }

    public void openUi()
    {
        closeUi(); // Ferme toute UI précédente avant d'en ouvrir une nouvelle
        
        if (craft == null)
        {
            UiManager.Instance.OpenUI("RecipeListUI", this);
        }
        else
        {
            UiManager.Instance.OpenUI("CentralUi", this);
        }

        // Définir le mode de la souris sur Visible
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
