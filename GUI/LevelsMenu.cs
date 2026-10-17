using Godot;
using System;

public partial class LevelsMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private int levelNumber = 5;
	private VBoxContainer _container;
	public override void _Ready()
	{
		int maxLevelUnlocked = SaveDataManager.Instance.Data.FinishedLevel + 1;
		_container = GetNode<VBoxContainer>("VBoxContainer");
		for(int i = 1; i <= levelNumber; i++)
		{
			Button newButton = new Button();
			newButton.Text = i.ToString();
			newButton.CustomMinimumSize = new Vector2(100,100);
			int level = i;
			if(level <= maxLevelUnlocked)newButton.Pressed += () => ButtonPressed(level);
			else newButton.Disabled = true;
			_container.AddChild(newButton);
		}
	}

	public void ButtonPressed(int level)
	{
		var music = GetNode<AudioStreamPlayer>("/root/BackgroundMusic");
		music.Play();
		GetTree().ChangeSceneToFile("res://Levels/level" + level.ToString() + ".tscn");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
