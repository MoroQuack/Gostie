using Godot;
using System;

public partial class FlickeringLight : LaserLight
{

	[Export] public double StaysOnTimer = 0;
	[Export] public double StaysOffTimer = 0;
	[Export] public double StartingTime = 0;
	[Export] public bool StartsOn = true;
	private double _timer = 0;
	private bool _on;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_on = StartsOn;
		_timer = StartingTime;
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		_timer += delta;
		if((_on && _timer>=StaysOnTimer) || (!_on && _timer>=StaysOffTimer))
		{
			_timer = 0;
			_on = !_on;
			GD.Print(_on);
		}
		
		if (_on)
		{
			base._PhysicsProcess(delta);
		}
		else
		{
			base._EraseEverything();
		}
	}
}
