using Godot;
using System;

public partial class Gostie : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public float JumpVelocity = -500.0f;
	private bool _invincible = true;
	private bool _isDead = false;
	private AnimatedSprite2D _sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Player spawned");
		AddToGroup("player");
		_Wait();
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private async void _Wait()
	{
		await ToSignal(GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);
		_invincible = false;
	}

	//public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_isDead) return;
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
		Animate();
		MoveAndSlide();
	}

	public async void getHit()
	{
		if(_invincible) return;
		if(_isDead)return;
		GD.Print("Player hit");
		_isDead = true;
		_sprite.Hide();
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
   		GetNode<GpuParticles2D>("DeathParticles").Emitting = true;
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);		
		GetTree().ReloadCurrentScene();
	}
	
	private void Animate()
	{
		if(Velocity != Vector2.Zero)
		{
			_sprite.Play("walk");
		}else _sprite.Play("idle");

		if (Velocity.X > 0)
		{
			_sprite.FlipH = false; // Moving right, face normally
		}
		else if (Velocity.X < 0)
		{
			_sprite.FlipH = true;  // Moving left, flip horizontally
		}
	}
}
