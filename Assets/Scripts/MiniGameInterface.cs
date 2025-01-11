using Godot;

public partial class MinigameInterface : Control
{
	public override void _Ready()
	{
		GD.Print("Minigame");
	}

	public void CloseMinigame()
	{
		QueueFree(); //la fonction ferme l'interface. Elle la met dans une queue qui supprimera le node et ses enfants sans pour autant supprimer les objets qui la réferance
	}
}
