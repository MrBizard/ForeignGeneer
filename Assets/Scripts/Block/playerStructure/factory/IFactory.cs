
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : IPlayerStructure, IRecipeUser
{
    Craft craft { get; set; }
    Inventory input { get; set; }
    short tier { get; set; }
    public void closeUi();
    public void openUi();
}