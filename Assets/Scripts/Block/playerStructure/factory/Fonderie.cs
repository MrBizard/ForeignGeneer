using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using Godot;
using Godot.Collections;

public partial class Fonderie : StaticBody3D, IFactory
{
    [Export] private Recipe recipe;
    [Export]public Array<Recipe> recipeList { get; set; }
    [Export] private PackedScene _recipeListUiPackedScene;
    public Craft craft { get; set; }
    public FactoryStatic factoryStatic { get; set; }
    public Inventory input { get; set; }
    public short tier { get; set; }
    public Inventory output { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer;
    public bool isCrafting { get; private set; } = false;

    [Export] private int inputSlotCount = 2;
    [Export] private PackedScene factoryUiPackedScene;

    private FonderieUi factoryUi;

    public Fonderie()
    {
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("interragir"))
        {
            openUi();
        }

        if (isCrafting)
        {
            craftProgress = 1f - (float)(craftTimer.TimeLeft / craft.recipe.duration);

            // Mettre à jour la ProgressBar
            if (factoryUi != null)
            {
                factoryUi.updateProgressBar(craftProgress);
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(inputSlotCount);
        output = new Inventory(1); // 1 slot pour l'output
        
        recipe.initRecipe();
        GD.Print("recipe "+recipe.input.Count);
        input.onInventoryUpdated += onInventoryUpdated;
        output.onInventoryUpdated += onInventoryUpdated;

        // Vérifier que la recette et l'output sont valides
        if (recipe == null || recipe.output == null)
        {
            GD.PrintErr("La recette ou l'output est invalide.");
        }
        else
        {
            GD.Print($"Recette initialisée avec output : {recipe.output.getResource().GetName()} (x{recipe.output.getStack()})");
        }
    }

    private void onInventoryUpdated()
    {
        if (!isCrafting && craft.compareRecipe())
        {
            var outputSlotItem = output.getItem(0);
            if (outputSlotItem == null || outputSlotItem.getStack() < outputSlotItem.getResource().getMaxStack)
            {
                startCraft();
            }
            else
            {
                GD.Print("Le slot de sortie est plein. Craft arrêté.");
            }
        }

        // Mettre à jour l'UI de la fonderie
        factoryUi?.updateUi();
    }

    private void startCraft()
    {
        if (isCrafting)
        {
            return;
        }

        if (!craft.consumeResources())
        {
            return;
        }

        if (craft.recipe.duration <= 0)
        {
            GD.PrintErr("Craft duration must be greater than zero.");
            return;
        }

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
        craftTimer.QueueFree();

        if (craft.recipe.output != null)
        {
            bool outputAdded = craft.addOutput();

            if (!outputAdded)
            {
                GD.Print("Le slot de sortie est plein. Craft arrêté.");
            }
            else
            {
                GD.Print("Output ajouté avec succès.");
                startCraft(); // Démarrer un nouveau craft si l'output a été ajouté
            }
        }
        else
        {
            GD.PrintErr("La recette n'a pas d'output défini.");
        }

        // Réinitialiser la ProgressBar
        if (factoryUi != null)
        {
            factoryUi.updateProgressBar(0f);
        }

        // Vérifier si un nouveau craft est possible
        onInventoryUpdated();
    }

    public float pollutionInd { get; set; }

    public void dismantle()
    {
        throw new System.NotImplementedException();
    }

    public void openUi()
{
    var inventoryUi = GetNode<InventoryUi>("/root/Main/PlayerInventoryManager/InventoryUi");

    if (factoryUi == null)
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        // Vérifie si le craft est null
        if (craft == null)
        {
            // Instancie et affiche la liste des recettes
            if (_recipeListUiPackedScene != null)
            {
                var recipeListUi = _recipeListUiPackedScene.Instantiate<RecetteList>(); // Assurez-vous que RecipeListUi est le bon type
                GetTree().Root.AddChild(recipeListUi);

                // Initialise la liste des recettes
                recipeListUi.initialize(this); // Passez la fonderie comme paramètre si nécessaire
            }
            else
            {
                GD.PrintErr("_recipeListUiPackedScene n'est pas défini.");
            }
        }
        else
        {
            // Instancie et affiche l'interface normale de la fonderie
            if (factoryUiPackedScene != null)
            {
                factoryUi = factoryUiPackedScene.Instantiate<FonderieUi>();
                GetTree().Root.AddChild(factoryUi);

                if (inventoryUi == null)
                {
                    GD.PrintErr("InventoryUi non trouvé.");
                    return;
                }

                factoryUi.initialize(this, inventoryUi);

                // Afficher l'inventaire à côté de la fonderie
                if (!inventoryUi.Visible)
                {
                    inventoryUi.Visible = true;
                    inventoryUi.Position = new Vector2(factoryUi.Position.X - inventoryUi.Size.X - 10, factoryUi.Position.Y);
                }

                // Mettre à jour la ProgressBar dès l'ouverture de l'interface
                if (isCrafting)
                {
                    factoryUi.updateProgressBar(craftProgress);
                }
                else
                {
                    factoryUi.updateProgressBar(0f);
                }
            }
            else
            {
                GD.PrintErr("factoryUiPackedScene n'est pas défini.");
            }
        }
    }
    else
    {
        // Ferme l'interface actuelle
        factoryUi.closeUi();
        factoryUi = null;

        // Masquer l'inventaire si on ferme la fonderie
        if (!Input.IsActionJustPressed("inventory"))
        {
            inventoryUi.Visible = false;
        }
    }
}
}