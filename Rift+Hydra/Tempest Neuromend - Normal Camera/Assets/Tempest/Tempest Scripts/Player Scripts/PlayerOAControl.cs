using UnityEngine;
using System.Collections;

public class PlayerOAControl : MonoBehaviour 
{
	private Vector3 translationAll = Vector3.zero;
	private CharacterController control;
	private CharacterMotor motor;
	public float speed = 3.0f;
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
		motor.movement.maxForwardSpeed=0;
		motor.movement.maxBackwardsSpeed=0;
		
		translationAll = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 1.0f);//1.0f is constant 'forward' movement
		translationAll = transform.TransformDirection(translationAll);//direction from world to local

		if(b_notOver&&!b_knockBack)
		{
			if(control)
			{
				//translationAll.y = 0.0f;
				//control.SimpleMove(translationAll * speed);
				Tempest.RazorHydra.HydraCharacterController handController = GetComponentInChildren<Tempest.RazorHydra.HydraCharacterController>();

				handController.ConstantWalkSpeed = speed;
			}
		}
		else if(b_knockBack)
		{
			Vector3 knockBack = new Vector3(0.0f, 0.0f, -3.0f);
			if(control)
				control.SimpleMove(knockBack);
			float knockBackTimeCheck = Time.time;
			if(knockBackTimeCheck-knockBackTime>knockBackDuration)
			{
				b_knockBack=false;
			}
		}
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
