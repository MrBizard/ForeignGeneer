using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	[Export] public PackedScene option;
	private Control optionUI;
	private float globalElectricity = 0;
	public override void _Ready()
	{
		optionUI = option.Instantiate<Control>();
		AddChild(optionUI); 
		optionUI.Visible = false;
	}

	public float getGlobalElectricity()
	{
		return globalElectricity;
	}

	public void addGlobalElectricity(float value)
	{
		globalElectricity += value;
	}
	public void removeGlobalElectricity(float value)
	{
		globalElectricity -= value;
	}

	public bool hasEnergy(float value)
	{
		return globalElectricity >= value;
	}

}
