using UnityEngine;
using System.Collections;

public class RangedDamageDealer : DamageDealer {
	[SerializeField] protected float ttl;

	private float startTime;
	[SerializeField] private Rigidbody2D rb;
	public Rigidbody2D Rigidbody {
		get {
			return rb;
		}
	}

	private bool isFlipped;
	public void Start() {
		startTime = Time.time;
		this.Active = true;
	}

	public void Update() {
		if (rb.velocity != Vector2.zero) {
			var lookingat = Quaternion.LookRotation(rb.velocity, Vector2.up);

			this.transform.rotation = new Quaternion(0, 0, lookingat.z, lookingat.w);
			if (rb.velocity.x < 0 && !isFlipped) {
				var scale = this.transform.localScale;
				scale.x *= -1;
				this.transform.localScale = scale;
				isFlipped = true;
			}
			if (rb.velocity.x > 0 && isFlipped) {
				var scale = this.transform.localScale;
				scale.x = Mathf.Abs (scale.x);
				this.transform.localScale = scale;
				isFlipped = false;
			}
		}

		if (startTime + ttl < Time.time) {
			Destroy (this.transform.root.gameObject);
		}
	}

	public void Trigger(Vector2 direction, float torque) {
		if (rb == null) {
			Debug.LogError("No Rigidbody on bullet!");
			Destroy (this.transform.root.gameObject);
		}
		else {
			rb.AddForce(direction);
			rb.AddTorque(torque, ForceMode2D.Impulse);
		}
	}

	protected override void HandleDamaging (Collider2D other) {
		// Look from the root down as the entity might be attached to a parent object
		var entity = other.transform.root.gameObject.GetComponentInChildren<BaseEntity> ();
		if (entity == null || entity == owner) {
			return;
		}
		
		entity.RemoveHealth (Mathf.Max(0, (owner.DamageValue * damageMultiplier) - entity.Defense));
		this.Active = false;
		Destroy (this.transform.root.gameObject); // since this is a projectile, we gonna go down after we did some damage
	}
}