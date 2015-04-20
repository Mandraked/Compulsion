using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class BathroomDoorControlIn : MonoBehaviour {

	public GameObject myo = null;
	public GameObject doorOne = null;
	public GameObject doorTwo = null;

	public float closedX = -197.37f;
	public float closedZ = -241.92f;

	public float openX = -199.23f;
	public float openZ = -240.04f;

	public bool useX = true;

	ThalmicMyo thalmicMyo = null;

	private bool unlocked = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked) {
			if (/*thalmicMyo.pose == Pose.WaveIn*/ true) {
				if (useX) {
					if (doorOne.transform.position.x > openX) {
						doorOne.transform.Translate(-Time.deltaTime, 0, 0, null);
						doorTwo.transform.Translate(-Time.deltaTime, 0, 0, null);
					}
				} else {
					if (doorOne.transform.position.z < openZ) {
						doorOne.transform.Translate(0, 0, Time.deltaTime, null);
						doorTwo.transform.Translate(0, 0, Time.deltaTime, null);
					}
				}
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				if (useX) {
					if (doorOne.transform.position.x < closedX) {
						doorOne.transform.Translate(-Time.deltaTime, 0, 0, null);
						doorTwo.transform.Translate(-Time.deltaTime, 0, 0, null);
					}
				} else {
					if (doorOne.transform.position.z > closedZ) {
						doorOne.transform.Translate(0, 0, -Time.deltaTime, null);
						doorTwo.transform.Translate(0, 0, -Time.deltaTime, null);
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		unlocked = true;
		thalmicMyo.Unlock(UnlockType.Hold);
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock();
		}
	}
}
