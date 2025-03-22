using Godot;
using System;

public partial class Hand : BoneAttachment3D
{
	//main et l'objet a attaché
	[Export] private BoneAttachment3D handNode;
	 private MeshInstance3D objectToAttach;
	public InventoryManager inv;
	private Node3D intermediateNode;

	private StaticBody3D objectInstance;
	Vector3 targetSize;
	

	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		inv=GetNode<InventoryManager>("/root/Main/Manager/InventoryManager");
		if(inv==null){
			GD.PrintErr("non trouvé");
		}
		
		if (handNode == null)
        {
            GD.PrintErr("node de la main non assigné (script Hand.cs)");
        }
		
		
	}

    /// <summary>
	/// Put the actual slot object in the hand of the player
	/// </summary>
	public void AttachObjectToHand()
    {
		if(objectInstance!=null){
			objectInstance.QueueFree();
			objectInstance=null;
		}
 
		int actuelSlot=inv.currentSlotHotbar; // slot actuel
		objectInstance=inv.hotbar.getItem(actuelSlot).getResource().instantiate();//  l'objet du slot actuel

        //objectInstance = (StaticBody3D)objectToAttach.Instantiate();
		objectInstance.SetCollisionLayerValue(1,false);
		objectInstance.SetCollisionMaskValue(1,false);

		Vector3 targetSize = new Vector3(0.2f, 0.2f, 0.2f);

		// Calculer l'échelle nécessaire pour atteindre la taille cible
		Vector3 originalSize = objectInstance.Scale;
		Vector3 scaleFactor = targetSize / originalSize;

		// Appliquer l'échelle calculée
		objectInstance.Scale = scaleFactor;
        objectInstance.RotationDegrees = new Vector3(-90,-45, 0); // Adjust for orientation
		//objectInstance.Position = new Vector3(-0.1f,0f,0f); // Adjust for orientation

		handNode.AddChild(objectInstance);
    }

	
}
