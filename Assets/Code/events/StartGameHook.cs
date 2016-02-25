/// <summary>
/// Fires hen the time is up.
/// </summary>
public class StartGameHook : Hook<StartGameHook>
{
	public string LevelName {
		get;
		private set;
	}
	public StartGameHook (string levelName) {
		this.LevelName = levelName;
	}
}

