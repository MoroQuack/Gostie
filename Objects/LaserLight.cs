using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class LaserLight : Node2D,IToggleable
{
	[Export] public int RayCount = 360; 
	[Export] public float MaxDistance = 1000.0f;
	[Export] public float Width = 50f;
	[Export] public Color LineColor = new Color(80, 80, 80, 0.8f); 

	[Export] public bool On = true;

	private List<Vector2> _drawPoints = new List<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetPhysicsProcess(false);
		//Wait because it would load before walls
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
		CalculateDrawingPoints();
	}

	public void CalculateDrawingPoints()
	{
		if(!On)_EraseEverything();
		else
		{
			_drawPoints.Clear();
			var spaceState = GetWorld2D().DirectSpaceState;

			// GlobalTransform.X is exactly Forward. 
			// GlobalTransform.Y is exactly Sideways (Perpendicular).
			Vector2 forward = GlobalTransform.X.Normalized();
			Vector2 sideways = GlobalTransform.Y.Normalized();

			// Divide by RayCount - 1 so the first ray is exactly on the left edge and the last is exactly on the right edge
			float rayDistance = Width / Mathf.Max(1, RayCount - 1);

			// Create temporary lists so we don't mess up the polygon drawing order
			List<Vector2> startPoints = new List<Vector2>();
			List<Vector2> hitPoints = new List<Vector2>();

			for (int i = 0; i < RayCount; i++)
			{
				// 1. Calculate the spread from -Width/2 to +Width/2
				float currentOffset = (-Width / 2.0f) + (i * rayDistance);

				// 2. Move the global position sideways by the offset amount
				Vector2 globalStart = GlobalPosition + (sideways * currentOffset);
				
				// 3. Shoot the ray forward from that new start point
				Vector2 globalEnd = globalStart + (forward * MaxDistance);

				// Save the local start point for drawing later
				startPoints.Add(ToLocal(globalStart));

				// Create the ray parameter query
				var query = PhysicsRayQueryParameters2D.Create(globalStart, globalEnd);
				query.CollisionMask = 1 | 2;
				var result = spaceState.IntersectRay(query);

				// if Count > 0 then it hit something
				if (result.Count > 0)
				{
					//see what it hits
					Node collider = (Node)result["collider"];
					
					//if it's a lightswitch press it, else stop position
					if (collider.IsInGroup("lightswitch"))
					{
						collider.Call("LitUp");
					} else
					{
						hitPoints.Add(ToLocal((Vector2)result["position"]));
					}
					
					// if its player kill him
					if (collider.IsInGroup("player"))
					{
						collider.Call("getHit");
					}	
				}
				else
				{
					// draw full line
					hitPoints.Add(ToLocal(globalEnd));
				}
			} 

			// 4. Build the polygon outline in a perfect loop
			_drawPoints.AddRange(startPoints); // Base of the laser (left to right)
			hitPoints.Reverse();               // Reverse the hits so it draws backwards (right to left)
			_drawPoints.AddRange(hitPoints);   // End of the laser	
			QueueRedraw();
		}
		
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
	public void _EraseEverything()
	{
		_drawPoints.Clear();
		QueueRedraw();
	}

	
	public void ChangeOnOf()
	{
		if(On==true)_EraseEverything();
		On=!On;
	}
	public bool IsOn()
	{
		return On;
	}
}
