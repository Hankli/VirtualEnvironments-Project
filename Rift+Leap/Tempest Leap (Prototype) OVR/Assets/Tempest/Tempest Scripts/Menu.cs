using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	Color backgroundColour = new Color (1.0f, 1.0f, 1.0f);
	Color buttonColour = new Color (0.22f, 1.0f, 0.97f);
	Color textColour = new Color (1.0f, 1.0f, 1.0f);
	
	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	
	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;

	private LeapControl variables = null;
	private GameObject gameControlObject = null;

	
	bool sound;
	float volume;
	bool twoHands;
	float sensitivity;

	void Awake()
	{
		if(gameControlObject = GameObject.FindGameObjectWithTag("Game"))
		{
			if(variables=gameControlObject.GetComponent<LeapControl>())
			{
				//bleh
			}
		}

	}
	// Use this for initialization
	void Start () 
	{
		sound = true;
		volume = 5.0f;
		twoHands = false;
		sensitivity = 5.0f;
		
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
			variables.SetTwoHands(twoHands);
			variables.SetSensitivity(sensitivity);
			Application.LoadLevel ("Way Finding");
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

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "CONFIGURATION"))
		{
			menuFunction = configuration;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), "CONTROLS"))
		{
			menuFunction = controls;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight), "AUDIO"))
		{
			menuFunction = audio;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void configuration()
	{
		float min = 0.0f;
		float max = 10.0f;
		GUI.color = buttonColour;

		GUILayout.BeginArea(new Rect((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight*2.0f));
		GUILayout.Label("Sensitivity");
		sensitivity = GUILayout.HorizontalSlider(sensitivity, min, max);
			GUILayout.BeginHorizontal();
				var defaultAlignment = GUI.skin.label.alignment;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUILayout.Label(min.ToString());
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUILayout.Label(max.ToString());
				GUI.skin.label.alignment = defaultAlignment;
				GUILayout.EndHorizontal();
		GUILayout.EndArea();

		twoHands = GUI.Toggle (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), twoHands, "Two hands");

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = settings;
		}
	}
	
	void audio()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*display audio options here*");
		
		sound = GUI.Toggle (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), sound, "Sound");
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.1f, buttonWidth, screenHeight), "Volume");
		volume = GUI.HorizontalSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), volume,0.0f,10.0f);
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
