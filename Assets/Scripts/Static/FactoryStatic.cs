using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(FactoryStatic), "", nameof(ItemStatic))]
public partial class FactoryStatic : ItemStatic
{
    [Export] public int ElectricalCost { get; set; }
    [Export] public int PollutionIndex { get; set; }

    // Constructor: Pass the parameters to the base class constructor
public FactoryStatic(Texture2D inventoryIcon, int p_nbMaxStack) : base(inventoryIcon, p_nbMaxStack)
    {
        ElectricalCost = 100;
        PollutionIndex = 10;
    }

    // Override LeftClick with Factory-specific behavior
    public override void LeftClick()
    {
        GD.Print("Factory Left Clicked");
    }

    // Override RightClick with Factory-specific behavior
    public override void RightClick()
    {
        GD.Print("Factory Right Clicked");
    }
}
