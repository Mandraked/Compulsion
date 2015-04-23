using UnityEngine;
using System.Collections;

using UnityStandardAssets.Characters.FirstPerson;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class CoffeeControl : MonoBehaviour {

	public AudioSource src = null;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;

	private bool unlocked = false;
	private bool isOn = false;
	private int magicNumber = 0;
	private int magicCounter = 0;
	private Pose _lastPose = Pose.Unknown;

	// Use this for initialization
	void Start () {
		magicNumber = (int) Mathf.Floor(Random.Range(2.0f, 5.9f));
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked) {
			if (thalmicMyo.pose == Pose.Fist && _lastPose != thalmicMyo.pose) {
				FirstPersonController.m_WalkSpeed = 0;
				
				if (isOn) {
					isOn = false;
					
					src.Pause();

					
				} else {
					magicCounter++;
					isOn = true;
					src.Play();
					
					if (magicCounter % magicNumber != 0) {
						MainScript.showNumber(magicCounter.ToString());

						MainScript.coffeeDone = false;
					} else {
						MainScript.showNumber("");
						magicCounter = 0;
						FirstPersonController.m_WalkSpeed = 5;

						MainScript.coffeeDone = true;
					}
				}
			}
			_lastPose = thalmicMyo.pose;
		}
	}

	void OnTriggerEnter(Collider other) {
		thalmicMyo.Unlock (UnlockType.Hold);
		unlocked = true;
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock();
		}
	}
}
