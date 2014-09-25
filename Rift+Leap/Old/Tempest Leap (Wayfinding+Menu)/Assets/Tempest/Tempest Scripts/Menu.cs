using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	Color backgroundColour = new Color (0.2f, 0.2f, 0.2f);
	Color buttonColour = new Color (0.22f, 1.0f, 0.97f);
	Color textColour = new Color (1.0f, 1.0f, 1.0f);

	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;

	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;

	// Use this for initialization
	void Start () 
	{
		screenHeight = Screen.height;
		screenWidth = Screen.width;

		buttonHeight = Screen.height * 0.1f;
		buttonWidth = Screen.width * 0.2f;

		Camera.main.backgroundColor = backgroundColour;

		menuFunction = anyKey;
	}

	void OnGUI()
	{
		menuFunction();
	}

	void anyKey()
	{
		if(Input.anyKey) //if the user has pressed any key
		{
			menuFunction = mainMenu; //change the menu to main menu
		}

		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.45f, screenWidth * 0.1f, screenHeight * 0.1f), "Press any key to continue");

	}

	void mainMenu()
	{
		GUI.color = buttonColour;
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "PLAY"))
		{
			Application.LoadLevel ("Way Finding");
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight), "CONTROLS"))
		{
			menuFunction = controls;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.5f, buttonWidth, buttonHeight), "SETTINGS"))
		{
			menuFunction = settings;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.7f, buttonWidth, buttonHeight), "SCORES"))
		{
			menuFunction = scores;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.7f, buttonWidth, buttonHeight), "ABOUT"))
		{
			menuFunction = about;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.1f, buttonWidth, buttonHeight), "QUIT"))
		{
			Application.Quit ();
		}
	}

	void scores()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.45f, screenWidth * 0.1f, screenHeight * 0.1f), "*display scores here*");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}
	void controls()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.45f, screenWidth * 0.1f, screenHeight * 0.1f), "*insert controls here*");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void settings()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.45f, screenWidth * 0.1f, screenHeight * 0.1f), "*insert settings here*");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void about()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.45f, screenWidth * 0.1f, screenHeight * 0.1f), "*insert about here*");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.9f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
