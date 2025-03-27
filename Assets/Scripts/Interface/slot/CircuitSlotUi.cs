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
        SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
        SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }

    public void initialize(Circuit circuit, int index, bool output = false)
    {
        this.circuit = circuit;
        slotIndex = index;
        isOutput = output;
        updateUi();
        adjustSize();
    }

    private void adjustSize()
    {
        int totalItems = circuit.currentRecipe.input[slotIndex].Stack;
        Vector2 newSize;
        switch (totalItems)
        {
            case 2:
                newSize = new Vector2(80, 80);
                break;
            case 3:
                newSize = new Vector2(110,110);
                break;
            default:
                newSize = new Vector2(64, 64);
                break;
        }

        CustomMinimumSize = newSize;
        SetDeferred("size", newSize);
        PivotOffset = newSize / 2;
    }
    private void updateUi()
    {
        var item = circuit.inputInventory.getItem(slotIndex);
        var recipeItem = circuit.currentRecipe.input[slotIndex];
        
        icon.Texture = item?.getResource().getInventoryIcon;
        background.Texture = recipeItem.getResource().getInventoryIcon;
        countLabel.Text = $"{item?.Stack ?? 0} / {recipeItem.Stack}";
        updateItemProgress();
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
                int maxAllowed = circuit.currentRecipe.input[slotIndex].Stack;

                if (stackItem == null)
                {
                    if (InventoryManager.Instance.currentItemInMouse.getStack() > maxAllowed)
                    {
                        var excess = InventoryManager.Instance.currentItemInMouse.getStack() - maxAllowed;
                        InventoryManager.Instance.currentItemInMouse.subtract(maxAllowed);
                        circuit.inputInventory.slots[slotIndex] = new StackItem(InventoryManager.Instance.currentItemInMouse.getResource(),maxAllowed);
                    }
                    else
                    {
                        circuit.inputInventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, slotIndex);
                        InventoryManager.Instance.setCurrentItemInMouse(null);
                    }
                }
                else if (stackItem.getResource() == InventoryManager.Instance.currentItemInMouse.getResource())
                {
                    int total = stackItem.getStack() + InventoryManager.Instance.currentItemInMouse.getStack();
                    if (total > maxAllowed)
                    {
                        int excess = total - maxAllowed;
                        stackItem.setStack(maxAllowed);
                        InventoryManager.Instance.currentItemInMouse.setStack(excess);
                    }
                    else
                    {
                        stackItem.setStack(total);
                        InventoryManager.Instance.setCurrentItemInMouse(null);
                    }
                }

                circuit.inputInventory.notifyInventoryUpdated();
            }
        }
        updateUi();
    }

    private void handleRightClick()
    {
        var stackItem = circuit.inputInventory.getItem(slotIndex);
        if (stackItem != null && stackItem.getStack() > 0)
        {
            StackItem splitItem = stackItem.split(); // Divise par 2
            
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

    public void updateItemProgress()
    {
        StackItem stackItem = circuit.inputInventory.getItem(slotIndex);
        int maxAllowed = circuit.currentRecipe.input[slotIndex].Stack;

        if (stackItem != null)
        {
            float progress = Mathf.Clamp((float)stackItem.Stack / maxAllowed, 0.3f, 1.0f);
            icon.Scale = new Vector2(progress, progress);
            icon.PivotOffset = icon.Size / 2;
            background.Visible = stackItem.Stack == 0;
        }
        
    }
}
