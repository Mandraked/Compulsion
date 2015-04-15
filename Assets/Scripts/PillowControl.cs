using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PillowControl : MonoBehaviour {

	public GameObject myo = null;
	public GameObject pillow = null;

	ThalmicMyo thalmicMyo = null;
	private bool unlocked = false;
	private bool hasVibrated = false;
	private float val = 0.0f;
	private float calculatedVal = 0.0f;
	private float currentAng = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked) {
			if (thalmicMyo.pose == Pose.Fist) {
				Vector3 rollAmt = computeZeroRollVector (myo.transform.forward);
            	val = MainScript.rollFromZero (rollAmt, myo.transform.forward, myo.transform.up);
				calculatedVal = val - MainScript._referenceRoll;

				pillow.transform.Rotate(calculatedVal * 0.1f, 0.0f, 0.0f, 0);
				currentAng = pillow.transform.eulerAngles.x;
				//Debug.Log(currentAng-308);
				if (Mathf.Abs(currentAng - 308) < 10 && !hasVibrated) {
					thalmicMyo.Vibrate(VibrationType.Short);
					hasVibrated = true;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		thalmicMyo.Unlock(UnlockType.Hold);
		unlocked = true;
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock();
		}
	}

	Vector3 computeZeroRollVector (Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross (m, myo.transform.forward);

        return roll.normalized;
    }
}
