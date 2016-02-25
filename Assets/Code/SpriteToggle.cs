using UnityEngine;
using System.Collections;

public class SpriteToggle : MonoBehaviour {

	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private bool autoTurnOff;
	[SerializeField] private float turnOffDelay;

	float startTime;
	public void Start() {
		startTime = Time.time;
	}

	public void Update() {
		if (autoTurnOff && startTime + turnOffDelay < Time.time) {
			TurnOff ();
		}
	}
	public void TurnOn() {
		var c = sprite.color;
		c.a = 1.0f;
		sprite.color = c;
	}

	public void TurnOff() {
		var c = sprite.color;
		c.a = 0.0f;
		sprite.color = c;
	}

	public void ScheduleToggleOff() {
		startTime = Time.time;
		autoTurnOff = true;
	}
}
