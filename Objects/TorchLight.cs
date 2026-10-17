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
	private AnimatedSprite2D _sprite;

	private List<Vector2> _drawPoints = new List<Vector2>();
	private List<Vector2> _oldPoints = new List<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetPhysicsProcess(false);
		//Wait because it would load before walls
		_Wait();
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
		Animate();	
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

			List<Vector2> newPoints = new List<Vector2>();

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
					drawEndPos = ToLocal((Vector2)result["position"]);
					//see what it hits
					Node collider = (Node)result["collider"];
					
					//if it's a lightswitch press it, else stop position
					if (collider.IsInGroup("solarpanel"))
					{
						collider.Call("LitUp");
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
				newPoints.Add(drawEndPos);
			}		

			//check if has to be redrawn


			bool needsRedraw = false;
			//GD.Print("New points: " + newPoints.Count());
			//GD.Print("Old points: " + _oldPoints.Count());
			if (newPoints.Count != _oldPoints.Count)
			{
				needsRedraw = true; // Different number of points 
			} else
			{
				for (int i = 0; i < newPoints.Count; i++)
				{
					// If any point moved by more than a tiny fraction of a pixel, trigger a redraw.
					float distanceSq = newPoints[i].DistanceSquaredTo(_oldPoints[i]);
					if (distanceSq > 1.0f)
					{
						needsRedraw = true;
						break;
					}
				}
			}

			if(needsRedraw){
				//GD.Print("Drawing...");
				_drawPoints.AddRange(newPoints);
				_oldPoints.Clear();
				_oldPoints.AddRange(newPoints);
				QueueRedraw();
			}	
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
		_oldPoints.Clear();
		QueueRedraw();
	}

	public async void ChangeOnOf()
	{
		if(On==true)_EraseEverything();
		if (On == false)
		{
			await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
		}
		On=!On;
	}
	public bool IsOn()
	{
		return On;
	}

	private void Animate()
	{
		if(RayCount==360 && IsOn())_sprite.Play("bulb_on");
		else if(RayCount==360 && !IsOn())_sprite.Play("bulb_off");
		else if(RayCount!=360 && IsOn())_sprite.Play("torch_on");
		else _sprite.Play("torch_off");
	}
}
