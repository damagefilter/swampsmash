using UnityEngine;
using System.Collections;

/// <summary>
/// This flips the gravity for the specified PlayerMovement while it collides with the attached collider.
/// When the collision exits, the effect is reversed back to normal gravity.
/// </summary>
public class GravityFlip : MonoBehaviour {
	[Tooltip("The handled PlayerMovement controller")]
	public PlayerMovement handle;
	[Tooltip("Ticked: Gravity reversed. Unticked: Gravity normal. Works on collision enter.")]
	public bool flipGravity = true;

	public CollisionSensor head;
	public CollisionSensor feet;

	private GameObject lastExited;

	public void Start() {
		head.SetCollisionEnterCallback(HeadEnterCollision);
		head.SetCollisionExitCallback(FeetExitCollision);
	}
	public void HeadEnterCollision(Collision2D other) {
		if (CanFlip(head, other)) {
			// Avoid double-collision during one frame approximately
			if (lastExited == other.gameObject) {
				lastExited = null;
				return;
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("World") && !this.handle.HasGravityFlipped) {
				if (flipGravity) {
					handle.FlipGravity();
				}
				else {
					handle.RevertGravity();
				}
			}
		}
	}
	
	public void FeetExitCollision(Collision2D other) {
		if (this.handle.HasGravityFlipped) {
			this.lastExited = other.gameObject;
			if (flipGravity) {
				handle.RevertGravity();
			}
		}
	}

	private bool CanFlip(CollisionSensor sensor, Collision2D other) {
		if (!handle.InAir) {
			return false;
		}
		Vector2 contactPoint = other.contacts[0].point;
		Vector2 center = sensor.GetCollider().bounds.center;

//		Debug.DrawLine(contactPoint, center, Color.red, 10f);
//		Vector3 pointB = center - contactPoint;
//		float angle = Vector2.Angle(contactPoint, center);
		bool top = contactPoint.y > center.y && Mathf.Abs (handle.VerticalVelocity - 0.5f) > 0.25f;
		return top;
	}
}
