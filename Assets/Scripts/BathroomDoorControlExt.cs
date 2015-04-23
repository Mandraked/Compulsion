using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class BathroomDoorControlExt : MonoBehaviour {

	public GameObject myo = null;
	public GameObject doorOne = null;
	public GameObject doorTwo = null;

	public float closedX = -197.37f;
	public float closedZ = -241.92f;

	public float openX = -199.23f;
	public float openZ = -240.04f;

	public bool useX = true;

	private bool isOpenX = false;
	private bool isOpenZ = false;
	private bool isCloseX = false;
	private bool isCloseZ = false;

	ThalmicMyo thalmicMyo = null;

	private bool unlocked = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked) {
			if (thalmicMyo.pose == Pose.WaveOut) {
				if (useX) {
					isOpenX = true;
					isCloseX = false;
				} else {
					isOpenZ = true;
					isCloseZ = false;
				}
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				if (useX) {
					isCloseX = true;
					isOpenX = false;
				} else {
					isCloseZ = true;
					isOpenZ = false;
				}
			}
		}

		if (isOpenX) {
			if (doorOne.transform.position.x > openX) {
				doorOne.transform.Translate(-Time.deltaTime, 0, 0, null);
				doorTwo.transform.Translate(-Time.deltaTime, 0, 0, null);
			} else {
				isOpenX = false;
			}
		} else if (isCloseX) {
			if (doorOne.transform.position.x < closedX) {
				doorOne.transform.Translate(Time.deltaTime, 0, 0, null);
				doorTwo.transform.Translate(Time.deltaTime, 0, 0, null);
			} else {
				isCloseX = false;
			}
		}
		if (isOpenZ) {
			if (doorOne.transform.position.z < openZ) {
				doorOne.transform.Translate(0, 0, Time.deltaTime, null);
				doorTwo.transform.Translate(0, 0, Time.deltaTime, null);
			} else {
				isOpenZ = false;
			}
		} else if (isCloseZ) {
			if (doorOne.transform.position.z > closedZ) {
				doorOne.transform.Translate(0, 0, -Time.deltaTime, null);
				doorTwo.transform.Translate(0, 0, -Time.deltaTime, null);
			} else {
				isCloseZ = false;
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
