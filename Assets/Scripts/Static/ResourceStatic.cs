using Godot;
using MonoCustomResourceRegistry;

public partial class ResourceStatic : ItemStatic
{
    public ResourceStatic(){}

    public ResourceType ResourceType { get; set; }

    public override void LeftClick()
    {
        GD.Print("Resource leftclick");
    }

    public override void RightClick(Player player)
    {
        GD.Print("Resource rightclick");
    }
}