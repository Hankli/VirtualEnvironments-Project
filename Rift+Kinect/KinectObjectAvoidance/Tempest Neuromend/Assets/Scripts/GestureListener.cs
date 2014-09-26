﻿using UnityEngine;
using System.Collections;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	public GUIText GestureInfo;
	
	private bool swipeLeft;
	private bool swipeRight;
	private bool swipeUp;
	private bool swipeDown;
	private bool pull;
	private bool push;

	public bool IsSwipeLeft()
	{
		if(swipeLeft)
		{
			swipeLeft = false;
			return true;
		}
		
		return false;
	}
	
	public bool IsSwipeRight()
	{
		if(swipeRight)
		{
			swipeRight = false;
			return true;
		}
		
		return false;
	}
	
	public bool IsSwipeUp()
	{
		if(swipeUp)
		{
			swipeUp = false;
			return true;
		}
		
		return false;
	}
	
	public bool IsSwipeDown()
	{
		if(swipeDown)
		{
			swipeDown = false;
			return true;
		}
		
		return false;
	}

	public bool IsPull()
	{
		if (pull)
		{
			pull = false;
			return(true);
		}

		return(false);
	}

	public bool IsPush()
	{
		if(push)
		{
			push = false;
			return(true);
		}

		return(false);
	}
	
	
	public void UserDetected(uint userId, int userIndex)
	{
		// detect these user specific gestures
		KinectManager manager = KinectManager.Instance;
		
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.Pull);
		manager.DetectGesture(userId, KinectGestures.Gestures.Push);

		//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
		//		manager.DetectGesture(userId, KinectGestures.Gestures.SwipeDown);
		
		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = "Pull to start walking, Push to stop";
		}
	}
	
	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = string.Empty;
		}
	}
	
	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		// don't do anything here
	}
	
	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		if(GestureInfo != null)
		{
			GestureInfo.guiText.text = sGestureText;
		}

        if (gesture == KinectGestures.Gestures.SwipeLeft)
            swipeLeft = true;
        else if (gesture == KinectGestures.Gestures.SwipeRight)
            swipeRight = true;
        else if (gesture == KinectGestures.Gestures.SwipeUp)
            swipeUp = true;
        else if (gesture == KinectGestures.Gestures.SwipeDown)
            swipeDown = true;
        else if (gesture == KinectGestures.Gestures.Pull)
            pull = true;
        else if (gesture == KinectGestures.Gestures.Push)
            push = true;
			
		return true;
	}
	
	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.SkeletonJoint joint)
	{
		// don't do anything here, just reset the gesture state
		return true;
	}
}
