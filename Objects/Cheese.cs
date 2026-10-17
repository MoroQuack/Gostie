using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;


public partial class Cheese : Area2D
{
	[Export] public string nextScene = "level1.tscn";
	[Export] public int currentLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("player won!");
			if(SaveDataManager.Instance.Data.FinishedLevel < currentLevel){
				SaveDataManager.Instance.Data.FinishedLevel = currentLevel;
				SaveDataManager.Instance.SaveGame();
			}
			GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile,"res://Levels/" + nextScene);
		}
		
	}
}
