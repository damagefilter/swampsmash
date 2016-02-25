using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HighscoreUI : MonoBehaviour {
	[SerializeField] private bool onMainMenu;
	// All should be 3 or 10 elements.
	[SerializeField] private Text[] scoreList;
	[SerializeField] private Text[] nameList;
	[SerializeField] private Text[] timeList;
	[SerializeField] private Text[] levelList;
	[SerializeField] private SpriteToggle spriteToggle;

	[SerializeField] private InputField playerName;
	[SerializeField] private Text score;
	private HighscoresList highscoreData;
	// Use this for initialization
	void Start () {
		highscoreData = new HighscoresList();
		highscoreData.Load();
		List<Highscore> topTen;
		int count;
		if (onMainMenu) {
			count = 3;
			topTen = highscoreData.GetTopThree();
		}
		else {
			count = 10;
			topTen = highscoreData.GetTopTen();
		}

		for (int i = 0; i < count; ++i) {
			if (topTen.Count > i) {
				scoreList[i].text = topTen[i].Score.ToString();
				nameList[i].text = topTen[i].PlayerName;
				if (!onMainMenu) {
					timeList[i].text = topTen[i].FormattedTimestamp;
					levelList[i].text = topTen[i].LevelName;
				}


			}
			else {
				scoreList[i].text = string.Empty;
				nameList[i].text = string.Empty;
				if (!onMainMenu) {
					timeList[i].text = string.Empty;
					levelList[i].text = string.Empty;
				}
			}

		}
		// On main menuthere is no score and stuff, just the list of highscores.
		if (!onMainMenu) {
			var gc = GameControl.Instance;
			var scoreNum = gc.GetScore();
			score.text = scoreNum.ToString();
		}

	}

	public void OnSubmitScore() {
		var gc = GameControl.Instance;
		var scoreNum = gc.GetScore();
		highscoreData.AddNewScore(scoreNum, gc.LevelName, playerName.text);
		highscoreData.Save();
		gc.BackToMainMenu();
	}

	public void OnBackToMainMenu() {
		var gc = GameControl.Instance;
		gc.BackToMainMenu();
	}

	public void OnShowCredits() {
		this.spriteToggle.TurnOn();
		this.spriteToggle.ScheduleToggleOff();
	}
}
