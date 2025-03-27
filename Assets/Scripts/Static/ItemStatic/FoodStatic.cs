using ForeignGeneer.Assets.Scripts.Block;
using Godot;
using MonoCustomResourceRegistry;
using System.Security.Cryptography.X509Certificates;
[RegisteredType(nameof(FoodStatic), "", nameof(ItemStatic))]
[GlobalClass]

public partial class FoodStatic : ItemStatic
{	[Export]
	public AudioStream eatingSound;
	[Export]
	public float feedPower = 1;

	public override void RightClick()
	{
		InventoryManager inventoryManager = InventoryManager.Instance;
		HealthBar healthManager = HealthManager.instance.healthManager;
		int currentslot = inventoryManager.currentSlotHotbar;
		if (healthManager.hungerBar.Value < healthManager.hungerBar.MaxValue)
		{
			inventoryManager.hotbar.removeItem(currentslot, 1);
			healthManager.addHunger(feedPower);
		}
		GD.Print(eatingSound.ToString(), "this is eatingsound");
		if (eatingSound != null)  AudioManager.instance.playSound(eatingSound);
		   
		
	}

}
