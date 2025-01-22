using Godot;
using System;

public partial class HealthBar : Control
{
	//bar de progression
	[Export] public TextureProgressBar healthBar ;
    [Export] public TextureProgressBar pollutionBar;
    [Export] public TextureProgressBar hungerBar;


    

    public override void _Ready()
    {
        base._Ready();

        //initialisation des valeur
        healthBar.Value = 100;
        pollutionBar.Value=20;
        hungerBar.Value=100;
    }

    public void remove_health(){
        healthBar.Value-=1;
    }

    public void add_health(){
        healthBar.Value+=1;
    }

    public void remove_hunger(){
        hungerBar.Value-=1;
    }

    public void add_hunger(){
        hungerBar.Value+=1;
    }

    public void remove_pollution(){
        pollutionBar.Value-=1;
    }

    public void add_pollution(){
        pollutionBar.Value+=1;
    }

    public double get_health(){
        return healthBar.Value;
    }
    public double get_hunger(){
        return hungerBar.Value;
    }
    public double get_pollution(){
        return pollutionBar.Value;
    }
}