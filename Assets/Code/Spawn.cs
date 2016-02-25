using UnityEngine;
using System.Collections;

/// <summary>
/// Used by the mob spawner.
/// </summary>
/// 
public class Spawn : MonoBehaviour {

	private MobSpawner spawner;

	public void SetMobSpawner(MobSpawner spawner) {
		this.spawner = spawner;
	}

	public void OnDestroy() {
		spawner.OneDown();
	}
}
