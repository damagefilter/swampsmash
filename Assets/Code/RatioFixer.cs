using UnityEngine;
using System.Collections;

public class RatioFixer : MonoBehaviour {

	private Camera cam;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cam.aspect != 1.11f) {
			cam.aspect = 1.11f;
		}
	}
}
