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
	public AudioSource src = null;
	private string message = "";

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
	        	//Debug.Log("Wave In");
				waterSystem.enableEmission = true;
				src.Play();
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	//Debug.Log("Wave Out");
				waterSystem.enableEmission = false;
				src.Pause();
			}
			else if (thalmicMyo.pose == Pose.DoubleTap && _lastPose != thalmicMyo.pose) {
				unlocked = false;
				thalmicMyo.Lock ();
        		//Debug.Log("Locking");
			}
		}
	}

	void OnGUI() {
        GUI.Label(new Rect(0, 0, 250, 200), message);
    }

	void OnTriggerEnter(Collider other) {
        thalmicMyo.Unlock (UnlockType.Hold);
        //Debug.Log("Unlocked");
		unlocked = true;
		message = "Turn the faucet on by waving in \nor off by waving out.";
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock ();
	        //Debug.Log("Locking");
		}
		message = "";
	}
}
