using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class WaterControl : MonoBehaviour {
	public ParticleSystem waterSystem;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;
	private bool unlocked = false;
	private Pose _lastPose = Pose.Unknown;

	// Use this for initialization
	void Start () {
		waterSystem.enableEmission = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked)
		{
			if (thalmicMyo.pose == Pose.WaveIn && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	Debug.Log("Wave In");
				waterSystem.enableEmission = true;
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	Debug.Log("Wave Out");
				waterSystem.enableEmission = false;
			}
			else if (thalmicMyo.pose == Pose.DoubleTap && _lastPose != thalmicMyo.pose) {
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
