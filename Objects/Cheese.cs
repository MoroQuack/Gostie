using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;


public partial class Cheese : Area2D
{
	[Export] public string nextScene = "level1.tscn";
	[Export] public int currentLevel;
	private AudioStreamPlayer _audioPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private bool _alreadyWon = false;
	private async void _on_body_entered(Node2D body)
	{
		if(_alreadyWon)return;
		if (body.IsInGroup("player"))
		{
			Win();
			GD.Print("player won!");
			if(SaveDataManager.Instance.Data.FinishedLevel < currentLevel){
				SaveDataManager.Instance.Data.FinishedLevel = currentLevel;
			}
			SaveDataManager.Instance.SaveGame();
			
		}
		
	}

	private async void Win()
	{
		_alreadyWon=true;
		_audioPlayer.Play();
		await ToSignal(GetTree().CreateTimer(2.5f), SceneTreeTimer.SignalName.Timeout);
		GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile,"res://Levels/" + nextScene);
	}
}
