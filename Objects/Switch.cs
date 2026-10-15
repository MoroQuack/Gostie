using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class Switch : Area2D
{
	//stuff it toggles
	[Export] public Godot.Collections.Array<Node2D> Targets;

	private bool _isPressed = false;

	// You would call this when the player steps on it or presses interact
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
	}
	public void _on_body_exited(Node2D body)
	{
		Press();
	}
}
