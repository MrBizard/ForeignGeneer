using Godot;

public partial class Manager : Node
{
	[Export] public ItemStatic item;
	[Export] public Node ingot;
	

	private StaticBody3D _newIngot;
	public override void _Ready()
	{
		_newIngot = null;
	}
	
	public override void _Process(double delta)
	{
		
		if (item != null && Input.IsActionJustPressed("T"))
		{
			GD.Print("ca marche");
			

			if (item.getPrefab.CanInstantiate())
			{
				_newIngot = item.getPrefab.Instantiate<StaticBody3D>();
				_newIngot.Name = "ingotIron";
				Node3D last = null;
				if (ingot.GetChildCount() > 0)
					last = (Node3D)ingot.GetChild(-1);
				if (last != null)
				{
					_newIngot.Position = last.Position + new Vector3(1, 0, 0);
				}
				else
				{
					_newIngot.Position = new Vector3(0, 2, 0);

				}
				ingot.AddChild(_newIngot);
			}
		}
	}
}
