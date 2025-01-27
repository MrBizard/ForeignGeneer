using Godot;

public partial class InputManager : Node
{
    public static InputManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree(); // Assure qu'il n'y a qu'une seule instance
        }
    }

    public override void _Process(double delta)
    {
        // Gestion de l'inventaire
        if (Input.IsActionJustPressed("inventory"))
        {
            if (!UiManager.instance.isUiOpen("inventoryUi"))
            {
                UiManager.instance.openUi("inventoryUi");
            }
            else
            {
                UiManager.instance.closeUi();
            }
        }

        if (Input.IsActionJustPressed("option"))
        {
            if (!UiManager.instance.isUiOpen("optionUi"))
            {
                UiManager.instance.openUi("optionUi");
            }
            else
            {
                UiManager.instance.closeUi();
            }
        }
        // Gestion du sprint
        if (Input.IsActionJustPressed("sprint"))
        {
            Player.Instance.SetSprinting(true);
        }
        if (Input.IsActionJustReleased("sprint"))
        {
            Player.Instance.SetSprinting(false);
        }

        // Gestion du saut
        if (Input.IsActionJustPressed("jump"))
        {
            Player.Instance.Jump();
        }

        // Gestion des clics
        if (Input.IsActionJustPressed("leftClick"))
        {
            Player.Instance.LeftClick();
        }

        if (Input.IsActionJustPressed("rightClick"))
        {
            Player.Instance.RightClick();
        }

        // Gestion du mouvement
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        if (inputDir != Vector2.Zero)
        {
            Player.Instance.Move(inputDir);
        }
        else
        {
            Player.Instance.StopMoving();
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Gestion de la rotation de la caméra
        if (@event is InputEventMouseMotion mouseMotionEvent)
        {
            Player.Instance.RotateCamera(mouseMotionEvent.Relative);
        }
    }
}