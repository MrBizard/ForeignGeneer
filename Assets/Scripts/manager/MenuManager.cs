using Godot;
using System;
using static System.Collections.Specialized.BitVector32;

///<tooltip>manage all buttons in menu </tooltip>
public partial class MenuManager : Control
{
	public override void _Ready()
	{
		GetNode<Button>("OptionsTabs/titlescreen").Visible = false;
	}
	///<tooltip>Fonction pour changer les scene</tooltip>
	 public void ChangeScene(string scenePath){
		// Load the new scene
		PackedScene newScene = ResourceLoader.Load<PackedScene>(scenePath);
		
		if (newScene != null){
			// Instantiate the new scene
			Node newSceneInstance = newScene.Instantiate();
			
			// Add the new scene to the root of the scene tree
			GetTree().Root.AddChild(newSceneInstance);
			
			// Optionally, you can free the current scene
			GetTree().CurrentScene.QueueFree();
		}
		else{
			GD.PrintErr("Failed to load scene: " + scenePath);
		}
	}

	///<tooltip>change scene to mainGame scene</tooltip>
	private void _onButtonStartDown(){
		ChangeScene("res://Assets/Scenes/main.tscn");
	}
	
	///<tooltip>change scene to mainGame scene</tooltip>
	private void _onNewGameButtonDown(){
		ChangeScene("res://Assets/Scenes/main.tscn");
	}
	
	///<tooltip>open the option settings</tooltip>
	private void _onOptionButtonDown(){
		GetNode<GridContainer>("OptionsTabs").Visible = true;	
	}
	///<tooltip>open the credit scene</tooltip>
	private void _onCreditButtonDown(){
		GD.Print("Credits opened");
		ChangeScene("res://Assets/Scenes/Menu/creditsScreen.tscn");
	}
	///<tooltip>close the game</tooltip>
	private void _onQuitButtonDown(){
		GetTree().Quit();
	}
}
