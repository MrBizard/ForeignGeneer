using Godot;

public abstract class BotAction
{
    public abstract void Execute(Bot bot);
}
public class MineAction : BotAction
{
	public BreakableResource m_res { get; private set; }
    public MineAction(BreakableResource res)
    {
        m_res = res;
    }
    public override void Execute(Bot bot)
    {
        bot.mine(this);
    }
}
public class TransportAction : BotAction
{
	public Inventory m_input { get; private set; }
    public Inventory m_ouput { get; private set; }
    public ItemStatic m_item { get; private set; }

    public TransportAction(Inventory input, Inventory output, ItemStatic item)
	{
		m_input = input;
		m_ouput = output;
        m_item = item;
    }
    public override void Execute(Bot bot)
    {
        bot.transport(this);
    }

    public StackItem getItemFromInput()
    {
        m_input.FindItem(m_item);
        return new StackItem();
    }
    public void setItemInOutput(Inventory botInv)
    {

    }

    public void move()
    {

    }
}

public partial class Bot : CharacterBody3D
{
    BotAction action;
    Inventory m_inventory;
	public override void _PhysicsProcess(double delta)
	{
        action?.Execute(this);
    }

	public void mine(MineAction action)
	{
        BreakableResource res = action.m_res;
        ItemStatic item = res.GetItem();
        m_inventory.addItem(res.botInteract());
    }
	public void transport(TransportAction action)
	{
        
	}
}
