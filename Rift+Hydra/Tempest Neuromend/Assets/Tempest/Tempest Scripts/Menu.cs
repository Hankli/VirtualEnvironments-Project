using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	Color backgroundColour = new Color (1.0f, 1.0f, 1.0f);
	Color buttonColour = new Color (0.22f, 1.0f, 0.97f);
	//Color textColour = new Color (1.0f, 1.0f, 1.0f);

	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;

	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	
	public string firstLevel="OI Video Tutorial";

	// Use this for initialization
	void Start () 
	{
		screenHeight = Screen.height;
		screenWidth = Screen.width;

		buttonHeight = Screen.height * 0.05f;
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

		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.35f, screenHeight * 0.3f, screenWidth * 0.3f, screenHeight * 0.1f), "Press any key to continue");

	}

	void mainMenu()
	{
		GUI.color = buttonColour;
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "PLAY"))
		{
			Application.LoadLevel (firstLevel);
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), "PROFILE"))
		{
			menuFunction = profile;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight), "SETTINGS"))
		{
			menuFunction = settings;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.4f, buttonWidth, buttonHeight), "ABOUT"))
		{
			menuFunction = about;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "QUIT"))
		{
			Application.Quit ();
		}
	}

	void profile()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "LOAD"))
		{
			menuFunction = loadProfile;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), "SCORES"))
		{
			menuFunction = scores;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void settings()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "CONTROLS"))
		{
			menuFunction = controls;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), "AUDIO"))
		{
			menuFunction = audio;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	new void audio()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*display audio options here*");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = settings;
		}
	}
	
	void about()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.4f, screenHeight * 0.3f, screenWidth * 0.2f, screenHeight * 0.1f), "NEUROMEND\n\nBrought to you by TEMPEST");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void scores()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*display scores here*");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = profile;
		}
	}

	void controls()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*insert controls here*");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = settings;
		}
	}
	void loadProfile()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*List profiles to load*");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = profile;
		}
	}



	// Update is called once per frame
	void Update () 
	{
	
	}
}
