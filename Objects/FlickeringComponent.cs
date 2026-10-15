using Godot;

public partial class FlickeringComponent : Node
{
	[Export] public double StaysOnTimer = 0;
	[Export] public double StaysOffTimer = 0;
	[Export] public double StartingTime = 0;
	private double _timer = 0;
	
	// This will hold whatever object this component is attached to
	private IToggleable _parent;

	public override void _Ready()
	{
		// Get the parent node that this script is a child of
		_parent = (IToggleable)GetParent<Node2D>();
		_timer = StartingTime;
	}

	public override void _PhysicsProcess(double delta)
	{
		_timer += delta;
		if((_parent.IsOn() && _timer >= StaysOnTimer)){
			_parent.ChangeOnOf();
			_timer = 0;
		}else if (!_parent.IsOn() && _timer >= StaysOffTimer)
		{
			_parent.ChangeOnOf();
			_timer = 0;
		}
	}
}
