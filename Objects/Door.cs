using Godot;
using System;

public partial class Door : StaticBody2D,IToggleable
{
	[Export] public bool Opened = false;
	// Called when the node enters the scene tree for the first time.
	
	private CollisionShape2D _collider;
	private AnimatedSprite2D _sprite;
	
	//sounds
	[Export] public AudioStream OpenSound;
	[Export] public AudioStream CloseSound;
	private AudioStreamPlayer _audioPlayer;

	public override void _Ready()
	{
		_collider = this.GetNode<CollisionShape2D>("CollisionShape2D");
		_sprite = this.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if(Opened){
			_sprite.Play("opened");
			_collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
		else _sprite.Play("closed");
		_audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeOnOf()
	{
		if (Opened)
		{
			_collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			_sprite.PlayBackwards("opening");
			_audioPlayer.Stream = CloseSound; 
			
		}
		else
		{
			_collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			_sprite.Play("opening");
			_audioPlayer.Stream = OpenSound; 
		}
		Opened = !Opened;
		_audioPlayer.Play();

	}

	public bool IsOn()
	{
		return Opened;
	}

}
