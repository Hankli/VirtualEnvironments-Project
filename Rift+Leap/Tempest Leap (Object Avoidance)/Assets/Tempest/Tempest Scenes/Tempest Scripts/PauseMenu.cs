using System;
using UnityEngine;

public class pausemenu : MonoBehaviour
{
	bool Pauseenabled;
	
	void OnGUI()
	{
		if (Pauseenabled == true)
		{
			if (GUI.Button( new Rect(300, 300, 100, 80), "Main Menu"))
			{
				Application.LoadLevel("Main Menu");
			}
		}
	}
	
	// Use this for initialization
	void Start()
	{
		Pauseenabled = false;
		Time.timeScale = 1.0F;
		Screen.showCursor = false;
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("escape"))
		{
			if (Pauseenabled == true)
			{
				Pauseenabled = false;
				Time.timeScale = 1.0F;
				Screen.showCursor = false;
			}
		}
		
		else if (Pauseenabled == false)
		{
			Pauseenabled = true;
			Time.timeScale = 0.0F;
			Screen.showCursor = true;
			
		}
	}
}