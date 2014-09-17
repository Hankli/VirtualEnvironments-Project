using UnityEngine;
using System.Collections;

public class FPSCrouchRun : MonoBehaviour 
{
	private CharacterMotor motor;
	private CharacterController control;
	private float initialHeight;
	private float initialForwardSpeed;
	private float initialBackwardSpeed;
	private float initialStrafeSpeed;
	
	public float crouchSpeed;
	public float runSpeed;
	public bool b_CanRun=true;
	public bool b_CanCrouch=true;
	
	float finalHeight = 0.0f;
	float forwardSpeed = 0.0f;
	float backwardSpeed = 0.0f;
	float strafeSpeed = 0.0f;
	
	bool b_leapCrouch=false;
	float leapCrouchDuration=1.0f;
	float leapCrouchStart=0.0f;
	
	void Start()
	{
		motor = GetComponent<CharacterMotor>();
		control = GetComponent<CharacterController>();
		initialHeight = control.height;
		initialForwardSpeed = motor.movement.maxForwardSpeed;
		initialBackwardSpeed = motor.movement.maxBackwardsSpeed;
		initialStrafeSpeed = motor.movement.maxSidewaysSpeed;
		crouchSpeed = motor.movement.maxForwardSpeed/2.0f;
		runSpeed = motor.movement.maxForwardSpeed*3;
	}
	
	void Update()
	{
		finalHeight = initialHeight;
		forwardSpeed = initialForwardSpeed;
		backwardSpeed = initialBackwardSpeed;
		strafeSpeed = initialStrafeSpeed;
		
		//RUN
		if(control.isGrounded && Input.GetKey("left shift") && b_CanRun)
		{
			forwardSpeed = runSpeed;
			backwardSpeed = runSpeed;
			strafeSpeed = runSpeed;
		}
		
		//CROUCH
		if(Input.GetKey("left ctrl") && b_CanCrouch||b_leapCrouch)
		{
			finalHeight = 0.5f*initialHeight;
			forwardSpeed = crouchSpeed;
			backwardSpeed = crouchSpeed;
			strafeSpeed = crouchSpeed;
		}
		if(b_leapCrouch)
		{
			float timeCheck=Time.time;
			if(timeCheck-leapCrouchStart>=leapCrouchDuration)
			{
				b_leapCrouch=false;
			}
		}
		
		motor.movement.maxForwardSpeed = forwardSpeed;
		motor.movement.maxBackwardsSpeed = backwardSpeed;
		motor.movement.maxSidewaysSpeed = strafeSpeed;
		
		float lastHeight = control.height;
		control.height = Mathf.Lerp(control.height, finalHeight, 5*Time.deltaTime);
		
		float bleh = (control.height-lastHeight)/2.0f;
		transform.position += new Vector3(0,bleh,0);
		
		
	}
	
	public void LeapCrouch()
	{
		if(b_CanCrouch)
		{
			b_leapCrouch=true;
			leapCrouchStart=Time.time;
			
			finalHeight = 0.5f*initialHeight;
			forwardSpeed = crouchSpeed;
			backwardSpeed = crouchSpeed;
			strafeSpeed = crouchSpeed;
			
			motor.movement.maxForwardSpeed = forwardSpeed;
			motor.movement.maxBackwardsSpeed = backwardSpeed;
			motor.movement.maxSidewaysSpeed = strafeSpeed;
			
			float lastHeight = control.height;
			control.height = Mathf.Lerp(control.height, finalHeight, 5*Time.deltaTime);
			
			float bleh = (control.height-lastHeight)/2.0f;
			transform.position += new Vector3(0,bleh,0);
		}
	}
}
