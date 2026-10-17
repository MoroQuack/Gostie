using Godot;
using System;
using System.Text.Json;


public class GameData
{
	public int FinishedLevel { get; set; } = 1;
	//public float MasterVolume { get; set; } = 0.0f;
}

public sealed class SaveDataManager
{
	private readonly string _savePath = "res://DataSaving/savedata.json";
	public GameData Data = new GameData(); 
	private static readonly Lazy<SaveDataManager> _instance = 
		new Lazy<SaveDataManager>(() => new SaveDataManager());

	// Global access point
	public static SaveDataManager Instance => _instance.Value;

	private SaveDataManager()
	{
		LoadGame();
	}

	/*public void DoSomething()
	{
		GD.Print("Level unlocked: " + Data.UnlockedLevel.ToString() );
	}*/
	public void SaveGame()
	{
		string jsonString = JsonSerializer.Serialize(Data);
		using var file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);
		GD.Print("Game Saved!");
	}

	private void LoadGame()
	{
		if (FileAccess.FileExists(_savePath))
		{
			using var file = FileAccess.Open(_savePath, FileAccess.ModeFlags.Read);
			string jsonString = file.GetAsText();
			Data = JsonSerializer.Deserialize<GameData>(jsonString);
			GD.Print("Game Loaded!");
		}
	}
}
