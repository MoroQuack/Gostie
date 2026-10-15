using Godot;
using System;

public partial class Door : StaticBody2D,IToggleable
{
	[Export] public bool Opened = false;
	// Called when the node enters the scene tree for the first time.
	
	private CollisionShape2D _collider;
	
	public override void _Ready()
	{
		_collider = this.GetNode<CollisionShape2D>("CollisionShape2D");
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
			this.Visible = true;
		}
		else
		{
			_collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			this.Visible = false;
		}
		Opened = !Opened;

	}

	public bool IsOn()
	{
		return Opened;
	}

}
