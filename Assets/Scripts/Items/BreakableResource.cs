using Godot;
using System;
using ForeignGeneer.Assets.Scripts.Block;

public partial class BreakableResource : StaticBody3D, IInteractable
{
	public StackItem item {get;set;}
	[Export] public ResourceStatic resourceStatic {get;set;}
	[Export]private double timer=5.0f;
	public bool IsActive=false;
	private CollisionShape3D collisionShape;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		item= new StackItem(resourceStatic);
		collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        
			
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(IsActive){
			this.Visible=false;
			collisionShape.Disabled = true;
			timer-=delta;
			if(timer<=0){
				timer=10;
				this.Visible=true;
				collisionShape.Disabled = false;
				IsActive=false;
			}
		}
	}
	public ItemStatic GetItem(){
		return item.getResource();
	}


	public void interact(InteractType interactType)
	{
		switch (interactType)
		{
			case InteractType.Interact:
				IsActive = true;
				InventoryManager.Instance.addItemToInventory(item);
				item.getResource().LeftClick();
				break;
		}
	}
}
