using UnityEngine;
using System.Collections;
using Leap;

enum SwipeDirection
{
	Up,
	Down,
	Left,
	Right
}

public class LeapAvoidance : MonoBehaviour 
{
	Controller m_leapController;
	float sensitivity = 50.0f;
	float speed;
	Vector3 newPos;
	bool twoHands;
	private CharacterController control;
	Vector3 centerPos;
	float jumpImpulse;
	Vector3 acceleration;
	Vector3 velocity;
	bool jump;


	private LeapControl variables = null;
	private GameObject gameControlObject = null;
	
	void Awake()
	{
		if(gameControlObject = GameObject.FindGameObjectWithTag("Game"))
		{
			if(variables=gameControlObject.GetComponent<LeapControl>())
			{
				//bleh
			}
		}
		
	}

	// Use this for initialization
	void Start () 
	{
		if (variables) 
		{
			//********variables changed in menu********//
			twoHands = variables.GetTwoHands();
			sensitivity = variables.GetSensitivity();
			sensitivity *= 10.0f; //sensitivity of the hand rotation. decrease for harder, increase for easier
			//********variables changed in menu********//
		}

		jump = true;
		acceleration = new Vector3(0.0f, 0.0f, 0.0f);
		velocity = new Vector3(0.0f, 0.0f, 0.0f);
		jumpImpulse = 20.0f;
		twoHands = false;
		speed = 2.0f;
		control = transform.parent.GetComponent<CharacterController>();
		
		m_leapController = new Controller();

		if (transform.parent == null)
		{
			Debug.LogError("LeapMove must have a parent object to control (eg camera)");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Frame frame = m_leapController.Frame();
		Frame lastFrame = m_leapController.Frame(1);

		HandList hands = frame.Hands;
		Hand firstHand = hands[0];

		//The change of position of this hand between the current frame and the specified frame.
		Vector handMovement = (firstHand.Translation(lastFrame));


		Vector3 handPos = firstHand.PalmPosition.ToUnity();
		//Debug.Log ("handPos = " + handPos);

		if (frame.Hands.Count >= 1) 
		{
			//if hand is left
			if(handPos.x < -sensitivity)
			{
				newPos.x = transform.parent.forward.x + speed;
				if(handPos.x < -(sensitivity+50.0f))
					newPos.x = transform.parent.forward.x + (speed*2.0f);
			}
			//if hand is right
			else if(handPos.x > sensitivity)
			{
				newPos.x = transform.parent.forward.x - speed;
				if(handPos.x > (sensitivity+50.0f))
					newPos.x = transform.parent.forward.x - (speed*2.0f);
			}
			else
					newPos.x = transform.parent.forward.x;
			
			//if hand is up
			if(handPos.z > sensitivity)
			{
				//motor.inputJump = false;
				//Debug.Log ("up");
				CharacterMotor motor = GetComponentInParent<CharacterMotor>(); //javascript class object
				if(motor.grounded)
				{
					//Debug.Log ("grounded");
					motor.inputMoveDirection = new Vector3(0.0f, jumpImpulse, 0.0f);
					motor.inputJump = true;
				}
				else
				{
					motor.inputJump = false;
				}
				/*Debug.Log ("up");
				if(jump)
				{
					Debug.Log ("JUMP");
					jump = false;
					acceleration.y = jumpImpulse;
				}
				velocity.y += acceleration.y * Time.deltaTime;
					
				CharacterMotor motor = GetComponentInParent<CharacterMotor>(); //javascript class object 
				acceleration.y -= motor.movement.gravity * Time.deltaTime;
				
				
				if(control.isGrounded) //character starts on ground and the code below runs and resets al previously alered values
				{
					if(acceleration.y < 0.0f)
					{
						velocity.y = 0.0f;
						acceleration.y = 0.0f;
						jump = true;
					}
				}

				//control.Move (new Vector3(0.0f, 1.0f, 0.0f));
				control.Move(velocity);*/
	
			}
			//if hand is down
			else if(handPos.z < -(sensitivity+50.0f))
			{
				FPSCrouchRun crouchScript;
				crouchScript=GetComponentInParent<FPSCrouchRun>();
				crouchScript.LeapCrouch();
			}
			else
				newPos.y = transform.parent.forward.y;

			control.SimpleMove (newPos);
		}
	}
}
