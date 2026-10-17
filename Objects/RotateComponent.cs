using Godot;

public partial class RotateComponent : Node
{
	[Export] public float SpinSpeed = 2.0f;
	[Export] bool clockwise = true;
	
	// This will hold whatever object this component is attached to
	private Node2D _parent;

	public override void _Ready()
	{
		// Get the parent node that this script is a child of
		_parent = GetParent<Node2D>();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_parent != null)
		{
			if(clockwise)_parent.Rotation += SpinSpeed * (float)delta;
			else _parent.Rotation -= SpinSpeed * (float)delta;
		}
	}
}
