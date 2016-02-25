using UnityEngine;
using System.Collections;

public class TimePickup : Pickup {
	// Generate a random time number between two constants
	[SerializeField] private float randomTimeFrom;
	[SerializeField] private float randomTimeTo;

	private float finalTimeAmount;

	public void Start() {
		finalTimeAmount = UnityEngine.Random.Range(randomTimeFrom, randomTimeTo);
	}

	#region implemented abstract members of Pickup

	public override void OnPickUp (BaseEntity entity) {
		TheTime.Instance.AddTime(finalTimeAmount);
	}

	#endregion
}
