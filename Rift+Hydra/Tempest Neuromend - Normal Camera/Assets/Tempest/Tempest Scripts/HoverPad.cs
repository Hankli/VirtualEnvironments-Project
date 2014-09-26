using UnityEngine;
using System.Collections;

//need to slow gravity towards peak and have it stay at 0?
//that is to stop the player exiting from the top of the volume...
//then if player looks down, gravity returns to normal/half speed

public class HoverPad : MonoBehaviour 
{
    public float hoverForce = 0.05f;
    //public float gravity = 10.0f;
	
	void Start()
	{
	}
	
	void Update() 
	{
	}
	
	void OnTriggerStay(Collider other)
    {
		CharacterController controller = other.GetComponent<CharacterController>();
		if(controller)
		{
			CharacterMotor motor = other.GetComponent<CharacterMotor>();
			if(motor)
			{
				//if negative on grounded jump, causes error...
				if(!controller.isGrounded)
				{
					motor.canControl = false;
					//Transform thisTransform = 
			
					motor.movement.gravity = -5.0f;
					if(controller.transform.position.y>=this.gameObject.transform.position.y-3)
					{
						//Debug.Log("blip");
						motor.movement.gravity = 2.0f;
					}
				}
				else
				{
					motor.movement.gravity = 20.0f;//so, reset gravity on ground
					motor.canControl = true;
				}
				//Debug.Log(motor.movement.gravity);
			}
		}
    }
    
	void OnTriggerExit(Collider other)
    {
		CharacterMotor motor = other.GetComponent<CharacterMotor>();
		if(motor)
		{
			motor.movement.gravity = 20.0f;
			motor.canControl = true;
		}
    }

}
