using UnityEngine;
using System.Collections;

public class LeapControl : MonoBehaviour 
{
	public bool twoHands;
	public float sensitivity;

	// Use this for initialization
	void Start () {
		twoHands = false;
		sensitivity = 5.0f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float GetSensitivity()
	{
		Debug.Log ("get sens " + sensitivity);
		return sensitivity;
	}
	
	public void SetSensitivity(float sens)
	{
		Debug.Log ("set sens to " + sens);
		sensitivity = sens;
	}

	public bool GetTwoHands()
	{
		Debug.Log ("get hands " + twoHands);
		return twoHands;
	}
	
	public void SetTwoHands(bool th)
	{
		Debug.Log ("set hands to " + th);
		twoHands = th;
	}
}
