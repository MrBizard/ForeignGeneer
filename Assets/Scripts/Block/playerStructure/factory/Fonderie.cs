using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

public partial class Fonderie : StaticBody3D, IFactory
{
    [Export] public RecipeList recipeList { get; set; }
    [Export] private PackedScene _recipeListUiPackedScene;
    [Export] private int _inputSlotCount = 2;
    [Export] private PackedScene _factoryUiPackedScene;
    public Craft craft { get; set; }
    public FactoryStatic factoryStatic { get; set; }
    public Inventory input { get; set; }
    public short tier { get; set; }
    public Inventory output { get; set; }
    public float craftProgress { get; private set; }
    public Timer craftTimer;
    public bool isCrafting { get; private set; } = false;
    

    private FonderieUi _factoryUi;
    private RecetteList _recipeListUi;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isCrafting)
        {
            craftProgress = 1f - (float)(craftTimer.TimeLeft / craft.recipe.duration);

            if (_factoryUi != null)
            {
                _factoryUi.updateProgressBar(craftProgress);
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        input = new Inventory(_inputSlotCount);
        output = new Inventory(1);
        recipeList.init();
        input.onInventoryUpdated += onInventoryUpdated;
        output.onInventoryUpdated += onInventoryUpdated;
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

        _factoryUi?.updateUi();
    }

    public void setCraft(Recipe recipe)
    {
        craft = recipe == null ? null : new Craft(recipe);
        if (craft != null)
            craft.init(input, output);
        openUi(); // Mettre à jour l'UI après avoir défini le craft
    }

    private void startCraft()
    {
        GD.Print("craft");
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
                startCraft();
            }
        }
        else
        {
            GD.PrintErr("La recette n'a pas d'output défini.");
        }

        if (_factoryUi != null)
        {
            _factoryUi.updateProgressBar(0f);
        }

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

        // Fermer les fenêtres existantes
        closeUi();

        // Afficher la fenêtre appropriée
        if (craft == null)
        {
            if (_recipeListUiPackedScene != null)
            {
                _recipeListUi = _recipeListUiPackedScene.Instantiate<RecetteList>();
                GetTree().Root.AddChild(_recipeListUi);
                _recipeListUi.Initialize(this);
                inventoryUi.Visible = false;
            }
            else
            {
                GD.PrintErr("recipeListUiPackedScene n'est pas défini.");
            }
        }
        else
        {
            if (_factoryUiPackedScene != null)
            {
                _factoryUi = _factoryUiPackedScene.Instantiate<FonderieUi>();
                GetTree().Root.AddChild(_factoryUi);

                if (inventoryUi == null)
                {
                    GD.PrintErr("InventoryUi non trouvé.");
                    return;
                }

                _factoryUi.initialize(this, inventoryUi);

                if (!inventoryUi.Visible)
                {
                    inventoryUi.Visible = true;
                    inventoryUi.Position = new Vector2(_factoryUi.Position.X - inventoryUi.Size.X - 10, _factoryUi.Position.Y);
                }

                if (isCrafting)
                {
                    _factoryUi.updateProgressBar(craftProgress);
                }
                else
                {
                    _factoryUi.updateProgressBar(0f);
                }
            }
            else
            {
                GD.PrintErr("factoryUiPackedScene n'est pas défini.");
            }
        }

        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void closeUi()
    {
        // Fermer FonderieUi si elle est ouverte
        if (_factoryUi != null)
        {
            _factoryUi.closeUi();
            _factoryUi = null;
        }

        // Fermer RecetteListUi si elle est ouverte
        if (_recipeListUi != null)
        {
            _recipeListUi.QueueFree();
            _recipeListUi = null;
        }

        // Masquer l'inventaire
        var inventoryUi = GetNode<InventoryUi>("/root/Main/PlayerInventoryManager/InventoryUi");
        if (inventoryUi != null)
        {
            inventoryUi.Visible = false;
        }

        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }
}