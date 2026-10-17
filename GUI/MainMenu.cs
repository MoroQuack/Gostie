using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		// 1. Find the buttons inside the VBoxContainer
		Button play = GetNode<Button>("VBoxContainer/PlayButton");
		Button levels = GetNode<Button>("VBoxContainer/LevelsButton");

		// 2. Connect the physical click event to our custom methods
		play.Pressed += OnPlayPressed;
		levels.Pressed += OnLevelsPressed;
	}

	private void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://Levels/level1.tscn");
	}

	private void OnLevelsPressed()
	{
		GetTree().ChangeSceneToFile("res://GUI/levels_menu.tscn");
	}
}
