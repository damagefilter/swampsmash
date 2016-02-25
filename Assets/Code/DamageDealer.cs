using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to things that need to do damage on collisions.
/// This requires the collider to be set up as trigger.
/// </summary>
public class DamageDealer : MonoBehaviour {
	[SerializeField] protected float damageMultiplier;

	[Tooltip("The rate of fire value determines how often this thing does damage OnTriggerStay")]
	[SerializeField] protected float rof;

	[SerializeField] protected BaseEntity owner;

	private float lastDamage;

	public bool Active {
		get;
		set;
	}

	public BaseEntity Owner {
		get {
			return owner;
		}
		set {
			owner = value;
		}
	}


	
	public void OnTriggerEnter2D(Collider2D other) {
		if (Active) {
			HandleDamaging (other);
		}
	}

	public void OnTriggerStay2D(Collider2D other) {
		if (Active) {
			HandleDamaging (other);
		}
	}

	protected virtual void HandleDamaging (Collider2D other)
	{
		if (lastDamage + rof <= Time.time) {
			var entity = other.gameObject.GetComponentInChildren<BaseEntity> ();
			if (entity == null) {
				return;
			}
			entity.RemoveHealth ((owner.DamageValue * damageMultiplier) - entity.Defense);
			this.lastDamage = Time.time;
		}
	}
}
