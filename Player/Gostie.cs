using Godot;
using System;

public partial class Gostie : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -500.0f;
	private bool _invincible = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player spawned");
		AddToGroup("player");
		_Wait();
	}

	private async void _Wait()
	{
		await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
		_invincible = false;
	}

	//public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 newVelocity = Velocity;

		Vector2 direction = Input.GetVector("left", "right","up","down");
		if (direction.X != 0 || direction.Y != 0)
		{
			//go
			newVelocity = Speed * direction;
		}
		else
		{
			//slow down
			newVelocity.X = Mathf.MoveToward(Velocity.X,0,Speed);
			newVelocity.Y = Mathf.MoveToward(Velocity.Y,0,Speed);
		}
		Velocity = newVelocity;
		MoveAndSlide();
	}

	public void getHit()
	{
		GD.Print("Player hit");
		if(!_invincible)GetTree().ReloadCurrentScene();
	}
}
