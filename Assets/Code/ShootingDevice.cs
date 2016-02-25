using UnityEngine;
using System.Collections;

public class ShootingDevice : MonoBehaviour {

	[SerializeField] private float bulletForce;
	[SerializeField] private float bulletTorque;
	[SerializeField] private float rof;
	[SerializeField] private GameObject bulletTemplate;
	[SerializeField] private AudioClip shootingSound;
	[SerializeField] private BaseEntity owner;

	private float lastShot;

	public void Shoot(Vector2 direction) {
		if (lastShot + rof > Time.time) {
			return;
		}
		Debug.DrawRay(this.transform.position, direction, Color.magenta);
		lastShot = Time.time;
		var go = Instantiate(bulletTemplate, this.transform.position, Quaternion.identity) as GameObject;
		var damageDealer = go.GetComponentInChildren<RangedDamageDealer>();
		var myCollider = damageDealer.transform.root.gameObject.GetComponentInChildren<Collider2D>();

		foreach (var collider in owner.transform.root.gameObject.GetComponentsInChildren<Collider2D>()) {
			Physics2D.IgnoreCollision(myCollider, collider);
		}

		if (damageDealer == null) {
			Debug.LogError("No DamageDealer on bullet!");
			Destroy (go);
		}
		else {
			damageDealer.Owner = this.owner;
			Debug.DrawRay(this.transform.position, direction, Color.magenta, 5);
			damageDealer.Trigger(direction * (bulletForce * 3.24f), bulletTorque);
		}
	}

	private void PlaySound() {
		var sound = new GameObject();
		sound.transform.position = owner.Position;
		var source = sound.AddComponent<AudioSource>();
		source.clip = shootingSound;
		source.volume = 0.28f;
		source.Play();
		Destroy (sound, source.clip.length);
	}
}
