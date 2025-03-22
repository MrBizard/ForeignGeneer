using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface.Inventory;

public partial class OverlayItemInformation: Control,BaseUi
{
    [Export] Label _nameLabel = null!;
    [Export] Label _descriptionLabel = null!;
    [Export] TextureRect _icon = null!;
    StackItem _stackItem;
    public void initialize(object data)
    {
        if (data == null)  
            return;
        _stackItem = data as StackItem;
        _nameLabel.SetText(_stackItem?.getResource().GetName());
        _descriptionLabel.SetText(_stackItem?.getResource().getDescription());
        _icon.SetTexture(_stackItem.getResource().getInventoryIcon);
        Position = GetViewport().GetMousePosition();
    }

    public void updateUi()
    {
       
    }

    public void close()
    {
        
    }

    public void update(InterfaceType? interfaceType)
    {
        
    }
}