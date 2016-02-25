using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Sword behaviour.
/// Used as is and on blob mob to rotate the bow
/// </summary>
public class SwordBehaviour : MonoBehaviour {
	[SerializeField] private Transform centerPoint;
	[SerializeField] private AudioClip swingSound;

	[SerializeField] private DamageDealer swordDamageDealer;
	[SerializeField] private Animator animator;

	[SerializeField] private float swingDistance = 2.5f;
	[SerializeField] private float swingTime = 2.5f;


	/// <summary>
	/// Orbits the sword swing sprite around the given center point (that's for instance the player)
	/// </summary>
	public void MoveSpriteTo (Vector3 pos) {
		if (pos == Vector3.zero) {
			return; // Don't change if nothing changed
		}
		// Get Angle in Radians
		float AngleRad = Mathf.Atan2 (pos.y, pos.x);
		// Get Angle in Degrees
		float AngleDeg = (180 / Mathf.PI) * AngleRad;
		// Rotate Object
		this.transform.rotation = Quaternion.Euler (0, 0, AngleDeg + 180f);

	}

	/// <summary>
	/// Moves the sword colliderforward
	/// </summary>
	public void SwingSword() {
		PlaySound ();
		StartCoroutine("InternalSwingSword");
	}

	public IEnumerator InternalSwingSword() {
		animator.SetTrigger("Attack");
		swordDamageDealer.Active = true;
		Ray2D r = new Ray2D(centerPoint.position, swordDamageDealer.transform.rotation * Vector2.up);
		var currentTarget = r.GetPoint(this.swingDistance);
		var newPos = Vector3.MoveTowards(swordDamageDealer.transform.position, currentTarget, swingTime * Time.deltaTime);

		while (!(Vector3.Distance(newPos, currentTarget) <= 0.01f)) {
			r = new Ray2D(centerPoint.position, swordDamageDealer.transform.rotation * Vector2.up);
			currentTarget = r.GetPoint(this.swingDistance);
			newPos = Vector3.MoveTowards(swordDamageDealer.transform.position, currentTarget, swingTime * Time.deltaTime);

			swordDamageDealer.transform.position = newPos;
			yield return null;
		}
		swordDamageDealer.transform.position = centerPoint.transform.position;
		swordDamageDealer.Active = false;
	}

	private void PlaySound() {
		var sound = new GameObject();
		sound.transform.position = centerPoint.position;
		var source = sound.AddComponent<AudioSource>();
		source.clip = swingSound;
		source.volume = 0.28f;
		source.Play();
		Destroy (sound, source.clip.length);
	}
}
