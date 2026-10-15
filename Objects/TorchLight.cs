using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class TorchLight : Node2D,IToggleable
{
	[Export] public int RayCount = 360; 
	[Export] public float MaxDistance = 1000.0f;
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
			if(RayCount != 360)_drawPoints.Add(Vector2.Zero);

			var spaceState = GetWorld2D().DirectSpaceState;

			
			float angleStep = Mathf.Tau / 360;

			for (int i = 0; i < RayCount; i++)
			{
			
				float angle = (90 - RayCount/2 + RotationDegrees + i) * angleStep;
				
				Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
				
				// calculate end (if no hit)
				Vector2 endPosition = GlobalPosition + (direction * MaxDistance);

				// Create the ray parameter query
				var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, endPosition);
				query.CollisionMask = 1 | 2;
				var result = spaceState.IntersectRay(query);

				Vector2 drawEndPos;

				// if Count > 0 then it hit smth
				if (result.Count > 0)
				{
					//see what it hits
					Node collider = (Node)result["collider"];
					
					//if it's a lightswitch press it, else stop position
					if (collider.IsInGroup("lightswitch"))
					{
						collider.Call("LitUp");
						drawEndPos = ToLocal(endPosition);
					} else
					{
						drawEndPos = ToLocal((Vector2)result["position"]);
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
					drawEndPos = ToLocal(endPosition);
				}
				_drawPoints.Add(drawEndPos);
			}			
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
