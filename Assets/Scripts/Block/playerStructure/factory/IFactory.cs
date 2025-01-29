
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : IPlayerStructure, IRecipeUser
{
    Craft craft { get; set; }
    Inventory input { get; set; }
    public void closeUi();
    public void openUi();
    public float craftProgress { get; }
    public Timer craftTimer { get; set;}
    public FactoryStatic factoryStatic { get; set; }
}