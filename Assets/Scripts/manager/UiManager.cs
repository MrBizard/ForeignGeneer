using System;
using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class UiManager : Node
{
    public static UiManager instance { get; private set; }

    // Tableaux exportables pour les clés et les PackedScene
    [Export] private Godot.Collections.Array<string> uiKeys = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<PackedScene> uiScenes = new Godot.Collections.Array<PackedScene>();

    // Dictionnaire pour stocker les UI ouvertes (nom de la scène -> instance)
    private Dictionary<string, Control> _openUis = new Dictionary<string, Control>();

    // Dictionnaire pour stocker les scènes UI chargées (nom de la scène -> PackedScene)
    private Dictionary<string, PackedScene> _loadedUiScenes = new Dictionary<string, PackedScene>();

    public event Action<bool> onUiStateChanged;

    public override void _Ready()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            QueueFree(); // Assurez-vous qu'il n'y a qu'une seule instance
        }

        // Convertit les tableaux exportés en dictionnaire
        for (int i = 0; i < uiKeys.Count; i++)
        {
            if (i < uiScenes.Count)
            {
                _loadedUiScenes[uiKeys[i]] = uiScenes[i];
            }
        }
    }

    /// <summary>
    /// Ouvre une UI par son nom.
    /// </summary>
    public void openUi(string uiName, Object data = null)
    {
        GD.Print("Opening UI: " + uiName);

        if (_openUis.ContainsKey(uiName))
        {
            GD.Print($"UI {uiName} is already open.");
            return;
        }

        if (_loadedUiScenes.TryGetValue(uiName, out var uiScene))
        {
            var uiInstance = uiScene.Instantiate<Control>();
            AddChild(uiInstance);

            var baseUi = uiInstance as BaseUi;
            baseUi?.initialize(data);

            _openUis[uiName] = uiInstance;
            Input.MouseMode = Input.MouseModeEnum.Visible;
            onUiStateChanged?.Invoke(true);

            GD.Print("UI opened: " + uiName);
        }
        else
        {
            GD.PrintErr($"UI {uiName} is not registered.");
        }
    }

    /// <summary>
    /// Ferme une UI spécifique ou toutes les UI si aucun paramètre n'est fourni.
    /// </summary>
    public void closeUi(string uiName = null)
    {
        GD.Print("UI: " + _openUis.Count);

        if (string.IsNullOrEmpty(uiName))
        {
            // Ferme toutes les UI
            foreach (var uiEntry in _openUis)
            {
                uiEntry.Value.QueueFree();
            }
            _openUis.Clear();
            GD.Print("passe 3 ");
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else
        {
            // Ferme une UI spécifique
            if (_openUis.TryGetValue(uiName, out var uiInstance))
            {
                GD.Print("passe : " + uiName);
                uiInstance.QueueFree();
                _openUis.Remove(uiName);
                GD.Print("name : " + _openUis.Count);
                GD.Print("Remaining UIs: " + string.Join(", ", _openUis.Keys));

                if (_openUis.Count == 0)
                {
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                    onUiStateChanged?.Invoke(false);
                    return;
                }
            }
            else
            {
                GD.Print($"UI {uiName} is not open.");
            }
        }
        onUiStateChanged?.Invoke(false);
    }

    /// <summary>
    /// Vérifie si une UI est ouverte.
    /// </summary>
    public bool isUiOpen(string uiName)
    {
        return _openUis.ContainsKey(uiName);
    }

    /// <summary>
    /// Vérifie si au moins une UI est ouverte.
    /// </summary>
    public bool isAnyUiOpen()
    {
        return _openUis.Count > 0;
    }

    /// <summary>
    /// Récupère une UI ouverte par son nom.
    /// </summary>
    public Control getUi(string uiName)
    {
        if (_openUis.TryGetValue(uiName, out var uiInstance))
        {
            return uiInstance;
        }
        else
        {
            GD.Print($"UI {uiName} is not open.");
            return null;
        }
    }

    /// <summary>
    /// Actualise l'interface utilisateur actuellement ouverte.
    /// </summary>
    public void refreshCurrentUi(Node data = null)
    {
        // Si aucune UI n'est ouverte, on ne fait rien
        if (_openUis.Count == 0)
        {
            GD.Print("No UI is currently open to refresh.");
            return;
        }

        // Rafraîchit toutes les UI ouvertes
        foreach (var uiEntry in _openUis)
        {
            var baseUi = uiEntry.Value as BaseUi;
            baseUi?.updateUi();
        }
    }
}