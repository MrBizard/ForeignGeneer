using Godot;

public partial class MinigameInterface : Control
{
	public override void _Ready()
	{
		GD.Print("Minigame");
	}
	
	public void CloseMinigame()
	{
	//{tooltip="Close Minigame}	
		QueueFree();  
	}
}
