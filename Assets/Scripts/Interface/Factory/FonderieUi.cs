using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class FonderieUi : Control
{
	[Export] private PackedScene slotUi;

	private Fonderie fonderie;
	private List<SlotUI> inputSlots = new();
	private SlotUI outputSlot;
	private ProgressBar craftProgressBar;
	private VBoxContainer inputList;
	private VBoxContainer outputContainer;
	private HBoxContainer mainContainer;
	private PlayerInventoryManager playerInventoryManager;

	/// <summary>
	/// Initialise l'UI de la fonderie avec la fonderie donnée.
	/// </summary>
	/// <param name="fonderie">La fonderie associée à cette UI.</param>
	public void initialize(Fonderie fonderie, InventoryUi inventoryUi)
	{
		this.fonderie = fonderie;

		// Initialiser les slots d'entrée
		for (int i = 0; i < this.fonderie.input.slots.Count; i++)
		{
			var slot = slotUi.Instantiate<SlotUI>();
			slot.initialize(this.fonderie.input.slots[i], this.fonderie.input, playerInventoryManager);
			inputSlots.Add(slot);
			inputList.AddChild(slot);
		}

		// Initialiser le slot de sortie
		outputSlot = slotUi.Instantiate<SlotUI>();
		outputSlot.initialize(this.fonderie.output.slots[0], this.fonderie.output, playerInventoryManager, true); // true pour indiquer que c'est un slot de sortie
		outputContainer.AddChild(outputSlot);

		// Afficher l'inventaire à gauche de la fonderie
		if (inventoryUi != null)
		{
			inventoryUi.Visible = true;
			inventoryUi.Position = new Vector2(this.Position.X - inventoryUi.Size.X - 10, this.Position.Y);
		}

		// Activer le curseur
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	/// <summary>
	/// Met à jour l'interface utilisateur de la fonderie.
	/// </summary>
	public void updateUi()
	{
		// Mettre à jour les slots d'entrée
		for (int i = 0; i < inputSlots.Count; i++)
		{
			var slot = inputSlots[i];
			var stackItem = fonderie.input.getItem(i);
			slot.initialize(stackItem, fonderie.input, playerInventoryManager);
			slot.updateSlot();
		}

		// Mettre à jour le slot de sortie
		var outputStackItem = fonderie.output.getItem(0);
		if (outputStackItem != null)
		{
			GD.Print($"Mise à jour du slot de sortie avec : {outputStackItem.getResource().GetName()} (x{outputStackItem.getStack()})");
		}
		else
		{
			GD.Print("Le slot de sortie est vide.");
		}
		outputSlot.initialize(outputStackItem, fonderie.output, playerInventoryManager, true); // true pour indiquer que c'est un slot de sortie
		outputSlot.updateSlot();
	}

	public override void _Ready()
	{
		base._Ready();
		mainContainer = GetNode<HBoxContainer>("Container");
		inputList = GetNode<VBoxContainer>("Container/InputList");
		outputContainer = GetNode<VBoxContainer>("Container/OutputContainer");
		craftProgressBar = GetNode<ProgressBar>("Container/ProgressBar");
		playerInventoryManager = GetNode<PlayerInventoryManager>("/root/Main/PlayerInventoryManager");
	}

	/// <summary>
	/// Met à jour la barre de progression.
	/// </summary>
	/// <param name="progress">La progression actuelle (entre 0 et 1).</param>
	public void updateProgressBar(float progress)
	{
		if (craftProgressBar != null)
		{
			craftProgressBar.Value = progress * 100; // Convertit la progression en pourcentage
		}
	}

	public void closeUi()
	{
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		QueueFree();
	}
}
