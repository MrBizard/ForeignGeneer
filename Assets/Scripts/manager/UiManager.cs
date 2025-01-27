using Godot;

public partial class UiManager : Node
{
    public static UiManager Instance { get; private set; }

    private Control _currentOpenUI = null;

    [Export] private PackedScene _inventoryUIScene;

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

    /// <summary>
    /// Ouvre ou ferme l'UI de l'inventaire.
    /// </summary>
    public void ToggleInventoryUI()
    {
        if (_currentOpenUI != null)
        {
            CloseCurrentUI();
        }
        else
        {
            OpenInventoryUI();
        }
    }

    /// <summary>
    /// Ouvre l'UI de l'inventaire.
    /// </summary>
    private void OpenInventoryUI()
    {
        _currentOpenUI = _inventoryUIScene.Instantiate<Control>();
        AddChild(_currentOpenUI);

        // Mettre à jour l'UI de l'inventaire
        var inventoryUI = _currentOpenUI as InventoryUi;
        inventoryUI?.UpdateUi();

        // Configurer la souris
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    /// <summary>
    /// Ferme l'UI actuelle.
    /// </summary>
    public void CloseCurrentUI()
    {
        if (_currentOpenUI != null)
        {
            _currentOpenUI.QueueFree();
            _currentOpenUI = null;

            // Configurer la souris
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}