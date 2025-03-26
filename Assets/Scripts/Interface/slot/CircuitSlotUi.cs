using Godot;
using System;
using ForeignGeneer.Assets.Scripts;
using ForeignGeneer.Assets.Scripts.Interface;

public partial class CircuitSlotUi : Control, BaseUi
{
	[Export]private TextureRect _textureRect;
	[Export]private TextureRect _textureBackgroundRect;
	[Export]private Label _labelCount;
	private StackItem _stackItem;
	private Circuit _circuit;
	private int _index = 0;
	private bool _isOutputSlot = false;
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void update(InterfaceType? interfaceType = null)
	{
		switch (interfaceType)
		{
			default:
				updateUi();
				break;
		}
	}

	public void detach()
	{
		_circuit.inputInventory.detach(_index,this);
	}

	private void updateUi()
	{
		setLabelCount();
		_textureRect.SetTexture(_stackItem?.getResource().getInventoryIcon);
	}
	private void setLabelCount()
	{
		if (_stackItem != null)
			_labelCount.Text = _stackItem.Stack + " / " + _circuit.currentRecipe.input[_index].Stack;
		else
			_labelCount.Text =  "0 / " + _circuit.currentRecipe.input[_index].Stack;
	}
	public void initialize(object data)
	{
	}

	public void initialize(Circuit circuit, int index, bool outputSlot = false)
	{
		_circuit = circuit;
		_index = index;
		_isOutputSlot = outputSlot;
		_stackItem = _circuit.inputInventory.getItem(index);
		_textureBackgroundRect.SetTexture(_circuit.currentRecipe.input[_index]?.getResource().getInventoryIcon);
		_circuit.inputInventory.attach(index,this);
		updateUi();
	}
	
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
        var stackItem = _circuit.inputInventory.getItem(_index);
        if (stackItem != null)
        {
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            _circuit.inputInventory.deleteItem(_index);
        }
    }
    else
    {
        if (!_isOutputSlot)
        {
            var stackItem = _circuit.inputInventory.getItem(_index);
            if (stackItem == null)
            {
	            _circuit.inputInventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, _index);
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
                _circuit.inputInventory.addItemToSlot(InventoryManager.Instance.currentItemInMouse, _index);
                InventoryManager.Instance.setCurrentItemInMouse(temp);
            }
            _circuit.inputInventory.notifyInventoryUpdated();
        }
    }
    updateUi();
    InventoryManager.Instance.setCurrentItemInMouse(InventoryManager.Instance.currentItemInMouse);
}


    private void handleRightClick()
    {
        var stackItem = _circuit.inputInventory.getItem(_index);
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

            _circuit.inputInventory.notifyInventoryUpdated();
            updateUi();
        }
        else if (stackItem != null && stackItem.getStack() == 1)
        {
            InventoryManager.Instance.setCurrentItemInMouse(stackItem);
            _circuit.inputInventory.deleteItem(_index);
            _circuit.inputInventory.notifyInventoryUpdated();
            updateUi();
        }
    }
    public void clearSlot()
    {
	    _textureRect.Texture = null;
    }
}
