using UnityEngine;
using System.Collections;
using Leap;

public class LeapMove : MonoBehaviour 
{
	Controller m_leapController;
	float smooth = 1.5f; //speed at which the camera will move
	float speed = 0.0f;
	Transform player; //reference to the player's transform
	Vector3 pos; //current position 
	Vector3 newPos; //position trying to reach
	Vector3 moveForce;
	float stopping;
	float forceMult;
	//float maxVelocity;
	Vector3 translationAll;

	private CharacterController control;
	
	
	// Use this for initialization
	void Start () 
	{

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


		//CharacterController cc = gameObject.GetComponent<CharacterController> ();
		HandList hands = frame.Hands;
		Hand firstHand = hands[0];
		float pitch = firstHand.Direction.Pitch;
		float yaw = firstHand.Direction.Yaw;
		float roll = firstHand.PalmNormal.Roll;
		Vector position = firstHand.PalmPosition;
		Vector velocity = firstHand.PalmVelocity;
		Vector direction = firstHand.Direction;
		Vector normal = firstHand.PalmNormal;

		//The change of position of this hand between the current frame and the specified frame.
		Vector handMovement = (firstHand.Translation(lastFrame));

		Debug.Log ("handMovement = " + handMovement);

		if (frame.Hands.Count >= 1) 
		{
			Vector3 newRot = transform.parent.localRotation.eulerAngles;

			speed = 1.0f;
			float rotSpeed = 1.0f;



			/*float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * speed;
			
			rotationY += Input.GetAxis("Mouse Y") * speed;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);*/

			//newRot.x = (control.transform.forward.x * handMovement.y * speed);
			//newRot.y = (control.transform.forward.y * handMovement.y * speed);
			//newRot.z = (control.transform.right.z * -handMovement.x * speed);
			//Vector3 m = (control.transform.forward * handMovement.y) + (control.transform.right * -handMovement.x);
			//newRot.x += frame.RotationAngle(lastFrame, Vector.XAxis); //, transform.localEulerAngles.x); //rotation around parents axis
			//newRot.y += frame.RotationAngle(lastFrame, Vector.YAxis); //, transform.localEulerAngles.y);
			//newRot.z += frame.RotationAngle(lastFrame, Vector.ZAxis); //, transform.localEulerAngles.z);
			//newRot.x = normal.Pitch * rotSpeed;
			//newRot.y += -normal.Roll * rotSpeed;
			newRot.y += -roll * rotSpeed;
			//newRot.z = normal.Yaw * rotSpeed;
			//Debug.Log ("newRot = " + newRot);
			transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), 0.1f);

			/*if (isExtended (firstHand) > 3) {//if open hands
					speed = 1.0f;
			} else if (isExtended (firstHand) < 3) {//if closed fist
					speed = -1.0f; //stop movement
			}*/

			//transform.parent.position += ((control.transform.forward * firstHandMovement.y * speed)  + (control.transform.right * -firstHandMovement.x * speed));
			//Vector3 m = (control.transform.forward * handMovement.y * speed) + (control.transform.right * -handMovement.x * speed);
			//control.Move(Vector3.Scale(m, moveForce) * Time.fixedDeltaTime);
			//Vector3 newPos = (transform.parent.localPosition + m);
			Vector3 newPos = new Vector3((transform.parent.forward.x * speed), (transform.parent.forward.y * speed), (transform.parent.forward.z * speed));

			control.SimpleMove (newPos);
		}
		//control.Move(m * Time.fixedDeltaTime);
		//translationAll = new Vector3(transform.parent.forward.x * forceMult, transform.parent.forward.y * forceMult, transform.parent.forward.z * forceMult);
		//control.SimpleMove(translationAll);
		

		/*Vector3 newRot = transform.parent.localRotation.eulerAngles;

		newRot.x = firstHand.RotationAngle(lastFrame, Vector.XAxis);
		newRot.y = firstHand.RotationAngle(lastFrame, Vector.YAxis);
		newRot.z = firstHand.RotationAngle(lastFrame, Vector.ZAxis);*/
		//Matrix handRotationTransform = firstHand.RotationMatrix(lastFrame);
		//Vector newPosition = thisMatrix.TransformPoint(oldPosition);
		//Vector newPosition = handRotationTransform.TransformPoint(transform.parent.position); //position
		//Vector newDirection = thisMatrix.TransformDirection(oldDirection);
		//Vector newDirection = handRotationTransform.TransformDirection(transform.parent.rotation); //direction
		//Vector newRot = handRotationTransform.TransformDirection(transform.parent.rotation); //direction
	

		//transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation, Quaternion.Euler(newRot), 0.1f);
		//transform.parent.localPosition 
		//Debug.Log ("m = " + m);
		//transform.parent.localPosition = (transform.parent.localPosition + m);
		
		//Vector axisOfHandRotation = firstHand.RotationAxis(lastFrame);
		//Debug.Log ("axisOfHandRotation = " + axisOfHandRotation);
		
		if (frame.Hands.Count >= 2)
		{
			stopping = 5.0f;//change for time it takes to stop after hands leave
			Hand leftHand = GetLeftMostHand(frame);
			Hand rightHand = GetRightMostHand(frame);

			//The change of position of this hand between the current frame and the specified frame.
			Vector leftHandMovement = leftHand.Translation(frame);
			Debug.Log ("leftHandMovement = " + leftHandMovement);

			// takes the average vector of the forward vector of the hands, used for the
			// pitch of the camera.
			Vector3 avgPalmForward = (frame.Hands[0].Direction.ToUnity() + frame.Hands[1].Direction.ToUnity()) * 0.1f;
			Debug.Log ("avgPalmForward = " + avgPalmForward);

			//frame.Translation();

			Vector3 move = (control.transform.forward * avgPalmForward.y)  + (control.transform.right * avgPalmForward.x);
			control.Move(Vector3.Scale(move, moveForce) * Time.fixedDeltaTime);



			/*
			
			Vector3 handDiff = leftHand.PalmPosition.ToUnityScaled() - rightHand.PalmPosition.ToUnityScaled();
			Debug.Log ("Hand diff = " + handDiff);
			Debug.Log ("x = " + handDiff.x);
			Debug.Log ("y = " + handDiff.y);
			Debug.Log ("x = " + handDiff.z);

			Vector3 newRot = transform.parent.localRotation.eulerAngles;
			Vector3 newPos = transform.parent.localPosition.Normalize();

			//newRot.z = -handDiff.y * 100.0f; //alter for angle
			
			// adding the rot.z as a way to use banking (rolling) to turn.
			//newRot.y += handDiff.z * 3.0f - newRot.z * 0.03f * transform.parent.rigidbody.velocity.magnitude;
			//newRot.y = (avgPalmForward.x) * 2500.0f;
			//newRot.x = -(avgPalmForward.y - 0.1f) * 100.0f;
			
			if ((isExtended (leftHand) > 3)&&(isExtended (rightHand) > 3))//if open hands
			{
				//if(newPos.x >= 0.0f)
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
			transform.parent.localPosition = Quaternion.Slerp(transform.parent.localPosition, Quaternion.Euler(newPos), 0.1f);
			//transform.parent.rigidbody.velocity = transform.parent.forward * forceMult;
			//transform.parent.rigidbody.AddForce (transform.parent.forward * (forceMult / Time.fixedDeltaTime), ForceMode.Impulse);
			//transform.parent.rigidbody.velocity = new Vector3(transform.parent.forward.x * forceMult, 0.0f, transform.parent.forward.z * forceMult);
			
			//if(transform.parent.rigidbody.velocity.magnitude<=maxVelocity)
			//transform.parent.rigidbody.AddForce (transform.parent.forward * (forceMult / Time.fixedDeltaTime), ForceMode.Acceleration);
			translationAll = new Vector3(transform.parent.forward.x * forceMult, transform.parent.forward.y * forceMult, transform.parent.forward.z * forceMult);
			control.SimpleMove(translationAll);*/
		}
		else if(frame.Hands.Count == 0)
		{
			stopping--;
			if(stopping==0.0f)
			{
				//transform.parent.rigidbody.velocity = transform.parent.forward * 0;
				//control.SimpleMove(0.0f);
			}
		}
		
	}
}
