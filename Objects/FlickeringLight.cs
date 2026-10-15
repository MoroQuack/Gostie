using Godot;
using System;

public partial class FlickeringLight : LaserLight
{

	[Export] public double TurnOffTime = 0;
	[Export] public double StartingTurnOffTime = 0;
	private double _turnOffTimer = 0;
	private bool _on;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_on = true;
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(TurnOffTime != 0)
		{
			_turnOffTimer += delta;
			if(_turnOffTimer >= TurnOffTime)
			{
				_turnOffTimer = 0;
				_on = !_on;
				GD.Print(_on);
			}
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
