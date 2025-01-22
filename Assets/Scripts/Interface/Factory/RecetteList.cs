using Godot;
using System;
using ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public partial class RecetteList : Control
{
    [Export] private PackedScene recipeUiPacked;
    private GridContainer gridContainer;
    private ScrollContainer scrollContainer;

    public override void _Ready()
    {
        // Récupérer le ScrollContainer et le GridContainer
        scrollContainer = GetNode<ScrollContainer>("VScroll");
        gridContainer = GetNode<GridContainer>("VScroll/GridContainer");

        // Vérifier que les nœuds sont correctement initialisés
        if (scrollContainer == null)
        {
            GD.PrintErr("ScrollContainer n'est pas initialisé.");
            return;
        }

        if (gridContainer == null)
        {
            GD.PrintErr("GridContainer n'est pas initialisé.");
            return;
        }
    }

    public void initialize(IFactory factory)
    {
        // Vérifier que gridContainer est correctement initialisé
        if (gridContainer == null)
        {
            GD.PrintErr("GridContainer n'est pas initialisé.");
            return;
        }

        // Parcourir chaque recette dans la liste
        GD.Print(factory.recipeList.Count);
        foreach (Recipe recipe in factory.recipeList)
        {
            // Instancier l'interface utilisateur de la recette
            var recipeUi = recipeUiPacked.Instantiate();

            // Récupérer les nœuds enfants de recipeUi
            Label nameItem = recipeUi.GetNode<Label>("NameItem");
            TextureRect textureItem = recipeUi.GetNode<TextureRect>("TextureItem");

            // Vérifier que les nœuds existent
            if (nameItem == null)
            {
                GD.PrintErr("Le nœud 'NameItem' est introuvable dans recipeUi.");
                continue; // Passer à la prochaine itération
            }

            if (textureItem == null)
            {
                GD.PrintErr("Le nœud 'TextureItem' est introuvable dans recipeUi.");
                continue; // Passer à la prochaine itération
            }

            // Mettre à jour les propriétés des nœuds avec les données de la recette
            nameItem.Text = recipe.GetName();
            textureItem.Texture = recipe.output.getResource().getInventoryIcon;

            // Ajouter l'objet au GridContainer
            gridContainer.AddChild(recipeUi);
            GD.PrintT("pass");
        }
    }

    public override void _Process(double delta)
    {
    }
}