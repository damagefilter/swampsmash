using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class TriggerSensor : MonoBehaviour {

	public string layerName;

	private Collider2D myCollider;
	private Action<Collider2D> collisionEnter;
	private Action<Collider2D> collisionExit;

	public void SetCollisionEnterCallback(Action<Collider2D> action) {
		this.collisionEnter = action;
	}

	public void SetCollisionExitCallback(Action<Collider2D> action) {
		this.collisionExit = action;
	}

	public void OnTriggerEnter2D(Collider2D other) {
 		if (this.collisionEnter == null) {
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer(layerName)) {
			this.collisionEnter(other);
		}
	}
	
	public void OnTriggerExit2D(Collider2D other) {
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
