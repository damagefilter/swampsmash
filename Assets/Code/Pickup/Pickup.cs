using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour {

	/// <summary>
	/// Apply this pickup.
	/// Method gets an entity as context argument.
	/// Every entity can pick this up.
	/// 
	/// </summary>
	/// <param name="entity">Entity.</param>
	public abstract void OnPickUp(BaseEntity entity);


	public void OnCollisionEnter2D(Collision2D other) {
		var entity = other.transform.root.gameObject.GetComponentInChildren<BaseEntity>();

		if (entity != null) {
			if (entity.GetType() != typeof(PlayerEntity)) {
				return;
			}
			OnPickUp(entity);
			Destroy (this.gameObject);
		}
	}
}
