using Godot;
using System;
using System.Diagnostics;

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
        if(UiManager.instance.isAnyUiOpen())
            return;
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
            if(UiManager.instance.isAnyUiOpen())
                InventoryManager.Instance.drop();
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

        if (Input.IsActionJustPressed("camera"))
        {
            Player.Instance.ToggleViewMode();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            Player.Instance.RotateCamera(mouseMotionEvent.Relative);
        }

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if(mouseButtonEvent.ButtonIndex == MouseButton.WheelUp && mouseButtonEvent.Pressed)
                InventoryManager.Instance.addCurrentItemToHotbar();
            else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown && mouseButtonEvent.Pressed)
                InventoryManager.Instance.removeCurrentItemToHotbar();
        }
    }
    
    private void HandleUiToggle(string uiName)
    {
        if (UiManager.instance.isAnyUiOpen())
        {
            UiManager.instance.closeUi();
            InventoryManager.Instance.drop();
        }
        else
        {
            UiManager.instance.openUi(uiName);
            InventoryManager.Instance.StopPreview();
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
