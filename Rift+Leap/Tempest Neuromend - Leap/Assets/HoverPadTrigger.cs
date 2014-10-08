using UnityEngine;
using System.Collections;

public class HoverPadTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider cl)
	{
		if(cl.gameObject.name == "hoverVolume")
		GetComponentInParent<CharacterMotor> ().inputJump = true;
		/*CharacterMotor motor = GetComponentInParent<CharacterMotor>(); //javascript class object
		if(cl.gameObject.name == "hoverVolume")
		{
			Debug.Log ("i love debugging");
			motor.inputJump = true;
		}*/
	}
	
	void OnTriggerExit(Collider cl)
	{
		/*CharacterMotor motor = GetComponentInParent<CharacterMotor>(); //javascript class object
		if(cl.gameObject.name == "hoverVolume")
		{
			//motor.inputJump = false;
		}*/
	}
}
