using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class LightControl : MonoBehaviour {
	public Light myLight;
	public GameObject lSwitch = null;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;
	private bool unlocked = false;
	private Pose _lastPose = Pose.Unknown;
	public float startingY = 0.3f;
	private string message = "";

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
	        	//Debug.Log("Wave In");
				Vector3 vec = new Vector3 (lSwitch.transform.localScale.x, -1 * startingY, lSwitch.transform.localScale.z);
				lSwitch.transform.localScale = vec;
				myLight.enabled = false;
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
	        	//Debug.Log("Wave Out");
				Vector3 vec = new Vector3 (lSwitch.transform.localScale.x, startingY, lSwitch.transform.localScale.z);
				lSwitch.transform.localScale = vec;
				myLight.enabled = true;
			}
			else if (thalmicMyo.pose == Pose.DoubleTap && _lastPose != thalmicMyo.pose) {
				unlocked = false;
				thalmicMyo.Lock ();
        		//Debug.Log("Locking");
			}
		}
	}

	void OnGUI() {
        GUI.Label(new Rect(10, 10, 250, 200), message);
    }

	void OnTriggerEnter(Collider other) {
        thalmicMyo.Unlock (UnlockType.Hold);
        //Debug.Log("Unlocked");
		unlocked = true;
		message = "Turn the light off by waving in \nor turn it on by waving out.";
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
