using Godot;

namespace ForeignGeneer.Assets.Scripts.manager;

public partial class EnergyManager : Node
{
	public static EnergyManager instance { get; private set; }

	public EnergyManager()
	{
		instance = this;
	}
	
	private float _globalElectricity = 100;
	
	public float getGlobalElectricity()
	{
		return _globalElectricity;
	}

	public void addGlobalElectricity(float value)
	{
		_globalElectricity = _globalElectricity + value;
	}
	public void removeGlobalElectricity(float value)
	{
		_globalElectricity = _globalElectricity - value;
	}

	public bool hasEnergy(float value)
	{
		return _globalElectricity - value >= 0;
	}

	public bool isDown()
	{
		return _globalElectricity <= 0;
	}
}
