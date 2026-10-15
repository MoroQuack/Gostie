using Godot;
using System;


public partial class Goal : Area2D
{
	[Export]
	public string nextScene = "level2.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("player won!");
			GetTree().ChangeSceneToFile("res://Levels/" + nextScene);
		}
		
	}
}
