using UnityEngine;
using System.Collections;
using System;

public class SimpleGestureListenerWF : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public GUIText GestureInfo;

    public GameObject character; //the character to pass gesture control too
    private Transform throwDir; //the throwing object direction
    private GameObject throwObj; //the object to throw

    public ObjectMove charMove;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    private bool swipeLeft;
    private bool swipeRight;
    private bool pushG; //the push gesture
    private bool pullG; //the pull gesture
    private bool jump; //jumping
    private bool crouch; //crouching

    public bool IsSwipeLeft()
    {
        charMove.isRotateLeft = true; //rotate this direction
        charMove.isRotateRight = false; //turn off other rotation
        return false;
    }

    public bool IsSwipeRight()
    {
        charMove.isRotateRight = true; //rotate this direction
        charMove.isRotateLeft = false; //turn off other rotate
        return false;
    }

    public bool IsPush()
    {/*
        Debug.Log("PUSH");
        throwObj = GameObject.FindGameObjectWithTag("Throwable");
        if (throwObj.GetComponent<ThrowableObject>().doTrans == true)
        {
            Vector3 p = new Vector3(0, 0, 0);
            throwDir = throwObj.transform;
            throwDir.collider.attachedRigidbody.useGravity = true;
            GameObject lHand = GameObject.FindGameObjectWithTag("LeftHand");
            GameObject rHand = GameObject.FindGameObjectWithTag("RightHand");
            if (lHand.GetComponent<hands>().isHolding == true)
            {
                p = lHand.transform.position;
                lHand.GetComponent<hands>().isHolding = false;
            }
            else if (rHand.GetComponent<hands>().isHolding == true)
            {
                p = rHand.transform.position;
                rHand.GetComponent<hands>().isHolding = false;
            }
            Ray ray3 = Camera.main.ScreenPointToRay(p);
            throwDir.collider.attachedRigidbody.AddForce(ray3.direction * 10, ForceMode.Impulse);
            throwObj.GetComponent<ThrowableObject>().doTrans = false;
        }*/
        return (true);
    }

    public bool isPull()
    {
        //charMove.isMove = true; //move forward

        //character.transform.position = character.transform.position - new Vector3(10.0f, 0.0f, 0.0f);
        return (true);
    }

    public bool isJump()
    {
        charMove.isJump = true;
        return (true);
    }

    public bool isCrouch()
    {
        charMove.isCrouch = true;
        return (true);
    }

    public void UserDetected(uint userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.Push);
        manager.DetectGesture(userId, KinectGestures.Gestures.Pull);
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.Squat);

        //		manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeUp);
        //		manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeDown);

        if (GestureInfo != null)
        {
            GestureInfo.guiText.text = "SwipeLeft, SwipeRight, Jump, Push or Pull.";
        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        if (GestureInfo != null)
        {
            GestureInfo.guiText.text = string.Empty;
        }
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
        float progress, KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
    {
        /*
		//GestureInfo.guiText.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));
		if(gesture == KinectGestures.Gestures.Click && progress > 0.3f)
		{
			string sGestureText = string.Format ("{0} {1:F1}% complete", gesture, progress * 100);
			if(GestureInfo != null)
				GestureInfo.guiText.text = sGestureText;
			
			progressDisplayed = true;
		}		
		else if((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
			if(GestureInfo != null)
				GestureInfo.guiText.text = sGestureText;
			
			progressDisplayed = true;
		}
		else if(gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
			if(GestureInfo != null)
				GestureInfo.guiText.text = sGestureText;
			
			progressDisplayed = true;
		}*/
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
    {
        string sGestureText = gesture + " detected";
        Debug.Log(sGestureText);
        /*
		if(gesture == KinectGestures.Gestures.Click)
			sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
		
		if(GestureInfo != null)
			GestureInfo.guiText.text = sGestureText;
		
		progressDisplayed = false;
         */
        if (GestureInfo != null)
        {
            GestureInfo.guiText.text = sGestureText;
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
        {
            IsSwipeLeft();
            swipeLeft = true;
        }
        else if (gesture == KinectGestures.Gestures.SwipeRight)
        {
            IsSwipeRight();
            swipeRight = true;
        }
        else if (gesture == KinectGestures.Gestures.Push)
        {
            IsPush();
            pushG = true;
        }
        else if (gesture == KinectGestures.Gestures.Pull)
        {
            isPull();
            pullG = true;
        }
        else if (gesture == KinectGestures.Gestures.Jump)
        {
            isJump();
            jump = true;
        }
        else if (gesture == KinectGestures.Gestures.Squat)
        {
            isCrouch();
            crouch = true;
        }

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.SkeletonJoint joint)
    {
        /*
		if(progressDisplayed)
		{
			// clear the progress info
			if(GestureInfo != null)
				GestureInfo.guiText.text = String.Empty;
			
			progressDisplayed = false;
		}*/

        return true;
    }

}
