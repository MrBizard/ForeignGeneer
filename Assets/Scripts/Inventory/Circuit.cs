using System;
using ForeignGeneer.Assets.Scripts;
using ForeignGeneer.Assets.Scripts.Block;
using ForeignGeneer.Assets.Scripts.manager;
using Godot;
using Godot.Collections;

public partial class Circuit : Node, IObservable, IInteractable
{
    public FixedInputOutputCraft _craft;
    public Recipe currentRecipe;
    private Timer _craftTimer;
    public Inventory inputInventory;
    public Inventory outputInventory;
    public override void _Ready()
    {
        base._Ready();
        _craftTimer = new Timer();
        _craftTimer.Name = "CraftTimer";
        AddChild(_craftTimer);
        generateRandomRecipe();
        outputInventory = new Inventory(1);
    }

    public void attach(IObserver observer)
    {
        
    }

    public void detach(IObserver observer)
    {
        
    }

    public void notify(InterfaceType? interfaceType = null)
    {
        
    }

    private void generateRandomRecipe()
    {
        Random rand  = new Random();
        currentRecipe = MiniGameManager.instance.listRecipe[rand.Next( MiniGameManager.instance.listRecipe.Count)];
        inputInventory = new Inventory(currentRecipe.input.Count);
        notify();
    }
    private void startCraft()
    {
        if (currentRecipe == null)
            generateRandomRecipe();
        
        _craft = new FixedInputOutputCraft(currentRecipe, inputInventory, outputInventory);
        _craft.craftTimer = _craftTimer;
        _craft.startCraft(onCraftFinished);
    }

    private void onCraftFinished()
    {
        _craft.stopCraft();
        _craft.addOutput();
        notify();
    }

    private void onCraftChange()
    {
        generateRandomRecipe();
    }
    public void interact(InteractType interactType)
    {
        switch (interactType)
        {
            case InteractType.Modify:
                onCraftChange();
                break;
            case InteractType.Craft:
                startCraft();
                break;
        }
    }
}