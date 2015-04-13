using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class LightControl : MonoBehaviour {
	public Light myLight;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;
	private bool unlocked = false;
	private Pose _lastPose = Pose.Unknown;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked)
		{
			if (thalmicMyo.pose == Pose.WaveIn && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	Debug.Log("Wave In");
				GameObject obj = GameObject.FindWithTag ("LightSwitch");
				float val = 0.3f;
				Vector3 vec = new Vector3 (obj.transform.localScale.x, val, obj.transform.localScale.z);
				obj.transform.localScale = vec;
				myLight.enabled = false;
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	Debug.Log("Wave Out");
				GameObject obj = GameObject.FindWithTag ("LightSwitch");
				float val = -0.3f;
				Vector3 vec = new Vector3 (obj.transform.localScale.x, val, obj.transform.localScale.z);
				obj.transform.localScale = vec;
				myLight.enabled = true;
			}
			else if (thalmicMyo.pose == Pose.Fist && _lastPose != thalmicMyo.pose) {
				unlocked = false;
				thalmicMyo.Lock ();
        		Debug.Log("Locking");
			}
		}
	}

	void OnTriggerEnter(Collider other) {
        thalmicMyo.Unlock (UnlockType.Hold);
        Debug.Log("Unlocked");

		unlocked = true;
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock ();
	        Debug.Log("Locking");
		}
	}
}
