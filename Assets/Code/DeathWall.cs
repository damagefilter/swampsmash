using UnityEngine;
using System.Collections;

public class DeathWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.isTrigger) {
            return; // Well that can happen ...
        }
		var ent = collider.transform.root.GetComponentInChildren<BaseEntity>();
		if (ent != null) {
			ent.RemoveHealth(9999999999); // definitely kill
		}
	}
}
