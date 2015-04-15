﻿using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;

public class MainScript : MonoBehaviour {

	public int magicNumber = 0;
	public static float height = Screen.height/5;
	public static float width = Screen.width/5;
	public static float x;
	public static float y;
	private string message = "";

	public static Quaternion antiYaw;
	public static Vector3 _antiYaw;
	public static float _referenceRoll = 0.0f;
	public GameObject myo = null;
	private Pose _lastPose = Pose.Unknown;
	public static bool angleSet = false;

	// Use this for initialization
	void Start () {
		//message = "Welcome to compulsion, the magic number is " + magicNumber + ".";
		message = "Welcome. Make a fist with your arm perpendicular to your body and your palm toward the ground.";
		x = Screen.width/2-width/2;
		y = Screen.height/2 - height/2;
	}
	
	// Update is called once per frame
	void Update () {
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (thalmicMyo.pose != _lastPose && thalmicMyo.pose == Pose.Fist && !angleSet) {
			_lastPose = Pose.Fist;
			angleSet = true;

			antiYaw = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
                new Vector3 (0, 0, 1)
            );

            _antiYaw = FromQ2(antiYaw);

            Vector3 referenceZeroRoll = computeZeroRollVector (myo.transform.forward);
            _referenceRoll = rollFromZero (referenceZeroRoll, myo.transform.forward, myo.transform.up);

            message = "Thanks for setting forward.";
            thalmicMyo.Lock();
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(x, y, width, height), message);
		if (Time.time >= 5 && !message.Equals("")) message = "";
	}

	public static float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        // The cosine of the angle between the up vector and the zero roll vector. Since both are
        // orthogonal to the forward vector, this tells us how far the Myo has been turned around the
        // forward axis relative to the zero roll vector, but we need to determine separately whether the
        // Myo has been rolled clockwise or counterclockwise.
        float cosine = Vector3.Dot (up, zeroRoll);

        // To determine the sign of the roll, we take the cross product of the up vector and the zero
        // roll vector. This cross product will either be the same or opposite direction as the forward
        // vector depending on whether up is clockwise or counter-clockwise from zero roll.
        // Thus the sign of the dot product of forward and it yields the sign of our roll value.
        Vector3 cp = Vector3.Cross (up, zeroRoll);
        float directionCosine = Vector3.Dot (forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        // Return the angle of roll (in degrees) from the cosine and the sign.
        return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
    }

    Vector3 computeZeroRollVector (Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross (m, myo.transform.forward);

        return roll.normalized;
    }
	
	public static Vector3 FromQ2 (Quaternion q1)
	{
	    float sqw = q1.w * q1.w;
	    float sqx = q1.x * q1.x;
	    float sqy = q1.y * q1.y;
	    float sqz = q1.z * q1.z;
	    float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
	    float test = q1.x * q1.w - q1.y * q1.z;
	    Vector3 v;

	    if (test>0.4995f*unit) { // singularity at north pole
	        v.y = 2f * Mathf.Atan2 (q1.y, q1.x);
	        v.x = Mathf.PI / 2;
	        v.z = 0;
	        return NormalizeAngles (v * Mathf.Rad2Deg);
	    }
	    if (test<-0.4995f*unit) { // singularity at south pole
	        v.y = -2f * Mathf.Atan2 (q1.y, q1.x);
	        v.x = -Mathf.PI / 2;
	        v.z = 0;
	        return NormalizeAngles (v * Mathf.Rad2Deg);
	    }
	    Quaternion q = new Quaternion (q1.w, q1.z, q1.x, q1.y);
	    v.y = (float)Mathf.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
	    v.x = (float)Mathf.Asin (2f * (q.x * q.z - q.w * q.y));                             // Pitch
	    v.z = (float)Mathf.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
	    return NormalizeAngles (v * Mathf.Rad2Deg);
	}

	static Vector3 NormalizeAngles (Vector3 angles)
	{
	    angles.x = NormalizeAngle (angles.x);
	    angles.y = NormalizeAngle (angles.y);
	    angles.z = NormalizeAngle (angles.z);
	    return angles;
	}

	static float NormalizeAngle (float angle)
	{
	    while (angle>360)
	        angle -= 360;
	    while (angle<0)
	        angle += 360;
	    return angle;
	}
	
}
