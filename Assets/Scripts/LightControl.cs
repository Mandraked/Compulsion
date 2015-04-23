using UnityEngine;
using System.Collections;

using UnityStandardAssets.Characters.FirstPerson;

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
	//private string message = "";

	private int magicNumber = 0;
	private int magicCounter = 0;

	private bool toggle = false;
	public bool isMainLight = false;

	// Use this for initialization
	void Start () {
		magicNumber = (int) Mathf.Floor(Random.Range(2.0f, 5.9f));
	}
	
	// Update is called once per frame
	void Update () {
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (unlocked)
		{
			if (thalmicMyo.pose == Pose.WaveIn && _lastPose != thalmicMyo.pose) {
				FirstPersonController.m_WalkSpeed = 0;
				_lastPose = thalmicMyo.pose;
				Vector3 vec = new Vector3 (lSwitch.transform.localScale.x, -1 * startingY, lSwitch.transform.localScale.z);
				lSwitch.transform.localScale = vec;
				myLight.enabled = false;
				toggle = true;

				if (toggle) {
					magicCounter++;
					if (magicCounter % magicNumber != 0) {
						MainScript.showNumber(magicCounter.ToString());

						if (isMainLight) MainScript.mainLightDone = false;
						else MainScript.bathroomLightDone = false;
					} else {
						MainScript.showNumber("");
						magicCounter = 0;
						FirstPersonController.m_WalkSpeed = 5;
						
						if (isMainLight) MainScript.mainLightDone = true;
						else MainScript.bathroomLightDone = true;
					}
					toggle = false;
				}
			}
			else if (thalmicMyo.pose == Pose.WaveOut && _lastPose != thalmicMyo.pose) {
				_lastPose = thalmicMyo.pose;
				Vector3 vec = new Vector3 (lSwitch.transform.localScale.x, startingY, lSwitch.transform.localScale.z);
				lSwitch.transform.localScale = vec;
				myLight.enabled = true;
			}
		}
	}

	void OnGUI() {
        //GUI.Label(new Rect(10, 10, 250, 200), message);
    }

	void OnTriggerEnter(Collider other) {
        thalmicMyo.Unlock (UnlockType.Hold);
		unlocked = true;
		//message = "Turn the light off by waving in \nor turn it on by waving out.";
	}

	void OnTriggerExit(Collider other) {
		if (thalmicMyo.unlocked == true) {
			unlocked = false;
			thalmicMyo.Lock ();
		}
		//message = "";
	}
}
