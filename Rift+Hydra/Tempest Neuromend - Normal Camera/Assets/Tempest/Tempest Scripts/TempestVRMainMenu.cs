using UnityEngine;
using System.Collections;

public class TempestVRMainMenu : VRGUI 
{
	Color backgroundColour = new Color (1.0f, 1.0f, 1.0f);
	Color buttonColour = new Color (0.22f, 1.0f, 0.97f);
	//Color textColour = new Color (1.0f, 1.0f, 1.0f);
	
	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	
	private Tempest.Menu.ProfileMenu profileMenu;
	private Tempest.RazorHydra.HydraControl hydraControl;
	
	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	
	public string firstLevel="OI Video Tutorial";
	
	//private LeapControl variables = null;
	
	bool sound;
	float volume;
	
	Texture2D background;
	private Rect backgroundPosition;
	
	void Awake()
	{
		GameObject gameControl=null;
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			GameControl gameControlScript=null;
			if(gameControlScript=gameControl.GetComponent<GameControl>())
			{
				hydraControl = gameControl.GetComponent<Tempest.RazorHydra.HydraControl>();
				gameControlScript.MenuActive();
			}
			//variables=gameControl.GetComponent<LeapControl>();
		}
	}
	
	
	void profile()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		
		profileMenu.Draw ();

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			profileMenu.BackToBeginning();

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
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight), "CONFIGURATION"))
		{
			menuFunction = configuration;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = mainMenu;
		}
	}
	
	float drawSlider(Rect rect, string title, float min, float max, ref float value)
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
		
		return value;
	}
	
	void configuration()
	{
		if(hydraControl != null)
		{
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight * 2.0f), "Left Joystick Sensitivity", 1.0f, 5.0f, ref hydraControl.m_leftJoystickSens);
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.3f, buttonWidth, buttonHeight * 2.0f), "Right Joystick Sensitivity ", 1.0f, 5.0f, ref hydraControl.m_rightJoystickSens);
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.4f, buttonWidth, buttonHeight * 2.0f), "Throw Sensitivity", 1.0f, 5.0f, ref hydraControl.m_throwSens);
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.5f, buttonWidth, buttonHeight * 2.0f), "Trigger Sensitivity", 1.0f, 5.0f, ref hydraControl.m_triggerSens);
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.6f, buttonWidth, buttonHeight * 2.0f), "Linear Hand Sensitivity", 1.0f, 5.0f, ref hydraControl.m_linearHandSens);
			drawSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.7f, buttonWidth, buttonHeight * 2.0f), "Angular Hand Sensitivity", 1.0f, 5.0f, ref hydraControl.m_angularHandSens);
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.8f, screenHeight * 0.8f, buttonWidth, buttonHeight), "BACK"))
		{
			menuFunction = settings;
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
		
		background = Resources.Load<Texture2D>("TitleProxy01");
		
		profileMenu = new Tempest.Menu.ProfileMenu ();
	}
	
	public override void OnVRGUI()
	{
		DrawBackground();
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
	
	void DrawBackground()
	{
		backgroundPosition.Set(	(Screen.width/2.0f)-512, (Screen.height/2.0f)-320, 1024, 640);
		GUI.DrawTexture(backgroundPosition,background);
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
			profileMenu.Initialize ();
			
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

	new void audio()
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
	
	
	
	// Update is called once per frame
	void Update () 
	{
		Screen.showCursor=false;
	}
}
