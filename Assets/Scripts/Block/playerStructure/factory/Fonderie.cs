using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;

public partial class Fonderie : StaticBody3D, IFactory
{
	// Propriétés exportées pour la configuration dans l'éditeur
	[Export] public RecipeList recipeList { get; set; }
	[Export] public PackedScene _recipeListUiPackedScene { get; set; }
	[Export] public int _inputSlotCount { get; set; } = 2;
	[Export] public PackedScene _factoryUiPackedScene { get; set; }
	public float electricalCost = 0;
	// Autres propriétés
	public Craft craft { get; set; }
	public Inventory input { get; set; }
	public short tier { get; set; }
	public Inventory output { get; set; }
	public float craftProgress { get; private set; }
	public Timer craftTimer;
	public bool isCrafting { get; private set; } = false;

	private FonderieUi _factoryUi;
	private RecetteList _recipeListUi;
	private Manager _manager;

	public override void _Ready()
	{
		base._Ready();
		if(_recipeListUiPackedScene != null)
			init();

	}

	public void init()
	{
		input = new Inventory(_inputSlotCount);
		output = new Inventory(1);
		recipeList?.init();
		input.onInventoryUpdated += onInventoryUpdated;
		output.onInventoryUpdated += onInventoryUpdated;
		_manager = GetNode<Manager>("/root/Main/Manager");
	}
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

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("inventory"))
		{
			if (_factoryUi != null || _recipeListUi != null)
			{
				closeUi();
			}
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
		}

		_factoryUi?.updateUi();
	}

	public void setCraft(Recipe recipe)
	{
		craft = recipe == null ? null : new Craft(recipe);
		if (craft != null)
			craft.init(input, output);
		openUi();
	}

	private void startCraft()
	{
		if (isCrafting || !_manager.hasEnergy(electricalCost))
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
				return;
			}
			else
			{
				startCraft();
			}
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

		closeUi();

		if (craft == null)
		{
			if (_recipeListUiPackedScene != null)
			{
				_recipeListUi = _recipeListUiPackedScene.Instantiate<RecetteList>();
				GetTree().Root.AddChild(_recipeListUi);
				_recipeListUi.initialize(this);
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
					return;
				}

				_factoryUi.initialize(this, inventoryUi);

				if (!inventoryUi.Visible)
				{
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
		}

		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public void closeUi()
	{
		if (_factoryUi != null)
		{
			_factoryUi.closeUi();
			_factoryUi = null;
		}

		if (_recipeListUi != null)
		{
			_recipeListUi.QueueFree();
			_recipeListUi = null;
		}

		var inventoryUi = GetNode<InventoryUi>("/root/Main/PlayerInventoryManager/InventoryUi");

		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}
}
