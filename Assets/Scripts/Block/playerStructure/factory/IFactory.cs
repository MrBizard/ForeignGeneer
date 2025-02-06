
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : IPlayerStructure
{
    Craft craft { get; set; }
    Inventory input { get; set; }
    float craftProgress { get; }
    Timer craftTimer { get; set; }
    bool isCrafting { get; } 
    FactoryStatic factoryStatic { get; set; } 

    void closeUi();
    void openUi();
}