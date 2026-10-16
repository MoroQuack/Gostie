using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class Switch : Area2D
{
	//stuff it toggles
	[Export] public Godot.Collections.Array<Node2D> Targets;

	private bool _isPressed = false;
	private AnimatedSprite2D _sprite;



	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ChangeState();
	}

	// Press it
	public void Press()
	{
		GD.Print("Click!");
		_isPressed = !_isPressed; 
	  
		foreach (Node2D node in Targets)
		{
			if(node is IToggleable){
				(node as IToggleable).ChangeOnOf();
			}
		}
		ChangeState();
	}
	public void _on_body_entered(Node2D body)
	{
		Press();
	}
	public void ChangeState()
	{
		if (_isPressed)
		{
			_sprite.Play("on");
		}
		else
		{
			_sprite.Play("off");
		}
	}
}
