using UnityEngine;
using System.Collections;

public class PlayerOAControl : MonoBehaviour 
{
	Vector3 translationAll = Vector3.zero;
	CharacterController control;
	CharacterMotor motor;
	public float speed = 3.0f;
	private bool b_notOver=true;
	
	void Start() 
	{
		control = GetComponent<CharacterController>();
		motor = GetComponent<CharacterMotor>();
	}
	
	void Update() 
	{
		motor.movement.maxForwardSpeed=0;
		motor.movement.maxBackwardsSpeed=0;
		
		translationAll = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 1.0f);//1.0f is constant 'forward' movement
		translationAll = transform.TransformDirection(translationAll);//direction from world to local
		if(b_notOver)
		{
			control.SimpleMove(translationAll*speed);
		}
	}
	
	public void ReachedEndZone()
	{	
		b_notOver=false;
	}
	
}
