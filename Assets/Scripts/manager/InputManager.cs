using Godot;
using System;

public partial class InputManager : Node
{
    public static InputManager Instance { get; private set; }

    private Raycast raycast;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            raycast = GetNode<Raycast>("/root/Main/Personnage/Pivot/SpringArm3D/Camera3D/RayCast3D");
        }
        else
        {
            QueueFree();
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            HandleUiToggle("inventoryUi");
        }

        if (Input.IsActionJustPressed("option"))
        {
            HandleUiToggle("optionUi");
        }

        if (Input.IsActionJustPressed("sprint"))
        {
            Player.Instance.SetSprinting(true);
        }
        if (Input.IsActionJustReleased("sprint"))
        {
            Player.Instance.SetSprinting(false);
        }

        if (Input.IsActionJustPressed("jump"))
        {
            Player.Instance.Jump();
        }

        if (Input.IsActionJustPressed("leftClick"))
        {
            Player.Instance.LeftClick();
        }

        if (Input.IsActionJustPressed("rightClick"))
        {
            Player.Instance.RightClick();
        }

        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        if (inputDir != Vector2.Zero)
        {
            Player.Instance.Move(inputDir);
        }
        else
        {
            Player.Instance.StopMoving();
        }

        if (Input.IsActionJustPressed("interragir"))
        {
            HandleInteraction();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            Player.Instance.RotateCamera(mouseMotionEvent.Relative);
        }
    }

    private void HandleUiToggle(string uiName)
    {
        if (UiManager.instance.IsAnyUiOpen())
        {
            UiManager.instance.closeUi();
        }
        else
        {
            UiManager.instance.openUi(uiName);
        }
    }

    private void HandleInteraction()
    {
        if (raycast != null)
        {
            raycast.InteractWithObject();
        }
    }
}
