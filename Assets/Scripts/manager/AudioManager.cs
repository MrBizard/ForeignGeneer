using Godot;
using System;

public partial class AudioManager : Node
{
	private float masterVolume = 1.0f;
	private float sfxVolume = 1.0f;
	private float bgmVolume = 1.0f;
	private float rmVolume = 1.0f;
	
	public override void _Ready(){
		masterVolume = getBusVolume("Master");
		sfxVolume = getBusVolume("SFX");
		bgmVolume = getBusVolume("BGM");
		rmVolume = getBusVolume("RM");
		SetBusVolume("SFX",sfxVolume);
		SetBusVolume("BGM",bgmVolume);
		SetBusVolume("RM",rmVolume);
	}
	
	public void SetBusVolume(string bus, float value){
		
		float volumeDB = -72.0f - (-72.0f * value);
		int busID = AudioServer.GetBusIndex(bus);
		AudioServer.SetBusVolumeDb(busID, volumeDB);
		GD.Print("bus id: " + busID + " name : " + AudioServer.GetBusName(busID) + " volume : " + AudioServer.GetBusVolumeDb(busID));
	}
	
	public float getBusVolume(string bus)
	{
		int busID = AudioServer.GetBusIndex(bus);
		float volumeDb = AudioServer.GetBusVolumeDb(busID);
		return Mathf.Pow(10, volumeDb / 20);

	}
}
