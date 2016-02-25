using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup {
	// Generate a random time number between two constants
	[SerializeField] private float randomHealthFrom;
	[SerializeField] private float randomHealthTo;

	private float finalHealth;

	public void Start() {
		finalHealth = UnityEngine.Random.Range(randomHealthFrom, randomHealthTo);
	}

	#region implemented abstract members of Pickup

	public override void OnPickUp (BaseEntity entity) {
		entity.AddHealth(finalHealth);
	}

	#endregion
}
