using Godot;

/// <summary>
/// Représente une ressource statique pour un item dans le jeu.
/// Contient les propriétés générales et le comportement par défaut des items.
/// </summary>
public partial class ItemStatic : Resource
{
    [Export] private int _maxStack;
    [Export] public PackedScene _prefab;
    [Export] private Material _material;
    [Export] private Texture2D _inventoryIcon;

    /// <summary>
    /// Obtient ou définit le nombre maximal d'items pouvant être empilés.
    /// </summary>
    public int getMaxStack
    {
        get => _maxStack;
        set => _maxStack = value;
    }

    /// <summary>
    /// Obtient ou définit la scène de préfabriqué associée à cet item.
    /// </summary>
    public PackedScene getPrefab
    {
        get => _prefab;
        set => _prefab = value;
    }

    /// <summary>
    /// Obtient ou définit le matériau visuel associé à cet item.
    /// </summary>
    public Material getMaterial
    {
        get => _material;
        set => _material = value;
    }

    /// <summary>
    /// Obtient ou définit l'icône utilisée pour représenter l'item dans l'inventaire.
    /// </summary>
    public Texture2D getInventoryIcon
    {
        get => _inventoryIcon;
        set => _inventoryIcon = value;
    }

    /// <summary>
    /// Constructeur par défaut. Nécessaire pour l'initialisation via Godot.
    /// </summary>
    public ItemStatic() {}

    /// <summary>
    /// Initialise une nouvelle instance d'ItemStatic.
    /// </summary>
    /// <param name="inventoryIcon">Icône à afficher dans l'inventaire.</param>
    /// <param name="maxStack">Nombre maximal d'items pouvant être empilés.</param>
    public ItemStatic(Texture2D inventoryIcon, int maxStack)
    {
        _maxStack = maxStack;
        _inventoryIcon = inventoryIcon;
    }

      public StaticBody3D instantiate(Vector3 pos)
{
    // Instantiate the StaticBody3D from the prefab
    StaticBody3D itemInstantiate = _prefab.Instantiate<StaticBody3D>();

    // Check if a material is assigned
    if (_material != null)
    {
        // Find the first MeshInstance3D child and apply the material
        MeshInstance3D meshInstance = itemInstantiate.GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
        if (meshInstance != null)
        {
            meshInstance.MaterialOverride = _material;
        }
        else
        {
            GD.Print("No MeshInstance3D found in the prefab to apply the material.");
        }
    }

    // Set the global position of the instantiated item
    itemInstantiate.GlobalPosition = pos;

    // Return the instantiated StaticBody3D
    return itemInstantiate;
}

    /// <summary>
    /// Action par défaut déclenchée lors d'un clic gauche sur cet item.
    /// </summary>
    public virtual void LeftClick()
    {
        GD.Print("Left clicked on ItemStatic");
    }

    /// <summary>
    /// Action par défaut déclenchée lors d'un clic droit sur cet item.
    /// </summary>
    public virtual void RightClick()
    {
        GD.Print("Right clicked on ItemStatic");
    }
}

