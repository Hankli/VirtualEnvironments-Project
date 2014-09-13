using UnityEngine;
using System.Collections;
using Leap;

public class LeapOculusMove : MonoBehaviour 
{
	Controller m_leapController;
	float speed;
	float rotSpeed;
	float stopping;
	Vector3 newPos;
	Vector3 newRot;
	
	private CharacterController control;
	
	
	// Use this for initialization
	void Start () 
	{
		speed = 2.0f;
		rotSpeed = 0.5f;
		control = transform.parent.GetComponent<CharacterController>();
		
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

		if (frame.Hands.Count == 1) 
		{
			//Vector handXBasis =  firstHand.PalmNormal.Cross(firstHand.Direction).Normalized;
			float roll = firstHand.PalmNormal.Roll;
			if((roll <= 2.9f) && (roll >= -2.9f))
				newRot.y -= roll; // * distance from center.
			transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), rotSpeed);
		}
		else if (frame.Hands.Count >= 2)
		{
			stopping = 5.0f;//change for time it takes to stop after hands leave
			Hand leftHand = GetLeftMostHand(frame);
			Hand rightHand = GetRightMostHand(frame);

			float rollLeft = leftHand.PalmNormal.Roll;
			if((rollLeft <= 2.9f) && (rollLeft >= -2.9f))
				newRot.y -= rollLeft; // * distance from center.

			transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), rotSpeed);

			if ((isExtended (leftHand) > 3)&&(isExtended (rightHand) > 3))//if open hands
			{
				speed = 2.0f;
			}
			else if ((isExtended (leftHand) < 3)&&(isExtended (rightHand) < 3))//if closed fist
			{
				speed = -2.0f; //stop movement
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
}
