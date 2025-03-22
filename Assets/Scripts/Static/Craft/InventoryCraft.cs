using System;

namespace ForeignGeneer.Assets.Scripts.Static.Craft.craft;

public class InventoryCraft : BaseCraft
{
    public InventoryCraft(Recipe recipe, Inventory input, Inventory output) : base(recipe, input, output) { }

    protected override bool compareRecipe()
    {
        if (recipe == null || recipe.input == null || _input == null)
        {
            return false;
        }

        foreach (var requiredItem in recipe.input)
        {
            bool found = false;
            for (int i = 0; i < _input.slots.Count; i++)
            {
                var slotItem = _input.getItem(i);
                if (slotItem != null && slotItem.getResource() == requiredItem.getResource() && slotItem.getStack() >= requiredItem.getStack())
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }
        }

        return true;
    }

    protected override bool consumeResources()
    {
        foreach (var requiredItem in recipe.input)
        {
            for (int i = 0; i < _input.slots.Count; i++)
            {
                var slotItem = _input.getItem(i);
                if (slotItem != null && slotItem.getResource() == requiredItem.getResource())
                {
                    int amountToConsume = Math.Min(slotItem.getStack(), requiredItem.getStack());
                    slotItem.subtract(amountToConsume);

                    if (slotItem.isEmpty())
                    {
                        _input.deleteItem(i);
                    }

                    requiredItem.subtract(amountToConsume);
                    if (requiredItem.getStack() <= 0)
                    {
                        break;
                    }
                }
            }
        }
        return true;
    }

    public override bool addOutput()
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
}