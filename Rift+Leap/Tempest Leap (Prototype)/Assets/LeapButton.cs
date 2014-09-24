using UnityEngine;
using System.Collections;
using Leap;

public class LeapButton : MonoBehaviour 
{
	private void OnTriggerEnter(Collider cl)
	{
		Debug.Log ("panel trigger");
		TouchPanelButton button = cl.transform.GetComponent<TouchPanelButton> ();
		
		if(button)
		{
			button.OnClick();
		}
	}
}