using Godot;

public partial class ItemStatic : Resource
{
    [Export] private int _maxStack;
    [Export] public PackedScene _prefab;
    [Export] private Material _material;
    [Export] private Texture2D _inventoryIcon;

    public int getMaxStack
    {
        get => _maxStack;
        set => _maxStack = value;
    }

    public PackedScene getPrefab
    {
        get => _prefab;
        set => _prefab = value;
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

    public ItemAuSol instantiate(Vector3 pos)
    {
        ItemAuSol itemInstantiate = _prefab.Instantiate<ItemAuSol>();

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
}