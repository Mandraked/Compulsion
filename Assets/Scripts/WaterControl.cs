using UnityEngine;
using System.Collections;

using UnityStandardAssets.Characters.FirstPerson;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class WaterControl : MonoBehaviour {
	public ParticleSystem waterSystem;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;
	private bool unlocked = false;
	private Pose _lastPose = Pose.Unknown;
	public AudioSource src = null;
	//private string message = "";

	private int magicNumber = 0;
	private int magicCounter = 0;
	private bool waterOn = false;


	// Use this for initialization
	void Start () {
		waterSystem.enableEmission = false;
		magicNumber = (int) Mathf.Floor(Random.Range(2.0f, 5.9f));
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked)
		{
			if (thalmicMyo.pose == Pose.WaveIn && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
				waterSystem.enableEmission = true;
				src.Play();
				waterOn = true;
				FirstPersonController.m_WalkSpeed = 0;
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
				waterSystem.enableEmission = false;
				src.Pause();

				if (waterOn) {
					waterOn = false;
					magicCounter++;
					if (magicCounter % magicNumber != 0) {
						MainScript.showNumber(magicCounter.ToString());
						MainScript.waterDone = false;						
					} else {
						MainScript.showNumber("");
						magicCounter = 0;
						FirstPersonController.m_WalkSpeed = 5;
						MainScript.waterDone = true;
					}
				}
			}
		}
	}

	void OnGUI() {
        //GUI.Label(new Rect(10, 10, 250, 200), message);
    }

	void OnTriggerEnter(Collider other) {
        thalmicMyo.Unlock (UnlockType.Hold);
        //Debug.Log("Unlocked");
		unlocked = true;
		//message = "Turn the faucet on by waving in \nor off by waving out.";
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock ();
	        //Debug.Log("Locking");
		}
		//message = "";
	}
}
