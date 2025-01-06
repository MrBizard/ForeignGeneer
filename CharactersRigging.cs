using Godot;
using System;

public partial class CharactersRigging : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float LerpVal = 0.5f;

	private Node3D Armature;
	private Node3D SpringArmPivot;
	private SpringArm3D SpringArm;
	private AnimationTree AnimTree;

	public override void _Ready()
	{
		Armature = GetNode<Node3D>("Armature");
		SpringArmPivot = GetNode<Node3D>("SpringArmPivot");
		SpringArm = GetNode<SpringArm3D>("SpringArmPivot/SpringArm3D");
		AnimTree = GetNode<AnimationTree>("AnimationTree");

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("quit"))
		{
			GetTree().Quit();
		}

		if (@event is InputEventMouseMotion mouseMotionEvent)
		{
			SpringArmPivot.RotateY(-mouseMotionEvent.Relative.X* 0.005f);
			SpringArm.RotateX(-mouseMotionEvent.Relative.Y * 0.005f);
			Vector3 springArmRotation = SpringArm.Rotation;
			springArmRotation.X = Mathf.Clamp(springArmRotation.X, -Mathf.Pi / 4, Mathf.Pi / 4);
			SpringArm.Rotation = springArmRotation;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Add gravity if not on the floor.
		if (!IsOnFloor())
		{
			Velocity += GetGravity();
		}

		// Get input direction and handle movement and deceleration.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		direction = direction.Rotated(Vector3.Up, SpringArmPivot.Rotation.Y);

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
}
