using System;
using Godot;
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class UiManager : Node
{
    public static UiManager instance { get; private set; }

    private Dictionary<string, PackedScene> uiScenes = new Dictionary<string, PackedScene>();

    [Export] public Godot.Collections.Array<string> sceneKeys { get; set; } = new Godot.Collections.Array<string>();
    [Export] public Godot.Collections.Array<PackedScene> uiScenesList { get; set; } = new Godot.Collections.Array<PackedScene>();

    public Node currentOpenUi = null;
    private string currentOpenUiId = null;

    public event Action<bool> onUiStateChanged;

    public override void _Ready()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            QueueFree();
        }

        for (int i = 0; i < sceneKeys.Count; i++)
        {
            if (i < uiScenesList.Count)
            {
                uiScenes.Add(sceneKeys[i], uiScenesList[i]);
            }
        }
    }

    /// <summary>
    /// Ouvre une interface utilisateur spécifique par son identifiant.
    /// </summary>
    /// <param name="uiId">Identifiant de l'UI à ouvrir.</param>
    /// <param name="data">Données optionnelles à passer à l'UI.</param>
    public void openUi(string uiId, Node data = null)
    {
        if (currentOpenUi != null)
        {
            closeUi();
        }

        if (uiScenes.TryGetValue(uiId, out var uiScene))
        {
            currentOpenUi = uiScene.Instantiate<Control>();
            AddChild(currentOpenUi);

            var baseUi = currentOpenUi as BaseUi;
            baseUi?.initialize(data);

            currentOpenUiId = uiId;

            Input.MouseMode = Input.MouseModeEnum.Visible;
            onUiStateChanged?.Invoke(true);
        }
        else
        {
            GD.PrintErr($"UI with id {uiId} not found.");
        }
    }

    /// <summary>
    /// Ferme l'interface utilisateur actuellement ouverte.
    /// </summary>
    public void closeUi()
    {
        if (currentOpenUi != null)
        {
            currentOpenUi.QueueFree();
            currentOpenUi = null;
            currentOpenUiId = null;

            Input.MouseMode = Input.MouseModeEnum.Captured;
            onUiStateChanged?.Invoke(false);
        }
    }

    /// <summary>
    /// Actualise l'interface utilisateur actuellement ouverte.
    /// </summary>
    /// <param name="data">Données optionnelles à passer à l'UI.</param>
    public void refreshCurrentUi(Node data = null)
    {
        if (currentOpenUi != null)
        {
            var baseUi = currentOpenUi as BaseUi;
            baseUi?.updateUi();
        }
        else
        {
            GD.Print("No UI is currently open to refresh.");
        }
    }

    /// <summary>
    /// Vérifie si une interface utilisateur spécifique est ouverte.
    /// </summary>
    /// <param name="uiId">Identifiant de l'UI à vérifier.</param>
    /// <returns>True si l'UI est ouverte, sinon False.</returns>
    public bool isUiOpen(string uiId)
    {
        return currentOpenUiId == uiId;
    }
    /// <summary>
    /// Vérifie si une interface utilisateur est actuellement ouverte.
    /// </summary>
    /// <returns>True si une UI est ouverte, sinon False.</returns>
    public bool IsAnyUiOpen()
    {
        return currentOpenUi != null;
    }
}