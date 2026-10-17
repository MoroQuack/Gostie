using System;

public sealed class GlobalData
{
	
	public int NumberOfLevels {get;} = 16;
	private static readonly Lazy<GlobalData> _instance = 
		new Lazy<GlobalData>(() => new GlobalData());

	// Global access point
	public static GlobalData Instance => _instance.Value;

	private GlobalData()
	{
		
	}

}
