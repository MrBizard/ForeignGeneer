using System;
using Godot;

public class Craft
{
    public Recipe recipe { get; private set; }

    private Inventory _input;
    private Inventory _output;
    public Timer craftTimer { get; set; }
    public float craftProgress { get; private set; } = 0;
    public bool isCrafting { get; private set; } = false;
    private Action _onCraftFinished;

    public Craft(Recipe recipe, Inventory input, Inventory output = null)
    {
        this.recipe = recipe;
        _input = input;
        _output = output;
    }

    public bool startCraft(Action onCraftFinished)
    {
        if (!canContinue())
            return false;
        if (compareRecipe())
        {
            isCrafting = true;
            _onCraftFinished = onCraftFinished;

            craftTimer.OneShot = true;
            craftTimer.WaitTime = recipe.duration;
            craftTimer.Timeout += onCraftFinished;
            craftTimer.Start();
            resetCraftProgress();
            consumeResources();
            return true;
        }
        return false;
    }

    public void stopCraft()
    {
        isCrafting = false;
        if (craftTimer != null)
        {
            craftTimer.Stop();
            if (_onCraftFinished != null)
            {
                craftTimer.Timeout -= _onCraftFinished;
                _onCraftFinished = null;
            }

            resetCraftProgress();
        }
    }

    public bool compareRecipe()
    {
        if (recipe == null || recipe.input == null || _input == null)
        {
            return false;
        }
        if (_output == null || _output.getItem(0) == null || _output.getItem(0).canAdd(recipe.output))
        {
            for (int i = 0; i < recipe.input.Count; i++)
            {
                var requiredItem = recipe.input[i];
                var slotItem = _input.getItem(i);

                if (slotItem == null || slotItem.getResource() != requiredItem.getResource() || slotItem.getStack() < requiredItem.getStack())
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public bool consumeResources()
    {
            for (int i = 0; i < recipe.input.Count; i++)
            {
                var requiredItem = recipe.input[i];
                var slotItem = _input.getItem(i);

                if (slotItem != null && slotItem.getResource() == requiredItem.getResource())
                {
                    slotItem.subtract(requiredItem.getStack());

                    if (slotItem.isEmpty())
                    {
                        _input.deleteItem(i);
                    }
                }
                else
                {
                    return false;
                }
        }
        return true;
    }

    public bool addOutput()
    {
        if (recipe == null || recipe.output == null || _output == null)
        {
            return true;
        }

        var recipeItem = recipe.output;
        var outputSlotItem = _output.getItem(0);

        if (outputSlotItem == null)
        {
            _output.addItemToSlot(new StackItem(recipeItem.getResource(), recipeItem.getStack()), 0);
            return true;
        }
        else if (outputSlotItem.getResource() == recipeItem.getResource())
        {
            int remainingSpace = outputSlotItem.getResource().getMaxStack - outputSlotItem.getStack();
            if (remainingSpace >= recipeItem.getStack())
            {
                outputSlotItem.add(recipeItem.getStack());
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void resetCraftProgress()
    {
        craftProgress = 0;
    }

    public void updateCraftProgress(double delta)
    {
        craftProgress += (float)delta / recipe.duration;
    }

    public bool canContinue()
    {
        if (isCrafting)
            return false;
        if (_output == null)
            return true;
        var outputSlotItem = _output.getItem(0);
        return outputSlotItem == null ||
               outputSlotItem.getStack() + recipe.output.getStack()
               < outputSlotItem.getResource().getMaxStack;
    }
     
    public override string ToString()
    {
        return "\n recipe : " + recipe.ToString() + "\n _output: " + _output.ToString() + "\n _input: " + _input.ToString();
    }
}