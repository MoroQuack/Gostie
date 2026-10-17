using Godot;

public partial class GlobalInputs : Node
{
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("full_screen"))
		{
			var currentMode = DisplayServer.WindowGetMode();

			if (currentMode == DisplayServer.WindowMode.Fullscreen){
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			}else{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			}
		}else if (@event.IsActionPressed("back_to_menu"))
		{
			GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile,"res://GUI/main_menu.tscn");
		}
	}
}
