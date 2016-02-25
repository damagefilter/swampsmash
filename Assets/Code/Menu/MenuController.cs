using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartLevelOne() {
		new StartGameHook("swamp-level1").Call ();
	}

	public void StartLevelTwo() {
		new StartGameHook("swamp-level2").Call ();
	}

	public void Quit() {
		Application.Quit();
	}
}
