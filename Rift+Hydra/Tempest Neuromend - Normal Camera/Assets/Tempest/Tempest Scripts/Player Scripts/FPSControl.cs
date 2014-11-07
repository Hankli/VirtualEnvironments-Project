using UnityEngine;
using System.Collections;

public class FPSControl : MonoBehaviour 
{
	private CharacterMotor motor;
	private CharacterController control;

	private float initialHeight;
	private float initialForwardSpeed;
	private float initialBackwardSpeed;
	private float initialStrafeSpeed;
	 
	private float crouchSpeed;
	private float runSpeed;

	private bool b_inputCrouch = false;
	private bool b_canRun = true;
	private bool b_canCrouch = true;
	private bool b_jump = false;

	private float finalHeight = 0.0f;
	private float forwardSpeed = 0.0f;
	private float backwardSpeed = 0.0f;
	private float strafeSpeed = 0.0f;

	private Vector3 direction;
	 
	public Vector3 Direction
	{
		get { return direction; }
		set { direction = value; }
	}

	void Start()
	{
		motor = GetComponent<CharacterMotor>();
		control = GetComponent<CharacterController>();
		initialHeight = control.height;

		//need to set speed here from menu/gamecontrol values...
		//initialForwardSpeed = motor.movement.maxForwardSpeed;
		//initialBackwardSpeed = motor.movement.maxBackwardsSpeed;
		//initialStrafeSpeed = motor.movement.maxSidewaysSpeed;
		//crouchSpeed = motor.movement.maxForwardSpeed/2.0f;
		//runSpeed = motor.movement.maxForwardSpeed*3;
	}
	 
	public void SetMovementSpeeds(float speed)
	{
		initialForwardSpeed = speed;
		initialBackwardSpeed = speed;
		initialStrafeSpeed = speed;
		crouchSpeed = speed / 2.0f;
		runSpeed = speed * 2.0f;
	}

	void Update()
	{
		finalHeight = initialHeight;
		forwardSpeed = initialForwardSpeed;
		backwardSpeed = initialBackwardSpeed;
		strafeSpeed = initialStrafeSpeed;

		Debug.Log (strafeSpeed);

		if(!b_inputCrouch)
		{
			float lastHeight = control.height;
			control.height = Mathf.Lerp(control.height, finalHeight, 5.0f * Time.deltaTime);

			transform.position += new Vector3(0, (control.height - lastHeight) / 2.0f, 0);
		}	

		//walking based movement
		float walk = direction.z < 0.0f ? (direction.z * backwardSpeed) : (direction.z * forwardSpeed);
		float strafe = direction.x * strafeSpeed;

		Vector3 directionVector = new Vector3(strafe, 0.0f, walk);


		if (directionVector != Vector3.zero) 
		{
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = direction.magnitude;

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
			directionVector.y = motor.movement.velocity.y; //keep jump velocity

			motor.movement.maxForwardSpeed = forwardSpeed;
			motor.movement.maxBackwardsSpeed = backwardSpeed;
			motor.movement.maxSidewaysSpeed = strafeSpeed;

			motor.movement.velocity = directionVector;
			motor.inputJump = b_jump;
		}
	}

	public void ForceJump()
	{
		b_jump = true;
	}

	public void StopJump()
	{
		b_jump = false;
	}

	public void StopCrouch()
	{
		b_inputCrouch = false;
	}
	
	public void ForceCrouch()
	{
		if(b_canCrouch)
		{
			b_inputCrouch = true;

			finalHeight = 0.5f * initialHeight;
			forwardSpeed = crouchSpeed;
			backwardSpeed = crouchSpeed;
			strafeSpeed = crouchSpeed;
			
			motor.movement.maxForwardSpeed = forwardSpeed;
			motor.movement.maxBackwardsSpeed = backwardSpeed;
			motor.movement.maxSidewaysSpeed = strafeSpeed;
			
			float lastHeight = control.height;
			control.height = Mathf.Lerp(control.height, finalHeight, 5.0f * Time.deltaTime);

 			transform.position += new Vector3(0,(control.height - lastHeight) / 2.0f, 0);
		}
	}
}
