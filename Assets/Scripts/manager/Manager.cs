using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	[Export] public PackedScene option;
	private Control optionUI;
	private float globalElectricity;
	public override void _Ready()
	{
		optionUI = option.Instantiate<Control>();
		AddChild(optionUI); 
		optionUI.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("option"))
		{
			GD.Print(optionUI);
			optionUI.Visible = !(optionUI.Visible);
			if (optionUI.Visible) 
				Input.MouseMode = Input.MouseModeEnum.Visible;
			else
				Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}

	public float getGlobalElectricity()
	{
		return globalElectricity;
	}

	public void setGlobalElectricity(float value)
	{
		globalElectricity += value;
	}


}
