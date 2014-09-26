using UnityEngine;
using System.Collections;

//has issues if rightHand uses character motor with moving platform enabled...

public class Push : MonoBehaviour 
{
	
	//float pushPower = 2.0f;
	//float weight = 2.0f;
	//CharacterMotor motor;
	CharacterController control;
	
	void Start()
	{
		//motor = GetComponent<CharacterMotor>();
		control = GetComponent<CharacterController>();
	}
	
	void Update(){}

	//should be applying force instead... also need to fix collision with objects on ground...
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		
		//Vector3 force = new Vector3(0.0f, -0.5f, 0.0f);
		//Debug.Log("move direction: "+hit.moveDirection);
			 
		if(body!=null && !body.isKinematic) 
		{
			//Debug.Log("move direction: "+hit.moveDirection);
/*
			if(hit.moveDirection.y < -0.3)
			{
				force = force * motor.movement.gravity * weight;
			} 
			else
			{
				force = hit.controller.velocity * pushPower;		
			}

			//Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
			//body.velocity = pushDir * pushPower;

			body.AddForceAtPosition(force, hit.point);
*/
			//if(hit.moveDirection.y > -0.3)
			//{
				Vector3 normalisedVelocity = control.velocity;
				normalisedVelocity.Normalize();
				body.velocity+=normalisedVelocity;
			//}
		}
	}
}


