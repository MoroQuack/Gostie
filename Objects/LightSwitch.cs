using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class LightSwitch : StaticBody2D
{
	//stuff it toggles
	[Export] public Godot.Collections.Array<Node2D> Targets;

	private bool _isLit = false;
	private bool _wasLit = false;

	public override void _Ready()
	{
		AddToGroup("lightswitch");
	}

	public override void _PhysicsProcess(double delta)
	{
		// 1. Check state changes
		if (_isLit && !_wasLit)
		{
			GD.Print("I got lighted up yoooo!");
			Modulate = new Color(1, 1, 0); //turn yellow
			ToggleEverything();
		}
		else if (!_isLit && _wasLit)
		{
			GD.Print("I'm not lit anymore :(");
			Modulate = new Color(1, 1, 1); // Return to normal
			ToggleEverything();
		}
  
		_wasLit = _isLit;

		_isLit = false;
	}


	// If light goes in
	public void LitUp()
	{
		_isLit = true; 	
	}

	private void ToggleEverything()
	{
		foreach (Node2D node in Targets)
		{
			if(node is IToggleable){
				(node as IToggleable).ChangeOnOf();
			}
		}
	}
}
