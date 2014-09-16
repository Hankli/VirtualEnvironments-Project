using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using Leap;


class LeapListener : Leap.Listener
{
	float initialHeight;
	float move = 2.0f;
	float jump = 2.0f;
	Vector3 newPos;
	private CharacterController control;
	Controller m_leapController;
	
	public delegate void SwipeEvent(SwipeDirection sd);
	public event SwipeEvent LeapSwipe;
	
	private int fingersCount;
	

	// Use this for initialization
	void Start () 
	{
		initialHeight = control.height;
		Debug.Log ("height = " + initialHeight);
		

	}

	void Awake()
	{
		m_leapController = new Controller();
		
		//controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
		//controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
		//controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
		m_leapController.Config.SetFloat("Gesture.Swipe.MinLength", 150); //10
		m_leapController.Config.SetFloat("Gesture.Swipe.MinVelocity", 500); //100
		m_leapController.Config.Save();
		m_leapController.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
	}

	
	// Update is called once per frame
	void FixedUpdate () 
	{
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
	
	public void SwipeAction(FingerList fingers, SwipeDirection sd)
	{
		fingersCount = fingers.Count;
		newPos = new Vector3(control.transform.position.x, control.transform.position.y, control.transform.position.z);
		//newPos = new Vector3((control.transform.right.x), (control.transform.up.y), (control.transform.forward.z));
		//if (fingersCount == 5)
		{
			switch (sd)
			{
			case SwipeDirection.Left:
				if (LeapSwipe != null)
				{
					LeapSwipe(SwipeDirection.Left);
					Debug.Log ("LEFT");
				}
				break;
			case SwipeDirection.Right:
				if (LeapSwipe != null)
				{
					LeapSwipe(SwipeDirection.Right);
					Debug.Log ("RIGHT");
				}
				break;
			case SwipeDirection.Up:
				if (LeapSwipe != null)
			{

				LeapSwipe(SwipeDirection.Up);
				Debug.Log ("UP");
			}
				break;
			case SwipeDirection.Down:
				if (LeapSwipe != null)
			{
				LeapSwipe(SwipeDirection.Down);
				Debug.Log ("DOWN");
			}
				break;
			}
		}
	}
}
