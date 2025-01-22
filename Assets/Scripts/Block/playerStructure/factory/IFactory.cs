
using System.Collections.Generic;
using ForeignGeneer.Assets.Scripts.Static.Craft;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : PlayerStructure
{
    public RecipeList recipeList { get; set; }
    Craft craft { get; set; }
    FactoryStatic factoryStatic { get; set; }
    Inventory input { get; set; }
    short tier { get; set; }
    public void setCraft(Recipe recipe);

}