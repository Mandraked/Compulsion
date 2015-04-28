using UnityEngine;
using System.Collections;

using UnityStandardAssets.Characters.FirstPerson;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class WatchControl : MonoBehaviour {

	private float prevTime = 0;
	private string message = "";
	public GameObject player = null;
	public GameObject myo = null;
	public AudioSource heart = null;
	private float ang = 0.0f;
	private Vector3 val;

	private Quaternion myoQuat;
	private bool wasUnlocked = false;
	private bool stillPlaying = true;

	public float watchTime = 300.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > MainScript.winTimer) {
			stillPlaying = false;
			message = "You have missed your ride.";
			MainScript.myStyle.normal.textColor = Color.red;
			MainScript.myStyle.fontSize = 30;
		}

		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (Time.time - prevTime > watchTime && stillPlaying) {
			if (thalmicMyo.unlocked) {
				wasUnlocked = true;
				//thalmicMyo.Vibrate(VibrationType.Short);
			}
			if (!heart.isPlaying) heart.Play();
			if (heart.volume<1) heart.volume += (float)(Time.time-prevTime-watchTime)*0.001f;

			shakeScreen((float)(Time.time-prevTime-watchTime)*0.05f);
			if (FirstPersonController.m_WalkSpeed > 0) FirstPersonController.m_WalkSpeed -= (float)(Time.time-prevTime-watchTime)*.0005f;

			thalmicMyo.Unlock(UnlockType.Hold);
			message = "You must check your watch.";

			Quaternion myoQ = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
                new Vector3 (0, 0, 1)
            );

            val = MainScript.FromQ2(myoQ);

            ang = val.y - MainScript._antiYaw.y;
            //Debug.Log(Mathf.Round(ang));

            if ((Mathf.Abs(ang - 90) < 15 || Mathf.Abs(ang+270) < 15) && thalmicMyo.pose == Pose.Fist) {
            	prevTime = Time.time;
            	if (!wasUnlocked) thalmicMyo.Lock();
            	wasUnlocked = false;
            	heart.volume = 0.0f;
            	heart.Stop();
            	FirstPersonController.m_WalkSpeed = 5;
            }
		} else {
			message = "Watch Time: " + Mathf.Ceil(Time.time-prevTime).ToString();
		}
	}

	void shakeScreen(float shakeAmt) {
		player.transform.Rotate(Random.insideUnitSphere * shakeAmt);
	}

	void OnGUI() {
		GUI.Label(new Rect(MainScript.x2, 10, MainScript.width2, MainScript.height), message, MainScript.myStyle);
	}
}
