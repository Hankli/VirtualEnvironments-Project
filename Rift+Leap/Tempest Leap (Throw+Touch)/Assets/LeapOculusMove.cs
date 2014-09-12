using UnityEngine;
using System.Collections;
using Leap;

public class LeapOculusMove : MonoBehaviour 
{
	Controller m_leapController;
	float move;
	float forceMult;
	//float maxVelocity;
	Vector3 translationAll;
	
	private CharacterController control;
	
	
	// Use this for initialization
	void Start () 
	{
		if (Application.HasProLicense())
			Debug.Log ("I have pro!");
		control = transform.parent.GetComponent<CharacterController>();
		
		m_leapController = new Controller();
		move = 0.0f;
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
		
		if (frame.Hands.Count >= 2)
		{
			move = 5.0f;//change for time it takesto stop after hands leave
			Hand leftHand = GetLeftMostHand(frame);
			Hand rightHand = GetRightMostHand(frame);
			
			// takes the average vector of the forward vector of the hands, used for the
			// pitch of the camera.
			Vector3 avgPalmForward = (frame.Hands[0].Direction.ToUnity() + frame.Hands[1].Direction.ToUnity()) * 0.1f;
			
			Vector3 handDiff = leftHand.PalmPosition.ToUnityScaled() - rightHand.PalmPosition.ToUnityScaled();
			
			Vector3 newRot = transform.parent.localRotation.eulerAngles;
			newRot.z = -handDiff.y * 100.0f; //alter for angle
			
			// adding the rot.z as a way to use banking (rolling) to turn.
			//newRot.y += handDiff.z * 3.0f - newRot.z * 0.03f * transform.parent.rigidbody.velocity.magnitude;
			newRot.y = (avgPalmForward.x) * 2500.0f;
			newRot.x = -(avgPalmForward.y - 0.1f) * 100.0f;
			
			if ((isExtended (leftHand) > 3)&&(isExtended (rightHand) > 3))//if open hands
			{
				if(newRot.x >= 0.0f) //if hands are angled further in the game
					forceMult = 2.0f; //go forwards, increase for speed
				else  //if hands closer to the body
					forceMult = -2.0f; //go backwards, decrease for speed
			}
			else if ((isExtended (leftHand) < 3)&&(isExtended (rightHand) < 3))//if closed fist
			{
				forceMult = 0.0f; //stop movement
			}
			
			transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), 0.1f);
			//transform.parent.rigidbody.velocity = transform.parent.forward * forceMult;
			//transform.parent.rigidbody.AddForce (transform.parent.forward * (forceMult / Time.fixedDeltaTime), ForceMode.Impulse);
			//transform.parent.rigidbody.velocity = new Vector3(transform.parent.forward.x * forceMult, 0.0f, transform.parent.forward.z * forceMult);
			
			//if(transform.parent.rigidbody.velocity.magnitude<=maxVelocity)
			//transform.parent.rigidbody.AddForce (transform.parent.forward * (forceMult / Time.fixedDeltaTime), ForceMode.Acceleration);
			translationAll = new Vector3(transform.parent.forward.x * forceMult, transform.parent.forward.y * forceMult, transform.parent.forward.z * forceMult);
			control.SimpleMove(translationAll);
		}
		else if(frame.Hands.Count == 0)
		{
			move--;
			if(move==0.0f)
			{
				//transform.parent.rigidbody.velocity = transform.parent.forward * 0;
				control.SimpleMove(translationAll*0.0f);
			}
		}
		
	}
}
