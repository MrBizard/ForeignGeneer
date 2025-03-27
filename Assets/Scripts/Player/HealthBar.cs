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
        pollutionBar.Value = 20;
        hungerBar.Value = 100;
    }

    public void removeHealth(float healthpoint = 1)
    {
        if (healthBar.Value - healthpoint <= healthBar.MinValue)
            healthBar.Value = healthBar.MinValue;
        else
        {
            healthBar.Value -= healthpoint;
        }
    }

    public void addHealth(float healthpoint = 1){
        if (healthBar.Value + healthpoint > healthBar.MaxValue)
            healthBar.Value = healthBar.MaxValue;
        else
        {
            healthBar.Value += healthpoint;
        }
    }

    public void removeHunger(float hungerpoint = 1){

        if (hungerBar.Value - hungerpoint <= hungerBar.MinValue)
            hungerBar.Value = hungerBar.MinValue;
        else
        {
            hungerBar.Value -= hungerpoint;
        }
    }

    public void addHunger(float hungerpoint = 1)
    {
        if(!(hungerBar.Value + hungerpoint > hungerBar.MaxValue))
        hungerBar.Value = hungerBar.MaxValue;
        else
        {
            hungerBar.Value += hungerpoint;
        }
        
    }

    public void removePollution(float pollutionpoint = 1)
    {
        pollutionBar.Value-=pollutionpoint; 
    }

    public void addPollution(float pollutionpoint = 1)
    {
        pollutionBar.Value+=pollutionpoint;
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