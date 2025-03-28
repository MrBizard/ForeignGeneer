using ForeignGeneer.Assets.Scripts;
using ForeignGeneer.Assets.Scripts.Interface;
using Godot;

public partial class HotbarSlotUi: Panel,BaseUi
{
    [Export] private TextureRect _textureRect;
    private int _hotbarIndex;
    public void update(InterfaceType? interfaceType = null)
    {
       updateUi();
    }

    private void updateUi()
    {
        StackItem _stackItem = InventoryManager.Instance.hotbar.slots[_hotbarIndex];
        if(_stackItem != null)
            _textureRect.Texture = _stackItem.getResource().getInventoryIcon;
        else
            _textureRect.Texture = null;
        GD.Print(_textureRect.Texture);
    }
    public void detach()
    {
        
    }

    public void initialize(object data)
    {
        _hotbarIndex = (int)data;
        Inventory inv = InventoryManager.Instance.hotbar;
        inv.attach(_hotbarIndex, this);
        updateUi();
    }
}