public class FixedInputCraft : BaseCraft
{
    public FixedInputCraft(Recipe recipe, Inventory input) : base(recipe, input) { }

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
        return true;
    }
}