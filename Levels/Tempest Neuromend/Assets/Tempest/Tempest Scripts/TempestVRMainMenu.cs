using UnityEngine;
using System.Collections;

public class TempestVRMainMenu : VRGUI 
{
	Color backgroundColour = new Color (1.0f, 1.0f, 1.0f);
	Color cursorColour = new Color (0.22f, 1.0f, 0.97f);
	Color buttonColour = new Color (1.0f, 1.0f, 1.0f);
	//Color textColour = new Color (1.0f, 1.0f, 1.0f);

	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	private Tempest.Menu.ProfileMenu profileMenu;

	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	
	private string firstLevel="OI Video Tutorial";//default... should to remove?
	private int firstLevelIndex=1;
	private int numberOfLevels = 0;
	private int[] levelIndexes=new int[7];


	public GUIStyle menuButtonStyle;//button GUIStyle
	public GUIStyle menuLabelStyle;//label GUIStyle
	public GUIStyle menuLabelStyleA;//label GUIStyle
	public GUIStyle menuToggleStyle;//toggle GUIStyle


	//private LeapControl variables = null;
	
	bool sound;
	float volume;
	bool twoHands;
	float sensitivity;

	private bool b_playTutorials=true;
	private bool b_objectInteraction=false;
	private bool b_objectAvoidance=false;
	private bool b_wayFinding=false;


	Texture2D background;
	Texture2D profileTitle;
	Texture2D settingsTitle;
	Texture2D aboutTitle;
	Texture2D exerciseTitle;
	Texture2D controlsTitle;
	Texture2D audioTitle;

	Texture2D titleTexture=null;

	private Rect profileTitlePosition;
	private Rect backgroundPosition;

	void Awake()
	{
		GameObject gameControl=null;
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			GameControl gameControlScript=null;
			if(gameControlScript=gameControl.GetComponent<GameControl>())
			{
				gameControlScript.MenuActive();
			}
			//variables=gameControl.GetComponent<LeapControl>();
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
		//menuFunction = anyKey;
		menuFunction = mainMenu;

		background = Resources.Load<Texture2D>("TitleProxy01");
		profileTitle = Resources.Load<Texture2D>("Profile01");
		settingsTitle = Resources.Load<Texture2D>("Settings01");
		aboutTitle = Resources.Load<Texture2D>("About01");
		exerciseTitle = Resources.Load<Texture2D>("Exercise01");
		controlsTitle = Resources.Load<Texture2D>("Controls01");
		audioTitle = Resources.Load<Texture2D>("Audio01");

		profileMenu = new Tempest.Menu.ProfileMenu ();
		menuButtonStyle.fontSize = (int)(Screen.width/40.0f);
		menuLabelStyle.fontSize = menuButtonStyle.fontSize;
		menuLabelStyleA.fontSize = menuButtonStyle.fontSize;
		menuToggleStyle.fontSize = menuButtonStyle.fontSize;
	}
	
	public override void OnVRGUI()
	{
		Screen.showCursor=false;
		if(screenHeight != Screen.height||screenWidth != Screen.width)
		{
			screenHeight = Screen.height;
			screenWidth = Screen.width;
			buttonHeight = Screen.height * 0.05f;
			buttonWidth = Screen.width * 0.2f;

			menuButtonStyle.fontSize = (int)(Screen.width/40.0f);
			menuLabelStyle.fontSize = menuButtonStyle.fontSize;
			menuLabelStyleA.fontSize = menuButtonStyle.fontSize;
			menuToggleStyle.fontSize = menuButtonStyle.fontSize;

		}

		DrawBackground();
		DrawTitle (titleTexture);
		menuFunction();
	}

	void DrawTitle(Texture2D name)
	{
		if(name!=null)
		{
			profileTitlePosition.Set(0, 0, Screen.width, Screen.height);
			GUI.DrawTexture(profileTitlePosition, name);
		}
	}

	void DrawBackground()
	{
		//backgroundPosition.Set(	(Screen.width/2.0f)-512, (Screen.height/2.0f)-320, 1024, 640);
		//GUI.DrawTexture(backgroundPosition,background);
		backgroundPosition.Set(	0, 0, Screen.width, Screen.height);
		GUI.DrawTexture(backgroundPosition, background);
	}

	void mainMenu()
	{
		GUI.color = buttonColour;

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "PROFILE", menuButtonStyle ))
		{
			titleTexture = profileTitle;
			menuFunction = profile;
		}

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "ABOUT", menuButtonStyle ))
		{
			titleTexture = aboutTitle;
			menuFunction = about;
		}

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "EXIT", menuButtonStyle ))
		{
			menuFunction = exitMain;
		}

		GUI.color = cursorColour;

	}

	void exitMain()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;


		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.4f, buttonWidth, buttonHeight), "Are you sure you want to exit?", menuLabelStyle);

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.25f, screenHeight * 0.5f, buttonWidth, buttonHeight), "YES", menuButtonStyle))
		{
			Application.Quit ();
		}
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.75f, screenHeight * 0.5f, buttonWidth, buttonHeight), "NO", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		

		GUI.color = cursorColour;

	}
	
	void profile()
	{
		//profileMenu.Draw ();

		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "SETTINGS", menuButtonStyle))
		{
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "START", menuButtonStyle))
		{
			titleTexture = exerciseTitle;
			menuFunction = levelSelect;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		GUI.color = cursorColour;
	}

	void levelSelect()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		//need to display current devices
		//


		//b_playTutorials = GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.09f, screenHeight * 0.2f, buttonWidth, buttonHeight), b_playTutorials, "");
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Play Tutorials", menuLabelStyleA);
		b_playTutorials = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.3f, buttonWidth*1.5f, buttonHeight), b_playTutorials, "Play Tutorials", menuToggleStyle);
		b_objectInteraction = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.4f, buttonWidth*1.5f, buttonHeight), b_objectInteraction, "Object Interaction", menuToggleStyle);
		b_objectAvoidance = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.47f, buttonWidth*1.5f, buttonHeight), b_objectAvoidance, "Object Avoidance", menuToggleStyle);
		b_wayFinding = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.54f, buttonWidth*1.5f, buttonHeight), b_wayFinding, "Way Finding", menuToggleStyle);


		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.3f, buttonWidth, buttonHeight), "Object Interaction", menuLabelStyleA);
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.37f, buttonWidth, buttonHeight), "Object Avoidance", menuLabelStyleA);
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.44f, buttonWidth, buttonHeight), "Way Finding", menuLabelStyleA);

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "START", menuButtonStyle))
		{
			//variables.SetTwoHands(twoHands);
			//variables.SetSensitivity(sensitivity);


			//set first level
			if(b_objectInteraction){firstLevelIndex = 3;}
			else if(b_objectAvoidance){firstLevelIndex = 5;}
			else if(b_wayFinding){firstLevelIndex = 7;}
			//adjust first level index if tutorials are to be played
			if(b_playTutorials){firstLevelIndex--;}


			//count levels and...
			//populate level index array for game control...
			numberOfLevels=0;
			if(b_objectInteraction)
			{
				if(b_playTutorials)
				{
					levelIndexes[numberOfLevels]=2;
					numberOfLevels++;
				}
				levelIndexes[numberOfLevels]=3;
				numberOfLevels++;

			}
			if(b_objectAvoidance)
			{
				if(b_playTutorials)
				{
					levelIndexes[numberOfLevels]=4;
					numberOfLevels++;
					
				}
				levelIndexes[numberOfLevels]=5;
				numberOfLevels++;
			}
			if(b_wayFinding)
			{
				if(b_playTutorials)
				{
					levelIndexes[numberOfLevels]=6;
					numberOfLevels++;
					
				}
				levelIndexes[numberOfLevels]=7;
				numberOfLevels++;
			}
			//score screen last
			levelIndexes[numberOfLevels]=8;
			numberOfLevels++;

			//send level indexes to gamecontrol...
			GameObject gameControl=null;
			if(gameControl=GameObject.FindWithTag("Game"))
			{
				GameControl gameControlScript=null;
				if(gameControlScript=gameControl.GetComponent<GameControl>())
				{
					gameControlScript.levelIndexes=levelIndexes;
					gameControlScript.numberOfLevels=numberOfLevels;
				}
				//variables=gameControl.GetComponent<LeapControl>();
			}

			if(b_objectInteraction||b_objectAvoidance||b_wayFinding)
			{
				//Application.LoadLevel (firstLevel);
				Application.LoadLevel (firstLevelIndex);
			}
			else
			{
				//display "please select at least one exercise..."
			}
		}

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = profileTitle;
			menuFunction = profile;
		}
		GUI.color = cursorColour;
	}

	void settings()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "CONTROLS", menuButtonStyle))
		{
			titleTexture = controlsTitle;
			menuFunction = controls;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "AUDIO", menuButtonStyle))
		{
			titleTexture = audioTitle;
			menuFunction = audio;
		}
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = profileTitle;
			menuFunction = profile;
		}
		GUI.color = cursorColour;
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

		twoHands = GUI.Toggle (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), twoHands, "Two hands", menuLabelStyle);

		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		GUI.color = cursorColour;
	}

	new void audio()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*display audio options here*", menuButtonStyle);
		
		sound = GUI.Toggle (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.1f, buttonWidth, buttonHeight), sound, "Sound", menuButtonStyle);
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.1f, buttonWidth, screenHeight), "Volume", menuLabelStyle);
		volume = GUI.HorizontalSlider (new Rect ((screenWidth - buttonWidth) * 0.2f, screenHeight * 0.2f, buttonWidth, buttonHeight), volume,0.0f,10.0f);
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		GUI.color = cursorColour;
	}
	
	
	void about()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.4f, screenHeight * 0.3f, screenWidth * 0.2f, screenHeight * 0.1f), "NEUROMEND\n\nBrought to you by TEMPEST");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		GUI.color = cursorColour;
	}

	void controls()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*insert controls here*");
		
		if(GUI.Button (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		GUI.color = cursorColour;
	}

	// Update is called once per frame
	void Update () 
	{
		Screen.showCursor=false;
	}
}
