using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class UiManager : Node
{
    public static UiManager Instance { get; private set; }

    // Dictionnaire pour stocker les scènes d'UI par clé
    private Dictionary<string, PackedScene> _uiScenes = new Dictionary<string, PackedScene>();

    // Tableaux exportables pour les clés et les scènes
    [Export] public Godot.Collections.Array<string> SceneKeys { get; set; } = new Godot.Collections.Array<string>();
    [Export] public Godot.Collections.Array<PackedScene> UIScenes { get; set; } = new Godot.Collections.Array<PackedScene>();

    private Node _currentOpenUI = null; // UI actuellement ouverte
    private string _currentOpenUIId = null; // ID de la scène ouverte

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree(); // Assurer qu'il n'y a qu'une seule instance
        }

        // Associer chaque clé à une scène
        for (int i = 0; i < SceneKeys.Count; i++)
        {
            if (i < UIScenes.Count)
            {
                _uiScenes.Add(SceneKeys[i], UIScenes[i]);
            }
        }
    }

    /// <summary>
    /// Ouvre une UI spécifique par son identifiant.
    /// </summary>
    public void OpenUI(string uiId, Node data = null)
    {
        if (_currentOpenUI != null)
        {
            CloseUI(); // Ferme l'UI précédente
        }

        // Vérifie si la clé existe dans le dictionnaire des scènes
        if (_uiScenes.TryGetValue(uiId, out var uiScene))
        {
            _currentOpenUI = uiScene.Instantiate<Control>();  // Instancier la scène
            AddChild(_currentOpenUI);  // Ajouter la scène à la hiérarchie

            var baseUi = _currentOpenUI as BaseUi;
            baseUi?.initialize(data);  // Initialiser si nécessaire

            _currentOpenUIId = uiId; // Suivi de la scène actuellement ouverte

            // Afficher la souris
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            GD.PrintErr($"UI with id {uiId} not found.");
        }
    }

    /// <summary>
    /// Ferme la UI actuelle.
    /// </summary>
    public void CloseUI()
    {
        if (_currentOpenUI != null)
        {
            _currentOpenUI.QueueFree(); // Retirer la scène
            _currentOpenUI = null;
            _currentOpenUIId = null; // Réinitialiser l'ID de l'UI ouverte

            // Cacher la souris
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }

    /// <summary>
    /// Vérifie si une UI spécifique est ouverte.
    /// </summary>
    public bool IsUIOpen(string uiId)
    {
        return _currentOpenUIId == uiId;
    }
}
