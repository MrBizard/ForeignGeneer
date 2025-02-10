using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.Inventory;

public partial class CurrentCursorItem :BaseUi
{
    public StackItem item { get; set; }
    
    public override void initialize(object data)
    {
        item = (StackItem)data;
        Sprite2D icon = GetNode<Sprite2D>("Icon");
        Label countLabel = GetNode<Label>("CountLabel");
        
        if (icon != null && countLabel != null)
        {
            icon.Texture = item.getResource().getInventoryIcon;
            countLabel.Text = item.getStack().ToString();
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (item != null)
        {
            updateUi();
        }
    }
    public override void updateUi()
    {
        Position = GetViewport().GetMousePosition();
    }

    public override void close()
    {
        InventoryManager.Instance.setCurrentItemInMouse(null);
    }
}