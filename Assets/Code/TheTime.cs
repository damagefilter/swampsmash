using UnityEngine;
using System.Collections;
using System;

public class TheTime : MonoBehaviour {

	private static TheTime _instance;
	public static TheTime Instance {
		get {
			return _instance;
		}
	}

	[SerializeField] private int initialTime;
	private float timeLeft;

	private float totalTimeElapsed;
	public float TotalTimeElapsed {
		get {
			return totalTimeElapsed;
		}
	}

	public TheTime() {
		_instance = this;
	}

	public string FormattedTimeLeft {
		get {
			var ts = new TimeSpan(0,0,(int)timeLeft);
			return string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
		}
	}
	// Use this for initialization
	void Start () {
		timeLeft = initialTime;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= 1 * Time.deltaTime;
		this.totalTimeElapsed += 1 * Time.deltaTime;
		if (timeLeft <= 0f) {
			new GameOverHook(MobCounter.Instance.TotalMobsKilled, (int)this.totalTimeElapsed).Call();
		}
	}

	public void AddTime(float time) {
		timeLeft += time;
	}
}
