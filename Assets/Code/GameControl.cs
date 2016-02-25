using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	#region Singleton
	private static GameControl _instance;
	public static GameControl Instance {
		get {
			return _instance;
		}
	}
	#endregion
	private string loadedLevel;
	private string lastLoadedLevel;
	public string LevelName {
		get {
			return lastLoadedLevel;
		}
	}
	private float timeElapsed;
	private int mobsKilled;

	void Awake() {
		if (_instance != null) {
			Destroy (this.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		EventDispatcher.Instance.Register<GameOverHook>(OnGameOver);
		EventDispatcher.Instance.Register<StartGameHook>(OnStartGame);
	}

	void OnLevelWasLoaded(int index) {
		lastLoadedLevel = this.loadedLevel;
		this.loadedLevel = Application.loadedLevelName;
	}

	public int GetScore() {
		// Something like that ... lol
		return (int)(this.timeElapsed / 2) * mobsKilled;
	}

	public void BackToMainMenu() {
		Application.LoadLevel("MainMenu");
		BackgroundMusicMixer.Instance.CrossfadeTwoToOne();
	}
	
	/// <summary>
	/// This happens only inside a game level.
	/// The only next possible step is to go to the highscore screen
	/// </summary>
	/// <param name="hook">Hook.</param>
	private void OnGameOver(GameOverHook hook) {
		this.timeElapsed = hook.TimeAlive;
		this.mobsKilled = hook.KilledMobs;
		Application.LoadLevel("Highscores");
		BackgroundMusicMixer.Instance.CrossfadeTwoToOne();
	}

	private void OnStartGame(StartGameHook hook) {
		Application.LoadLevel(hook.LevelName);
		BackgroundMusicMixer.Instance.CrossfadeOneToTwo();
	}
}
