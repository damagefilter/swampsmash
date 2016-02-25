using System;
using UnityEngine;

public class BaseEntity : MonoBehaviour {
	[SerializeField] protected float maxHealth;
	[SerializeField] protected float currentHealth;

	[SerializeField] protected float damageValue;
	[SerializeField] protected float defense;

	public float CurrentHealth {
		get {
			return currentHealth;
		}
	}

	public float DamageValue {
		get {
			return damageValue;
		}
	}

	public float Defense {
		get {
			return defense;
		}
	}

	public Vector3 Position {
		get {
			return this.transform.position;
		}
		set {
			this.transform.position = value;
		}
	}

    public Rigidbody2D Rigidbody {
        get {
            return this.GetComponent<Rigidbody2D>();
        }
    }

	public virtual void RemoveHealth(float value) {
		Debug.Log ("Removing " + value + " health from entity");
		this.currentHealth -= value;
		if (this.currentHealth <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void AddHealth(float value) {
		this.currentHealth += value;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
	}

    public virtual void UpdateMovement(Vector2 movement) {
        var rb = this.GetComponent<Rigidbody2D>();
        var vel = rb.velocity;
        vel.x = movement.x;
        rb.velocity = vel;
        this.GetComponent<Rigidbody2D>().AddForce(movement.normalized);
    }
}

