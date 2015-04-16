using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class WatchControl : MonoBehaviour {

	private float prevTime = 0;
	private string message = "";
	public GameObject myo = null;
	private float ang = 0.0f;
	private Vector3 val;

	private Quaternion myoQuat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (Time.time - prevTime > 10) {
			thalmicMyo.Unlock(UnlockType.Hold);
			message = "You must check your watch.";

			Quaternion myoQ = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
                new Vector3 (0, 0, 1)
            );

            val = MainScript.FromQ2(myoQ);

            Debug.Log("anti: " + MainScript._antiYaw.y);
            Debug.Log("val: " + val.y);

            ang = val.y - MainScript._antiYaw.y;
            Debug.Log(Mathf.Round(ang));

            if (Mathf.Abs(ang - 90) < 15 && thalmicMyo.pose == Pose.Fist) {
            	prevTime = Time.time;
            	thalmicMyo.Lock();
            	Debug.Log("here");
            }
		} else {
			message = "Watch Time: " + Mathf.Ceil(Time.time-prevTime).ToString();
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(MainScript.x, 10, MainScript.width, MainScript.height), message);
	}
}
