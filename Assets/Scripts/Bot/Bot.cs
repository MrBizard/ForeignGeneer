using Godot;
using System.Net.Sockets;

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

	public TransportAction(Inventory input, Inventory output)
	{
		m_input = input;
		m_ouput = output;
	}
	public override void Execute(Bot bot)
	{
		bot.transport(this);
	}

	public void getItemFromInput(Inventory bot)
	{
		var slots = m_input.slots;
		for (int i = 0; i < slots.Count ; i++)
		{
			if (slots[i] != null)
			{
				if (m_item == null)
				{
					m_item = slots[i].getResource();
				}
				
				if (slots[i].Resource == m_item)
				{
					StackItem it = m_input.getItem(i);
					bot.addItem(it);
					m_input.deleteItem(i);
				}
			}
		}
	}

	public void setItemInOutput(Inventory bot)
	{
		for (int i = 0; i < bot.slots.Count; i++)
		{
			if (bot.slots[i] != null)
			{
				if (m_item == null)
				{
					if (bot.slots[i].Resource == m_item)
					{
						StackItem it = bot.getItem(i);
						m_ouput.addItem(it);
						bot.deleteItem(i);
					}
				}
				
			}
		}
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
		action.getItemFromInput(m_inventory);
		action.setItemInOutput(m_inventory);
	}
}
