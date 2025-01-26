using Godot;
using System;

public partial class MusicController : AudioStreamPlayer2D
{
	private BobMovement3 _bob;
	[Export] public AudioStream OverwaterMusic;

	[Export] public float SwapMusicTime = 3;
	private bool _isToggled;
	private bool _isTurningOn;
	private double _tick;

	private float _ogVolume;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bob = GetNode<BobMovement3>("../../bob");
		if (_bob == null) throw new Exception("BobMovement3 is null");
		_ogVolume = VolumeDb;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Playing)
		{
			Playing = true;
		}
		if (_isTurningOn)
		{
			VolumeDb += 5f * (float) delta;
			if (VolumeDb > _ogVolume)
			{
				_isTurningOn = false;
				VolumeDb = _ogVolume;
			}
		}
		if (!(_bob.skyLimit >= _bob.GlobalPosition.Y) || _isToggled) return;
		_tick += delta;
		VolumeDb -= 5f * (float) delta;
		if (_tick < SwapMusicTime) return;
		_isToggled = true;
		_isTurningOn = true;
		Stream = OverwaterMusic;
		Playing = true;
	}
}
