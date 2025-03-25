using Godot;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;
using ForeignGeneer.Assets.Scripts.manager;
using ForeignGeneer.Assets.Scripts.Static.Craft;

public partial class CentralInput : StaticBody3D, IInputFactory<CraftingFactoryStatic>
{
	[Export] public int inputSlotCount { get; set; } = 2;
	[Export] public CraftingFactoryStatic factoryStatic { get; set; }
	[Export] public string factoryUiName { get; set; }
	[Export] public string recipeUiName { get; set; }
	public Inventory input { get; set; }
	public Craft craft { get; set; }

	private CentralUi _centralUi;
	private Timer _craftTimer;

	public override void _Ready()
	{
		base._Ready();
		input = new Inventory(inputSlotCount);
		input.onInventoryUpdated += onInventoryUpdated;
		factoryStatic.recipeList?.init();
		PollutionManager.instance.addPolution(0);

		_craftTimer = new Timer();
		_craftTimer.Name = "CraftTimer";
		AddChild(_craftTimer);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (craft != null && craft.isCrafting)
		{
			craft.updateCraftProgress(delta);
			updateProgressBar();
		}
	}

	private void onInventoryUpdated()
	{
		if (craft != null && craft.canContinue())
		{
			startCraft();
		}

		updateUi();
	}

	public void setCraft(Recipe recipe)
	{
		if (recipe == null)
		{
			craft = null;
			return;
		}

		craft = new Craft(recipe, input);
		craft.craftTimer = _craftTimer;
		craft.startCraft(onCraftFinished);
		openUi();
	}

	private void startCraft()
	{
		if (EnergyManager.instance.isDown() || craft.isCrafting)
		{
			return;
		}
		if(craft.startCraft(onCraftFinished))
			EnergyManager.instance.addGlobalElectricity(factoryStatic.electricalCost);
		
		updateProgressBar();
		if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
		{
			_centralUi.updateElectricity();
		}
	}

	private void onCraftFinished()
	{
		if (craft != null)
		{
			craft.stopCraft();
			EnergyManager.instance.removeGlobalElectricity(factoryStatic.electricalCost);
			if (craft.canContinue())
			{
				startCraft();
			}
		}
		updateUi();
	}

	public void openUi()
	{
		closeUi();
		if (craft == null)
		{
			UiManager.instance.openUi(recipeUiName, this);
			_centralUi = null;
		}
		else
		{
			UiManager.instance.openUi(factoryUiName, this);
			_centralUi = (CentralUi)UiManager.instance.getUi(factoryUiName);
		}
	}

	public void closeUi()
	{
		_centralUi = null;
		UiManager.instance.closeUi();
	}

	public void dismantle()
	{
		craft?.stopCraft();
		input.onInventoryUpdated -= onInventoryUpdated;
		QueueFree();
	}

	private void updateUi()
	{
		if (UiManager.instance.isAnyUiOpen())
		{
			UiManager.instance.refreshCurrentUi(this);
		}
	}
	private void updateProgressBar()
	{
		if (_centralUi != null && UiManager.instance.isUiOpen(factoryUiName))
		{
			_centralUi.updateProgressBar(craft.craftProgress);
		}
	}
}
