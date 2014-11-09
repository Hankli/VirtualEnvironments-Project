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

	private bool b_inputCrouch = false;
	private bool b_inputRun = false;
	private bool b_inputJump = false;

	private bool b_canRun = false;
	private bool b_canJump = false;
	private bool b_canCrouch = false;

	private float finalHeight = 0.0f;
	private float forwardSpeed = 0.0f;
	private float backwardSpeed = 0.0f;
	private float strafeSpeed = 0.0f;

	private float crouchSpeedModifier = 0.5f;
	private float sprintSpeedModifier = 2.0f;

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
	}
	 
	public void SetMovementSpeeds(float speed)
	{
		initialForwardSpeed = speed;
		initialBackwardSpeed = speed;
		initialStrafeSpeed = speed;
	}

	public void SetMovementSpeeds(float forward, float backward, float strafe)
	{
		initialForwardSpeed = forward;
		initialBackwardSpeed = backward;
		initialStrafeSpeed = strafe;
	}

	private void Update()
	{
		SetAllowableMotion ();

		finalHeight = initialHeight;
		forwardSpeed = initialForwardSpeed;
		backwardSpeed = initialBackwardSpeed;
		strafeSpeed = initialStrafeSpeed;

		if(b_inputCrouch)
		{
			forwardSpeed = initialForwardSpeed * crouchSpeedModifier;
			backwardSpeed = initialBackwardSpeed * crouchSpeedModifier;
			strafeSpeed = initialStrafeSpeed * crouchSpeedModifier;
		}
		else
		{
			float lastHeight = control.height;
			control.height = Mathf.Lerp(control.height, finalHeight, 5.0f * Time.deltaTime);
			
			transform.position += new Vector3(0, (control.height - lastHeight) / 2.0f, 0);
		}

		if(b_inputRun)
		{
			forwardSpeed = initialForwardSpeed * sprintSpeedModifier;
			backwardSpeed = initialBackwardSpeed * sprintSpeedModifier;
			strafeSpeed = initialStrafeSpeed * sprintSpeedModifier;
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
			motor.inputJump = b_inputJump;
		}
	}

	private void SetAllowableMotion()
	{
		if(motor)
		{
			if(motor.grounded)
			{
				b_canRun = true;
				b_canJump = true;
				b_canCrouch = true;
			}
			else
			{
				b_canRun = false;
				b_canJump = false;
				b_canCrouch = false;
			}
		}
	}

	public void ForceRun()
	{
		if(b_canRun)
		{
			b_inputRun = true;
		}
	}

	public void StopRun()
	{
		b_inputRun = false;
	}

	public void ForceJump()
	{
		if(b_canJump)
		{
			b_inputJump = true;
		}
	}

	public void StopJump()
	{
		b_inputJump = false;
	}
	
	public void ForceCrouch()
	{
		if(b_canCrouch)
		{
			b_inputCrouch = true;

			finalHeight = 0.5f * initialHeight;
			float lastHeight = control.height;
			control.height = Mathf.Lerp(control.height, finalHeight, 5.0f * Time.deltaTime);

 			transform.position += new Vector3(0,(control.height - lastHeight) / 2.0f, 0);
		}
	}

	public void StopCrouch()
	{
		b_inputCrouch = false;
	}
}
