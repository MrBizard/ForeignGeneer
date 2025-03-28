using Godot;
using System;

public partial class filterManager : CanvasLayer
{
	public static filterManager instance;

	[Export]
	public ColorRect filter;
	public override void _Ready(){
		instance = this;
	}

	
	public void setFilter(int mode){
		
	}
}
