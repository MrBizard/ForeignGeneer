
using Godot;
using System.Collections.Generic;

public partial class Hotbar : Panel
{
	List<SlotUI> slots = new List<SlotUI>();
	public override void _Ready()
	{
		foreach(StackItem slot in InventoryManager.Instance.hotbar.slots)
		{
			
		}
	}
}
