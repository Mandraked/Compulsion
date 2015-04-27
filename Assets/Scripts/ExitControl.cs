using UnityEngine;
using System.Collections;

public class ExitControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (MainScript.allDone) {
			//Debug.Log("Exiting");
			Application.LoadLevel (0);
		}
	}
}
