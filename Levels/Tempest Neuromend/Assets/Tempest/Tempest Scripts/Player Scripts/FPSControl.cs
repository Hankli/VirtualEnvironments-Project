/*
Ary...
slight mod of standard Unity javascript FPSInputController to allow for forced jump not using input button etc...

*/
using UnityEngine;
using System.Collections;

public class FPSControl : MonoBehaviour 
{
	CharacterMotor motor=null;
	private bool b_jump;

	private bool b_forceJump=false;

	void Awake() 
	{
		motor = GetComponent<CharacterMotor>();
	}
		
	void Update() 
	{
		b_jump=Input.GetButton("Jump");

		if(b_forceJump)
		{
			b_jump=true;
			b_forceJump=false;
		}

		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if (directionVector != Vector3.zero) 
		{
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		// Apply the direction to the CharacterMotor
		if(motor)
		{
			motor.inputMoveDirection = transform.rotation * directionVector;
			motor.inputJump = b_jump;
		}

	}

	public void ForceJump()
	{
		b_forceJump = true;
	}


}

