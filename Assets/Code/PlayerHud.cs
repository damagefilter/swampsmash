using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHud : MonoBehaviour {

	[SerializeField] BaseEntity playerRef;
	[SerializeField] MobCounter mobCounter;
	[SerializeField] TheTime theTime;
	[SerializeField] Text health;
	[SerializeField] Text time;
	[SerializeField] Text mobsLeft;
	
	// Update is called once per frame
	void Update () {
		health.text = ((int)playerRef.CurrentHealth).ToString();
		time.text = theTime.FormattedTimeLeft;
		mobsLeft.text = mobCounter.MobsAlive.ToString();
	}
}
