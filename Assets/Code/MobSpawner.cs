using UnityEngine;
using System.Collections;

/// <summary>
/// A spawner of Mobs
/// </summary>
public class MobSpawner : MonoBehaviour {

	[SerializeField] private int maxSpawnableMobs = 10;
	[SerializeField] private float spawnRate;
	[SerializeField] private float spawnRadius;
	[SerializeField] private GameObject mobPrefab;
	[SerializeField] private TriggerSensor playerSensor;
	private bool stopSpawning;

	private int currentlySpawnedMobs;
	private float lastSpawn;

	public void Start() {
		playerSensor.SetCollisionEnterCallback(TriggerEnter);
		playerSensor.SetCollisionExitCallback(TriggerExit);
	}

	public void Update() {
		if (stopSpawning) {
			return;
		}
		if (lastSpawn + spawnRate <= Time.time && currentlySpawnedMobs < maxSpawnableMobs) {
			lastSpawn = Time.time;

			var pos = this.transform.position;
			float angle = UnityEngine.Random.Range(-180, 180); // Results in a 360 degree arc
			pos.x = pos.x + spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
			pos.y = pos.y + spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
			pos.z = 0;

			var go = (GameObject)Instantiate(mobPrefab, pos, Quaternion.identity);
			var spawn = go.AddComponent<Spawn>();
			spawn.SetMobSpawner(this);
			currentlySpawnedMobs++;
		}
	}

	public void OneDown() {
		currentlySpawnedMobs--;
		if (currentlySpawnedMobs < 0) {
			currentlySpawnedMobs = 0;
		}
	}

	private void TriggerEnter(Collider2D other) {
		stopSpawning = true;
	}

	private void TriggerExit(Collider2D other) {
		stopSpawning = false;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.transform.position, this.spawnRadius);
	}
}
