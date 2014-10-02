using UnityEngine;
using System.Collections;
using Leap;

public class LeapOculusMove : MonoBehaviour 
{
	Controller m_leapController;
	float speed;
	float forwardSpeed;
	float backwardSpeed;
	float sensitivity = 2.9f;
	float rotSpeed;
	float stopping;
	Vector3 newPos;
	Vector3 newRot;
	OVRDevice oculus;
	bool twoHands;

	private CharacterController control;
	
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
		forwardSpeed = 3.0f;
		backwardSpeed = -2.0f;

		if(variables)
		{
			//********variables changed in menu********//
			twoHands = variables.GetTwoHands();//if two or 1 hands selected
			sensitivity = variables.GetSensitivity();
			Debug.Log ("sensitivity = " + sensitivity);
			float OldRange = (10.0f - 0.0f);  
			float NewRange = (3.5f - 2.5f);  
			sensitivity = (((sensitivity - 0.0f) * NewRange) / OldRange) + 2.5f;
			Debug.Log ("sensitivity = " + sensitivity);
			//sensitivity = 2.9f; //sensitivity of the hand rotation. decrease for harder (2.5), increase for easier (3.1), ave 2.9.
			//********variables changed in menu********//
		}

		speed = 2.0f;
		rotSpeed = 0.5f;
		control = transform.parent.GetComponent<CharacterController>();
		oculus = GetComponent<OVRDevice> ();
		
		m_leapController = new Controller();
		stopping = 0.0f;
		if (transform.parent == null)
		{
			Debug.LogError("LeapMove must have a parent object to control (eg camera)");
		}
		
	}
	
	Hand GetLeftMostHand(Frame f)
	{
		float smallestVal = float.MaxValue;
		Hand h = null;
		for (int i = 0; i < f.Hands.Count; ++i)
		{
			if (f.Hands[i].PalmPosition.ToUnity().x < smallestVal)
			{
				smallestVal = f.Hands[i].PalmPosition.ToUnity().x;
				h = f.Hands[i];
			}
		}
		return h;
	}
	
	Hand GetRightMostHand(Frame f)
	{
		float largestVal = -float.MaxValue;
		Hand h = null;
		for (int i = 0; i < f.Hands.Count; ++i)
		{
			if (f.Hands[i].PalmPosition.ToUnity().x > largestVal)
			{
				largestVal = f.Hands[i].PalmPosition.ToUnity().x;
				h = f.Hands[i];
			}
		}
		return h;
	}
	
	int isExtended(Hand hand)
	{
		int extendedFingers = 0;
		for (int f = 0; f < hand.Fingers.Count; f++)
		{
			Finger digit = hand.Fingers[f];
			if(digit.IsExtended) extendedFingers++;
		}
		return extendedFingers;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Frame frame = m_leapController.Frame();
		Frame lastFrame = m_leapController.Frame(1);

		HandList hands = frame.Hands;
		Hand firstHand = hands[0];

		float pitch = firstHand.Direction.Pitch;
		float yaw = firstHand.Direction.Yaw;
		//float roll = firstHand.PalmNormal.Roll;
		Vector position = firstHand.PalmPosition;
		Vector velocity = firstHand.PalmVelocity;
		Vector direction = firstHand.Direction;
		Vector normal = firstHand.PalmNormal;
		
		//The change of position of this hand between the current frame and the specified frame.
		Vector handMovement = (firstHand.Translation(lastFrame));

		newRot = transform.parent.localRotation.eulerAngles;

		if(twoHands)
		{
			if (frame.Hands.Count == 1) 
			{
				//Vector handXBasis =  firstHand.PalmNormal.Cross(firstHand.Direction).Normalized;
				float roll = firstHand.PalmNormal.Roll;
				if((roll <= sensitivity) && (roll >= -sensitivity))
					newRot.y -= roll; // * distance from center.
				transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), rotSpeed);
			}
			else if (frame.Hands.Count >= 2)
			{
				stopping = 5.0f;//change for time it takes to stop after hands leave
				Hand leftHand = GetLeftMostHand(frame);
				Hand rightHand = GetRightMostHand(frame);

				float rollLeft = leftHand.PalmNormal.Roll;
				if((rollLeft <= sensitivity) && (rollLeft >= -sensitivity))
					newRot.y -= rollLeft; // * distance from center.

				transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), rotSpeed);

				if ((isExtended (leftHand) > 3)&&(isExtended (rightHand) > 3))//if open hands
				{
					speed = forwardSpeed;
				}
				else if ((isExtended (leftHand) < 3)&&(isExtended (rightHand) < 3))//if closed fist
				{
					speed = backwardSpeed;
				}

				newPos = new Vector3((transform.parent.forward.x * speed), (transform.parent.forward.y * speed), (transform.parent.forward.z * speed));
				control.SimpleMove (newPos);
			}
			else if(frame.Hands.Count == 0)
			{
				stopping--;
				if(stopping==0.0f)
				{
					control.SimpleMove(newPos*0.0f);
				}
			}
		}
		else if(!twoHands)
		{
			if (frame.Hands.Count == 1) 
			{
				float roll = firstHand.PalmNormal.Roll;
				if((roll <= sensitivity) && (roll >= -sensitivity))
					newRot.y -= roll;
				transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), rotSpeed);

				if (isExtended (firstHand) > 3)//if open hands
				{
					speed = forwardSpeed;
				}
				else if (isExtended (firstHand) < 3)//if closed fist
				{
					speed = backwardSpeed;
				}
				
				newPos = new Vector3((transform.parent.forward.x * speed), (transform.parent.forward.y * speed), (transform.parent.forward.z * speed));
				control.SimpleMove (newPos);
			}
		}
	}
}
