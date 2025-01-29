using Godot;

namespace ForeignGeneer.Assets.Scripts.manager;

public partial class PollutionManager : Node
{
    public static PollutionManager instance { get; private set; }

    private float _pollution = 0;
    
    public PollutionManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance = null;
        }
    }

    public void addPolution(float pollution)
    {
        _pollution += pollution;
    }

    public void removePolution(float pollution)
    {
        _pollution -= pollution;
    }

    public float getPollution()
    {
        return _pollution;
    }

    public bool hasPollution(float pollution)
    {
        return _pollution >= pollution;
    }
    
}