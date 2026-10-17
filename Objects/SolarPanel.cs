using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class SolarPanel : StaticBody2D
{
	//stuff it toggles
	[Export] public Godot.Collections.Array<Node2D> Targets;
	private AnimatedSprite2D _sprite;

	private bool _isLit = false;
	private bool _wasLit = false;

	public override void _Ready()
	{
		AddToGroup("solarpanel");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if(!_isLit)_sprite.Play("off");
	}

	public override void _PhysicsProcess(double delta)
	{
		// 1. Check state changes
		if (_isLit && !_wasLit)
		{
			GD.Print("I got lighted up yoooo!");
			_sprite.Play("on");
			ToggleEverything();
		}
		else if (!_isLit && _wasLit)
		{
			GD.Print("I'm not lit anymore :(");
			_sprite.Play("off");
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
