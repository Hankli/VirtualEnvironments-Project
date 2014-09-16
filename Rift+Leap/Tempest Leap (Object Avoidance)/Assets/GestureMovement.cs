using System;
using System.Linq;
using UnityEngine;
using System.ComponentModel;
using Leap;

public enum SwipeDirection
{
	Up,
	Down,
	Left,
	Right
} 

class GestureMovement : MonoBehaviour
{
	Controller m_leapController;
	LeapListener listener; 

	// Use this for initialization
	void Start()
	{
		Debug.Log ("start");
		m_leapController = new Controller();

		listener = new LeapListener ();

		m_leapController.AddListener (listener);
		listener.LeapSwipe += SwipeAction;
	}

	private void SwipeAction(SwipeDirection sd)
	{
		Debug.Log ("swipeaction");
		switch (sd)
		{
		case SwipeDirection.Up:
			Debug.Log ("up");
			break;
		case SwipeDirection.Down:
			Debug.Log ("down");
			break;
		case SwipeDirection.Left:
			Debug.Log ("left");
			break;
		case SwipeDirection.Right:
			Debug.Log ("right");
			break;
		}
	}
	
	//public void DisposeListener()
	public void OnApplicationQuit()
	{
		Debug.Log ("quit");
		m_leapController.RemoveListener(listener);
		listener.Dispose();
		m_leapController.Dispose();
	}
	
	public event PropertyChangedEventHandler PropertyChanged;
	
	private void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
