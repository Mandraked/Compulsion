using UnityEngine;
using System.Collections;

using UnityStandardAssets.Characters.FirstPerson;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class TVControl : MonoBehaviour {

	public GameObject tvScreen = null;
	public GameObject myo = null;
	ThalmicMyo thalmicMyo = null;

	public Texture onTexture = null;
	public Texture offTexture = null;

	private bool unlocked = false;
	private bool isOn = true;
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
					magicCounter++;
					tvScreen.GetComponent<Renderer>().material.mainTexture = offTexture;

					if (magicCounter % magicNumber != 0) {
						MainScript.showNumber(magicCounter.ToString());

						MainScript.tvDone = false;
					} else {
						MainScript.showNumber("");
						magicCounter = 0;
						FirstPersonController.m_WalkSpeed = 5;

						MainScript.tvDone = true;
					}
				} else {
					isOn = true;
					tvScreen.GetComponent<Renderer>().material.mainTexture = onTexture;
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
