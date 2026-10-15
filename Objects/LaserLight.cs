using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class LaserLight : Node2D
{
	[Export] public int RayCount = 360; 
	[Export] public float MaxDistance = 1000.0f;
	[Export] public Color LineColor = new Color(80, 80, 80, 0.8f); 

	private List<Vector2> _drawPoints = new List<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Wait because it would load before walls
		SetPhysicsProcess(false);
		_Wait();
		//Redraws
		QueueRedraw();
	}
	private async void _Wait()
	{
		await ToSignal(GetTree().CreateTimer(0.05), SceneTreeTimer.SignalName.Timeout);
		SetPhysicsProcess(true);
	}

	public override void _PhysicsProcess (double delta)
	{
		_drawPoints.Clear();
		//_drawPoints.Add(Vector2.Zero);

		var spaceState = GetWorld2D().DirectSpaceState;

		// calculate when to make rays (tau is 360 degrees)
		float angleStep = Mathf.Tau / RayCount;

		for (int i = 0; i < RayCount; i++)
		{
			float angle = i * angleStep;
			
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			
			// calculate end (if no hit)
			Vector2 endPosition = GlobalPosition + (direction * MaxDistance);

			// Create the ray parameter query
			var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, endPosition);
			
			var result = spaceState.IntersectRay(query);

			Vector2 drawEndPos;

			// if Count > 0 then it hit a wall
			if (result.Count > 0)
			{
				// finish line at wall
				drawEndPos = ToLocal((Vector2)result["position"]);
				//see if its player
				Node collider = (Node)result["collider"];
				if (collider.IsInGroup("player"))
				{
					collider.Call("getHit");
				}
			}
			else
			{
				// draw full line
				drawEndPos = ToLocal(endPosition);
			}
			_drawPoints.Add(drawEndPos);
		}
		QueueRedraw();
	}


	
	public override void _Draw()
	{
		/*
		foreach (Vector2 point in _drawPoints)
		{
			DrawLine(Vector2.Zero, point, LineColor, 2.0f);
		}
		*/

		// Don't draw if we don't have enough points to build a shape
		if (_drawPoints.Count < 3) return;

		// --- 2. Draw the filled shape ---
		// Generates a transparent 2D polygon connecting all the ray points
		DrawPolygon(_drawPoints.ToArray(), new Color[] { LineColor });
	}
}
