using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		
		Button play = GetNode<Button>("VBoxContainer/PlayButton");
		Button levels = GetNode<Button>("VBoxContainer/LevelsButton");
		Button settings = GetNode<Button>("VBoxContainer/SettingsButton");

		play.Pressed += OnPlayPressed;
		levels.Pressed += OnLevelsPressed;
		settings.Pressed += OnSettingsPressed;

	}

	private void OnPlayPressed()
	{
		int nextLevel = SaveDataManager.Instance.Data.FinishedLevel + 1;
		if (nextLevel > GlobalData.Instance.NumberOfLevels)
		{
			nextLevel = GlobalData.Instance.NumberOfLevels;
		}

		var music = GetNode<AudioStreamPlayer>("/root/BackgroundMusic");
		music.Play();
		
		GetTree().ChangeSceneToFile("res://Levels/level" + nextLevel.ToString() + ".tscn");
	}

	private void OnLevelsPressed()
	{
		GetTree().ChangeSceneToFile("res://GUI/levels_menu.tscn");
	}

	private void OnSettingsPressed()
	{
		GetTree().ChangeSceneToFile("res://GUI/settings_menu.tscn");
	}
}
