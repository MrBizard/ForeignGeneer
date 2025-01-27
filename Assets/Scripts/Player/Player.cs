using Godot;

public partial class Player : CharacterBody3D
{
    public static Player Instance { get; private set; }

    [Export] public float Speed = 5.0f; // Vitesse de base du joueur
    public const float LerpVal = 0.5f; // Valeur de lissage pour les mouvements
    private bool _isSprinting = false; // Indicateur de sprint

    // Références aux nœuds de la scène
    private Node3D _armature;
    private Node3D _pivot;
    private SpringArm3D _springArm;
    private AnimationTree _animTree;

    private Vector3 _movementDirection = Vector3.Zero; // Direction du mouvement

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

        // Initialisation des références aux nœuds
        _armature = GetNode<Node3D>("Armature_001");
        _pivot = GetNode<Node3D>("Pivot");
        _springArm = GetNode<SpringArm3D>("Pivot/SpringArm3D");
        _animTree = GetNode<AnimationTree>("AnimationTree");

        // Capture la souris pour les contrôles de la caméra
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Méthode pour activer/désactiver le sprint
    public void SetSprinting(bool isSprinting)
    {
        _isSprinting = isSprinting;
        Speed = isSprinting ? 50.0f : 5.0f;
    }

    // Méthode pour faire sauter le joueur
    public void Jump()
    {
        if (IsOnFloor())
        {
            Velocity = new Vector3(Velocity.X, 5.0f, Velocity.Z);
        }
    }

    // Méthode pour gérer le clic gauche
    public void LeftClick()
    {
        // Logique pour le clic gauche (à implémenter)
    }

    // Méthode pour gérer le clic droit
    public void RightClick()
    {
        // Logique pour le clic droit (à implémenter)
    }

    // Méthode pour gérer la rotation de la caméra
    public void RotateCamera(Vector2 mouseDelta)
    {
        _pivot.RotateY(-mouseDelta.X * 0.005f); // Rotation horizontale
        _springArm.RotateX(-mouseDelta.Y * 0.005f); // Rotation verticale

        // Limite la rotation verticale de la caméra
        Vector3 springArmRotation = _springArm.Rotation;
        springArmRotation.X = Mathf.Clamp(springArmRotation.X, -Mathf.Pi / 3, Mathf.Pi / 3);
        _springArm.Rotation = springArmRotation;
    }

    // Méthode pour gérer le mouvement
    public void Move(Vector2 inputDir)
    {
        _movementDirection = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        _movementDirection = _movementDirection.Rotated(Vector3.Up, _pivot.Rotation.Y);
    }

    // Méthode pour arrêter le mouvement
    public void StopMoving()
    {
        _movementDirection = Vector3.Zero;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Applique la gravité si le joueur n'est pas au sol
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * (float)delta;
        }

        // Applique le mouvement
        if (_movementDirection != Vector3.Zero)
        {
            Velocity = new Vector3(
                Mathf.Lerp(Velocity.X, _movementDirection.X * Speed, LerpVal),
                Velocity.Y,
                Mathf.Lerp(Velocity.Z, _movementDirection.Z * Speed, LerpVal)
            );

            // Calcule la rotation du personnage en fonction de la direction
            float targetRotationY = Mathf.Atan2(-Velocity.X, -Velocity.Z);
            _armature.Rotation = new Vector3(
                _armature.Rotation.X,
                Mathf.LerpAngle(_armature.Rotation.Y, targetRotationY, LerpVal),
                _armature.Rotation.Z
            );
        }
        else
        {
            // Arrête progressivement le mouvement
            Velocity = new Vector3(
                Mathf.Lerp(Velocity.X, 0.0f, LerpVal),
                Velocity.Y,
                Mathf.Lerp(Velocity.Z, 0.0f, LerpVal)
            );
        }

        // Met à jour l'animation du personnage
        if (_animTree != null)
        {
            float blendPosition = Velocity.Length() / Speed;
            _animTree.Set("parameters/BlendSpace1D/blend_position", blendPosition);
        }

        // Applique le mouvement
        MoveAndSlide();
    }
}