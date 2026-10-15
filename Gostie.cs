using Godot;
using System;

public partial class Gostie : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -500.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player spawned");
		AddToGroup("player");
	}


	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 newVelocity = Velocity;
		//gravity
		if (!IsOnFloor())
		{
			newVelocity.Y += Gravity * (float)delta;
		}

		//jump
		if(Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			newVelocity.Y = JumpVelocity;
		}

		float direction = Input.GetAxis("left", "right");
		if (direction != 0)
		{
			//go
			newVelocity.X = Speed * direction;
		}
		else
		{
			//slow down
			newVelocity.X = Mathf.MoveToward(Velocity.X,0,Speed);
		}
		Velocity = newVelocity;
		MoveAndSlide();
	}

	public void getHit()
	{
		GD.Print("Player hit");
		GetTree().ReloadCurrentScene();
	}
}
