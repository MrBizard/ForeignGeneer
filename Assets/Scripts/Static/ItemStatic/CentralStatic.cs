using Godot;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(CentralStatic), "", nameof(FactoryStatic))]
[GlobalClass]
public partial class CentralStatic : FactoryStatic
{
    [Export] public float duration { get; set; }

    /// <summary>
    /// Starts the operation of the power plant.
    /// </summary>
    public void StartOperation()
    {
        GD.Print("Power plant operation started with duration: " + duration);
        // Logique spécifique à la centrale
    }
}