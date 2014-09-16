using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using Leap;

public class LeapGestures : MonoBehaviour 
{
	float initialHeight;
	float move = 20.0f;
	float jump = 2.0f;

	bool b_swipeable=true;
	float swipeStart=0.0f;
	float timeCheck=0.0f;
	float swipeDelay=1.0f;//time allowed between swipes

	Vector3 newPos;
	private CharacterController control;
	Controller m_leapController;

	delegate void SwipeEvent(SwipeDirection sd);
	event SwipeEvent LeapSwipe;
	
	private int fingersCount;

	enum SwipeDirection
	{
		Up,
		Down,
		Left,
		Right
	}

	// Use this for initialization
	void Start () 
	{
		control = transform.parent.GetComponent<CharacterController>();
		initialHeight = control.height;
		Debug.Log ("height = " + initialHeight);

		m_leapController = new Controller();

		if (transform.parent == null)
		{
			Debug.LogError("LeapMove must have a parent object to control (eg camera)");
		}
		//controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
		//controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
		//controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
		m_leapController.Config.SetFloat("Gesture.Swipe.MinLength", 150); //10
		m_leapController.Config.SetFloat("Gesture.Swipe.MinVelocity", 1000); //100
		m_leapController.Config.Save();
		m_leapController.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		timeCheck = Time.time;
		if(timeCheck-swipeStart>=swipeDelay&&b_swipeable==false)
		{
			b_swipeable=true;
		}

		Frame currentFrame = m_leapController.Frame();
		HandList hands = currentFrame.Hands;

		if (!hands.IsEmpty)
		{
			Debug.Log ("hands");
			Hand firstHand = currentFrame.Hands[0];
			FingerList fingers = firstHand.Fingers;
			
			if (!fingers.IsEmpty)
			{
				GestureList gestures = currentFrame.Gestures();
				foreach (Gesture gst in gestures)
				{
					swipeStart=Time.time;

					if(b_swipeable)
					{
						b_swipeable=false;
						SwipeGesture swipe = new SwipeGesture(gst);
						if (Math.Abs(swipe.Direction.x) > Math.Abs(swipe.Direction.z)) // Horizontal swipe
						{
							if (swipe.Direction.x > 0) // right swipe
							{
								Debug.Log ("Left swpie");
								SwipeAction(fingers, SwipeDirection.Left);
							}
							else // left swipe
							{
								Debug.Log ("Right swipe");
								SwipeAction(fingers, SwipeDirection.Right);
							}
						}
						else // Vertical swipe
						{
							if (swipe.Direction.z > 0) // upward swipe
							{
								Debug.Log ("Down swipe");
								SwipeAction(fingers, SwipeDirection.Down);
							}
							else // downward swipe
							{
								Debug.Log ("Up swipe");
								SwipeAction(fingers, SwipeDirection.Up);
							}
						}
					}
				}
			}
		}
	}
	
	void SwipeAction(FingerList fingers, SwipeDirection sd)
	{
		fingersCount = fingers.Count;
		newPos = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z);
		//newPos = new Vector3((control.transform.right.x), (control.transform.up.y), (control.transform.forward.z));
		if (fingersCount == 5)
		{
			switch (sd)
			{
			case SwipeDirection.Left:
				newPos.x -= move;
				control.SimpleMove (newPos);
				Debug.Log ("LEFT");
				break;
			case SwipeDirection.Right:
				newPos.x += move;
				control.SimpleMove (newPos);
				Debug.Log ("RIGHT");
				break;
			case SwipeDirection.Up:
				Debug.Log ("newPos.y = " + newPos.y);
				newPos.y += initialHeight;
				//newPos.y * jump; // * Time.deltaTime;
				Debug.Log ("newnewPos.y = " + newPos.y);
				control.Move (newPos);
				Debug.Log ("UP");
				break;
			case SwipeDirection.Down:
				newPos.y += initialHeight;
				control.Move (newPos);
				Debug.Log ("DOWN");
				break;
			}
		}
	}
}
