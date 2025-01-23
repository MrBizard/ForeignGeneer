using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public float Speed = 5.0f;
	public const float LerpVal = 0.5f;
	private bool b_IsSprinting = false;
	public Inventory inv;
	[Export] private Node3D Map;
	
	private Node3D Armature;
	private Node3D Pivot;
	private SpringArm3D SpringArm;
	private AnimationTree AnimTree;

	//main et l'objet a attaché
	[Export] private Node3D handNode;
    [Export] private PackedScene objectToAttach;

	StackItem currentItemInHand=null;
    
	

	public override void _Ready()
	{
		inv = new Inventory(28);
		Armature = GetNode<Node3D>("Armature");
		Pivot = GetNode<Node3D>("Pivot");
		SpringArm = GetNode<SpringArm3D>("Pivot/SpringArm3D");
		AnimTree = GetNode<AnimationTree>("AnimationTree");

		Input.MouseMode = Input.MouseModeEnum.Captured;
		

		if (handNode == null)
        {
            GD.PrintErr("node de la main non assigné (script Player)");
        }
	}

	public override void _UnhandledInput(InputEvent @event)
	{

		if (Input.IsKeyPressed(Key.Shift))
		{
			Speed = 50.0f;
		}
		else
		{

			Speed = 5.0f;
		}
		if (Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}
		if (IsOnFloor() && Input.IsActionJustPressed("Jump"))
		{
			//saut
			Velocity = new Vector3(Velocity.X, 5.0f, Velocity.Z);
		}
		if (@event is InputEventMouseMotion mouseMotionEvent)
		{
			Pivot.RotateY(-mouseMotionEvent.Relative.X* 0.005f);
			SpringArm.RotateX(-mouseMotionEvent.Relative.Y * 0.005f);
			Vector3 springArmRotation = SpringArm.Rotation;
			springArmRotation.X = Mathf.Clamp(springArmRotation.X, -Mathf.Pi / 3, Mathf.Pi / 3);
			SpringArm.Rotation = springArmRotation;
		}
	}


	// public void ShowInventory(){
	// 	var TypeTempInv = GetNode("../Inventory Manager");
	// 		GD.Print(TypeTempInv);
	// 	var TempInv = GetNode<Control>("../Inventory Manager");
		

	// 	if (TempInv != null && TempInv.Visible){
	// 		TempInv.Show();
	// 	}
	// 	else
	// 	{
	// 		TempInv.Hide();
	// 	}
		

	// }
	public override void _PhysicsProcess(double delta)
	{	

		// Add gravity if not on the floor.
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}

		
		// Get input direction and handle movement and deceleration.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		direction = direction.Rotated(Vector3.Up, Pivot.Rotation.Y);

		if (direction != Vector3.Zero)
		{
			Velocity = new Vector3(
				Mathf.Lerp(Velocity.X, direction.X * Speed, LerpVal),
				Velocity.Y,
				Mathf.Lerp(Velocity.Z, direction.Z * Speed, LerpVal)
			);

			float targetRotationY = Mathf.Atan2(-Velocity.X, -Velocity.Z);
			Armature.Rotation = new Vector3(
				Armature.Rotation.X,
				Mathf.LerpAngle(Armature.Rotation.Y, targetRotationY, LerpVal),
				Armature.Rotation.Z
			);
		}
		else
		{
			Velocity = new Vector3(
				Mathf.Lerp(Velocity.X, 0.0f, LerpVal),
				Velocity.Y,
				Mathf.Lerp(Velocity.Z, 0.0f, LerpVal)
			);
		}

		AnimTree.Set("parameters/BlendSpace1D/blend_position", Velocity.Length() / Speed);
		MoveAndSlide();
	}


	// Permet d'attacher et de faire suivre l'objet actuellement selectionée dans la main 
	//(necessite un personnage avec une main détaché du reste du corps
	//	pour faire suivre l'objet avec l'animation)

	/*public void AttachObjectToHand()
    {
        if (handNode == null || objectToAttach == null)
        {
            GD.PrintErr("Hand node or object to attach is not assigned!");
            return;
        }

        var objectInstance = (Node3D)objectToAttach.Instantiate();
        handNode.AddChild(objectInstance);

        // Set object's transform (adjust as needed)
        objectInstance.Translation = new Vector3(0, 0, 0); // Adjust for the hand position
        objectInstance.RotationDegrees = new Vector3(0, 90, 0); // Adjust for orientation
    }*/
}
