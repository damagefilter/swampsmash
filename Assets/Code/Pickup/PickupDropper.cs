using UnityEngine;
using System.Collections;

public class PickupDropper : MonoBehaviour {
	[SerializeField] private GameObject[] drops;
	[Tooltip("A number from 0 to 100 representing dropchance in %")]
	[SerializeField] private float dropChance;

	public void DropPickups(Vector3 center) {
		float range = UnityEngine.Random.Range (0f, 100f);
		if (range <= dropChance) {
			for (int i = 0; i < drops.Length; ++i) {
				float secondRange = UnityEngine.Random.Range (0f, 100f);
				if (secondRange <= dropChance) {
					center.y += 1;
					var go = (GameObject)Instantiate(drops[i], center, Quaternion.identity);
					
					go.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 530f);
				}
			}
		}
	}
}
