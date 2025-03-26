using ForeignGeneer.Assets.Scripts;
using Godot;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class CircuitSlotUi : Control, BaseUi
{
    [Export] public TextureRect icon;
    [Export] public TextureRect background;
    [Export] private Label countLabel;
    
    private Circuit circuit;
    private int slotIndex;
    private bool isOutput;
    
    public override void _Ready()
    {
        base._Ready();
        CustomMinimumSize = new Vector2(64, 64);
        SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    public void initialize(Circuit circuit, int index, bool output = false)
    {
        this.circuit = circuit;
        this.slotIndex = index;
        this.isOutput = output;
        
        updateUi();
    }
    private void updateUi()
    {
        var item = circuit.inputInventory.getItem(slotIndex);
        var recipeItem = circuit.currentRecipe.input[slotIndex];
        
        icon.Texture = item?.getResource().getInventoryIcon;
        background.Texture = circuit.currentRecipe.input[slotIndex].getResource().getInventoryIcon;
        countLabel.Text = $"{item?.Stack ?? 0} / {recipeItem.Stack}";
        
    }
    public void update(InterfaceType? interfaceType = null) => updateUi();
    public void detach() => circuit.inputInventory.detach(slotIndex, this);
    public void initialize(object data) { }
    
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                handleLeftClick();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                handleRightClick();
            }
        }
    }

    private void handleLeftClick()
{
    if (InventoryManager.Instance.currentItemInMouse == null)
    {
        var stackItem = circuit.inputInventory.getItem(slotIndex);
        if (stackItem != null)
        {
            // Prendre l'item du slot
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            circuit.inputInventory.deleteItem(slotIndex);
            circuit.inputInventory.notifyInventoryUpdated();
        }
    }
    else
    {
        if (!isOutput)
        {
            var stackItem = circuit.inputInventory.getItem(slotIndex);
            if (stackItem == null)
            {
                circuit.inputInventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, slotIndex);
                InventoryManager.Instance.setCurrentItemInMouse(null);
            }
            else if (stackItem.getResource() == InventoryManager.Instance.currentItemInMouse.getResource())
            {
                InventoryManager.Instance.currentItemInMouse.setStack(stackItem.add(InventoryManager.Instance.currentItemInMouse.getStack()));
                if (InventoryManager.Instance.currentItemInMouse.getStack() <= 0)
                {
                    InventoryManager.Instance.setCurrentItemInMouse(null);
                }
            }
            else
            {
                var temp = stackItem;
                circuit.inputInventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, slotIndex);
                InventoryManager.Instance.setCurrentItemInMouse(temp);
            }
            circuit.inputInventory.notifyInventoryUpdated();
        }
    }
    updateUi();
    InventoryManager.Instance.setCurrentItemInMouse(InventoryManager.Instance.currentItemInMouse);
}


    private void handleRightClick()
    {
        var stackItem = circuit.inputInventory.getItem(slotIndex);
        if (stackItem != null && stackItem.getStack() > 0)
        {
            StackItem splitItem = stackItem.split();
            
            StackItem currentMouseItem = InventoryManager.Instance.currentItemInMouse;
            if (currentMouseItem == null)
            {
                InventoryManager.Instance.setCurrentItemInMouse(splitItem);
            }
            else if (currentMouseItem.getResource() == splitItem.getResource())
            {
                int excess = currentMouseItem.add(splitItem.getStack()); 

                if (excess > 0)
                {
                    stackItem.setStack(stackItem.getStack() + excess);
                }

                InventoryManager.Instance.setCurrentItemInMouse(currentMouseItem);
            } 

            circuit.inputInventory.notifyInventoryUpdated();
            updateUi();
        }
        else if (stackItem != null && stackItem.getStack() == 1)
        {
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            circuit.inputInventory.deleteItem(slotIndex);
            circuit.inputInventory.notifyInventoryUpdated();
            updateUi();
        }
    }
    public void UpdateItemProgress()
    {
        StackItem stackItem = circuit.inputInventory.getItem(slotIndex);
        float progress = (float)stackItem.Stack / circuit.currentRecipe.input[slotIndex].Stack;
        icon.Scale = new Vector2(progress, 1);
    
        if (stackItem.Stack > 0)
        {
            background.Visible = false;
        }
    }
}