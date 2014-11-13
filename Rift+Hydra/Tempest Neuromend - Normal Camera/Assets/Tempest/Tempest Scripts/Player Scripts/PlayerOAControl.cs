using UnityEngine;
using System.Collections;

public class PlayerOAControl : MonoBehaviour 
{
	private Vector3 translationAll = Vector3.zero;
	private CharacterController control;
	private CharacterMotor motor;
	private float m_walk = 3.0f;
	private float m_strafe = 0.0f;
	private bool b_notOver=true;
	private bool b_knockBack=false;
	private float knockBackDuration = 0.1f;
	private float knockBackTime = 0.0f;
	
	void Start() 
	{
		control = GetComponent<CharacterController>();
		motor = GetComponent<CharacterMotor>();
	}
	
	void Update() 
	{
		//enable user strafe
		m_strafe = motor.movement.velocity.x;

		//set max speeds
		motor.movement.maxForwardSpeed = 0;
		motor.movement.maxBackwardsSpeed = 0;

		//disable velocity based movement
		motor.movement.velocity.x = 0;
		motor.movement.velocity.z = 0; 
		
		translationAll = transform.TransformDirection(Vector3.forward);//direction from world to loca

		if(b_notOver && !b_knockBack)
		{
			if(control)
			{
				translationAll.x = m_strafe;
				translationAll.z *= m_walk;

				control.SimpleMove(translationAll);
			}
		}
		else if(b_knockBack)
		{
			Vector3 knockBack = new Vector3(0.0f, 0.0f, -3.0f);
			if(control)
			{
				control.SimpleMove(knockBack);
			}

			float knockBackTimeCheck = Time.time;
			if(knockBackTimeCheck - knockBackTime > knockBackDuration)
			{
				b_knockBack=false;
			}
		}
	}

	public void SetMovementSpeed(float speed)
	{
		m_walk = speed;
	}
	
	public void ReachedEndZone()
	{	
		b_notOver=false;
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		//Debug.Log(hit.gameObject.tag);
		if(hit.gameObject.tag=="Moving Obstacle")
		{
			//Debug.Log("Knockback!");
			b_knockBack=true;
			knockBackTime = Time.time;
		}
	}
	
}
