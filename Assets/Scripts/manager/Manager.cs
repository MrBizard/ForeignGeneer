using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	[Export] public PackedScene option;
	private Control optionUI;
	
	public override void _Ready()
	{
		optionUI = option.Instantiate<Control>();
		AddChild(optionUI); 
		optionUI.Visible = false;
	}

	

}
