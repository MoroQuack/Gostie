using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class Switch : Area2D
{
	//stuff it toggles
	[Export] public Godot.Collections.Array<Node2D> Targets;


	//sounds
	[Export] public AudioStream OnSound;
	[Export] public AudioStream OffSound;
	private AudioStreamPlayer _audioPlayer;

	private bool _isPressed = false;
	private AnimatedSprite2D _sprite;



	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		PlayAnimation();
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
		PlayAnimation();
		PlaySounds();
	}
	private void PlayAnimation()
	{
		if (_isPressed)_sprite.Play("on");
		else _sprite.Play("off");
	}
	private void PlaySounds()
	{
		if (_isPressed)_audioPlayer.Stream = OnSound; 
		else _audioPlayer.Stream = OffSound; 
		_audioPlayer.Play();
	}
}
