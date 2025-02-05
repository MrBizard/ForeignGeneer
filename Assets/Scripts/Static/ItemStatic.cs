using Godot;

public partial class ItemStatic : Resource
{
    [Export] private int _maxStack;
    [Export] public string _scenePath;
    [Export] private Material _material;
    [Export] private Texture2D _inventoryIcon;

    public int getMaxStack
    {
        get => _maxStack;
        set => _maxStack = value;
    }

    public string getPrefab
    {
        get => _scenePath;
        set => _scenePath = value;
    }

    public Material getMaterial
    {
        get => _material;
        set => _material = value;
    }

    public Texture2D getInventoryIcon
    {
        get => _inventoryIcon;
        set => _inventoryIcon = value;
    }

    public ItemStatic() {}

    public ItemStatic(Texture2D inventoryIcon, int maxStack)
    {
        _maxStack = maxStack;
        _inventoryIcon = inventoryIcon;
    }

    public ItemAuSol Instantiate(Vector3 pos)
    {
        if (string.IsNullOrEmpty(_scenePath))
        {
            GD.PrintErr("scenePath is not set for this item.");
            return null;
        }

        PackedScene scene = GD.Load<PackedScene>(_scenePath);
        if (scene == null)
        {
            GD.PrintErr($"Failed to load scene at path: {_scenePath}");
            return null;
        }

        ItemAuSol itemInstantiate = scene.Instantiate<ItemAuSol>();
        if (itemInstantiate == null)
        {
            GD.PrintErr("Failed to instantiate ItemAuSol.");
            return null;
        }

        // Appliquer le matériau si disponible
        if (_material != null)
        {
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

        itemInstantiate.GlobalPosition = pos;
        return itemInstantiate;
    }


    public virtual void LeftClick()
    {
        GD.Print("Left clicked on ItemStatic");
    }

    public virtual void RightClick(Player player)
    {
        GD.Print("Right clicked on ItemStatic");
    }

    public override string ToString()
    {
        return " name : " + GetName() + "\n scenePath : " + _scenePath + "\n inventoryIcon : " 
               + _inventoryIcon.ToString() + "\n maxStack : " + _maxStack + "\n material : " + _material.ToString();
    }
}