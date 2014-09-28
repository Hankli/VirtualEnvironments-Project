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

	private GameObject gameControlObject = null;

	private Tempest.RazorHydra.HydraControl m_control = new Tempest.RazorHydra.HydraControl();
	
	bool sound;
	float volume;

	void Awake()
	{
		if(gameControlObject = GameObject.FindGameObjectWithTag("Game"))
		{
		
		}
	}
	// Use this for initialization
	void Start () 
	{
		sound = true;
		volume = 5.0f;
		
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

	void DrawCustomSlider(Rect rect, string title, int min, int max, ref float value)
	{
		GUILayout.BeginArea(rect);
		GUILayout.Label(title);
		value = GUILayout.HorizontalSlider(value, min, max);
		GUILayout.BeginHorizontal();
		var defaultAlignment = GUI.skin.label.alignment;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUILayout.Label(min.ToString());
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUILayout.Label(max.ToString());
		GUI.skin.label.alignment = defaultAlignment;
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void configuration()
	{
		Color colorState = GUI.color;
		GUILayout.BeginArea (new Rect (0, 0, screenWidth, screenHeight * 0.08f));
		GUI.color = new Color (0.2f, 0.3f, 0.8f, 1.0f);
		GUI.skin.label.fontSize = 15; 
		GUILayout.Label ("Hydra Configuration");
		GUILayout.EndArea ();
		GUI.color = colorState;

		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight * 2.0f),
		                  "Character Move Sensitivity", 1, 5, ref m_control.m_characterMoveSensitivity);
		
		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight * 2.0f),
		                  "Camera Rotate Sensitivity", 1, 5, ref m_control.m_cameraRotateSensitivity);
		
		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight * 2.0f), 
		                  "Grip Sensitivity", 1, 5, ref m_control.m_grippingSensitivity);
		
		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.4f, buttonWidth, buttonHeight * 2.0f), 
		                  "Throw Sensitivity", 1, 5, ref m_control.m_throwingSensitivity);
		
		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.5f, buttonWidth, buttonHeight * 2.0f), 
		                  "Hand Move Sensitivity", 1, 5, ref m_control.m_handMoveSensitivity);
		
		DrawCustomSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.6f, buttonWidth, buttonHeight * 2.0f), 
		                  "Hand Rotate Sensitivity", 1, 5, ref m_control.m_handRotateSensitivity);
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}

	void mainMenu()
	{
		GUI.color = buttonColour;
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), "PLAY"))
		{
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
