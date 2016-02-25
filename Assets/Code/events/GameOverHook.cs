/// <summary>
/// Fires hen the time is up.
/// </summary>
public class GameOverHook : Hook<GameOverHook>
{
	public int KilledMobs {
		get;
		private set;
	}

	public int TimeAlive {
		get;
		private set;
	}

	public GameOverHook (int killedMobs, int timeAlive) {
		this.KilledMobs = killedMobs;
		this.TimeAlive = timeAlive;
	}
}

