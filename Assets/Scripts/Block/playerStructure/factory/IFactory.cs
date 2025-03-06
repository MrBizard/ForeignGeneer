
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : IPlayerStructure
{
    FactoryStatic factoryStatic { get; set; } 
    void closeUi();
    void openUi();
}