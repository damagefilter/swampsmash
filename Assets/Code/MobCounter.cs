using UnityEngine;
using System.Collections;

public class MobCounter : MonoBehaviour {

	private static MobCounter _instance;
	public static MobCounter Instance {
		get {
			return _instance;
		}
	}
	private int totalMobs;
	public int TotalMobsKilled {
		get {
			return totalMobs;
		}
	}
	private int mobsAlive;
	public int MobsAlive {
		get {
			return mobsAlive;
		}
	}

	public MobCounter() {
		_instance = this;
	}

	public void PlusOneToAlive() {
		++mobsAlive;
	}

	public void MinusOneToAlive() {
		--mobsAlive;
		++totalMobs;
	}

	public void OnDestroy() {
		_instance = null;
	}
}
