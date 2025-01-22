using Godot;
using System;

public partial class HealthManager : Node
{
    [Export] public Player player; // Reference to the Player node
    [Export] public HealthBar healthManager; // Reference to the HealthBar node
     private float previousVelocityY = 0.0f;
    
    private double timer=10.0;


    public override void _Ready()
    {
        if (player == null)
        {
            GD.PrintErr("Player node n'est pas assigné (HealthBar manager)");
        }
        

        if (healthManager == null)
        {
            GD.PrintErr("HealthBar node is not assigned!");
        }

    }

    public override void _Process(double delta)
    {
        timer-=delta;
        if (timer<=0)
        {
            timer=10;
            // Add 1 to the value of the Value property of the TextureProgress node
            if(healthManager.get_hunger()>80 && healthManager.get_health()<100){
                healthManager.healthBar.Value+=10;
            }
            if(healthManager.get_hunger()<=1){
                healthManager.healthBar.Value-=5;
            }

            healthManager.remove_hunger();

        }
        if (healthManager.get_health()<=0){
            player.GlobalPosition= new Vector3(0,0,0);
            healthManager.healthBar.Value=100;
            healthManager.hungerBar.Value=100;
        }
        if(Input.IsActionPressed("ui_up")){
            //healthManager.add_health();
            healthManager.add_hunger();
        }
        else if(Input.IsActionPressed("ui_down")){
            //healthManager.remove_health();
            healthManager.remove_hunger();
        }

        // Degat de chute
        if (player.IsOnFloor() && previousVelocityY < -8)
        {
            // calcule des degats (velocité * 2)
            float fallDamage = Mathf.Abs(previousVelocityY);
            healthManager.healthBar.Value-=fallDamage*2;
        }

        // Update previous vertical velocity
        previousVelocityY = player.Velocity.Y;
        
        // If we press the down arrow key
        
    }

}