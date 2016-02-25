using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class CollisionSensor : MonoBehaviour {

	public string layerName;

	private Collider2D myCollider;
	private Action<Collision2D> collisionEnter;
	private Action<Collision2D> collisionExit;

	public void SetCollisionEnterCallback(Action<Collision2D> action) {
		this.collisionEnter = action;
	}

	public void SetCollisionExitCallback(Action<Collision2D> action) {
		this.collisionExit = action;
	}

	public void OnCollisionEnter2D(Collision2D other) {
		if (this.collisionEnter == null) {
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer(layerName)) {
			this.collisionEnter(other);
		}
	}
	
	public void OnCollisionExit2D(Collision2D other) {
		if (this.collisionExit == null) {
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer(layerName)) {
			this.collisionExit(other);
		}
	}

	public Collider2D GetCollider() {
		if (myCollider == null) {
			myCollider = GetComponent<Collider2D>();
		}
		return myCollider;
	}
}
