using System;
using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Manager : Node
{
	public static Manager instance;
	
	[Export] public PackedScene option;
	private Control optionUI;
	
	public override void _Ready()
	{
		if(instance == null)
			instance = this;
		optionUI = option.Instantiate<Control>();
		AddChild(optionUI); 
		optionUI.Visible = false;
	}

	
}
