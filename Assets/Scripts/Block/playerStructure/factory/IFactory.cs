
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.block.playerStructure.Factory;

public interface IFactory : PlayerStructure
{
    public Godot.Collections.Array<Recipe> recipeList { get; set; }
    Craft craft { get; set; }
    FactoryStatic factoryStatic { get; set; }
    Inventory input { get; set; }
    short tier { get; set; }
    
}