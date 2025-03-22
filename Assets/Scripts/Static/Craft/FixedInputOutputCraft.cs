public class FixedInputOutputCraft : BaseCraft
{
    public FixedInputOutputCraft(Recipe recipe, Inventory input, Inventory output) : base(recipe, input, output) { }

    protected override bool compareRecipe()
    {
        if (recipe == null || recipe.input == null || _input == null)
        {
            return false;
        }

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

    protected override bool consumeResources()
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