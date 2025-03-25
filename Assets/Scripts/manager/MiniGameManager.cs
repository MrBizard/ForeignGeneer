using System;
using Godot;
using Godot.Collections;

namespace ForeignGeneer.Assets.Scripts.manager;

public partial class MiniGameManager:Node
{
    public static MiniGameManager instance { get; private set; }
    [Export] private Array<Recipe> listRecipe = new Array<Recipe>();
    
    private Circuit _circuit = new Circuit();
    public override void _Ready()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
}