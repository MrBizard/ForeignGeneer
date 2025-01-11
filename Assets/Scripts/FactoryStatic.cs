using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class FactoryStatic : ItemStatic
{
    [Export] public int ElectricalCost { get; set; }
    [Export] public int PollutionIndex { get; set; }

    // Constructor
    public FactoryStatic(Texture2D inventoryIcon) : base(inventoryIcon)
    {
        ElectricalCost = 100; 
        PollutionIndex = 10;  
    }

    public override void LeftClick()
    {
        GD.Print("factory left click");
    }

    public override void RightClick()
    {
        GD.Print("factory right click");
        
    }
}
