using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class TempestVRMainMenu : MonoBehaviour
//public class TempestVRMainMenu : VRGUI 
{
	Color backgroundColour = new Color(1.0f, 1.0f, 1.0f);
	//Color backgroundColour = new Color(0.0f, 0.0f, 0.0f);
	Color cursorColour = new Color(0.22f, 1.0f, 0.97f);
	Color buttonColour = new Color(1.0f, 1.0f, 1.0f);

	private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	private Tempest.Menu.ProfileMenu profileMenu;

	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	
	//private string firstLevel="OI Video Tutorial";//default... should to remove?
	private int firstLevelIndex=1;
	private int numberOfLevels = 0;
	private int[] levelIndexes=new int[7];


	public GUIStyle menuButtonStyle;//button GUIStyle
	public GUIStyle menuLabelStyle;//label GUIStyle
	public GUIStyle menuLabelStyleA;//label GUIStyle
	public GUIStyle menuLabelStyleB;//label GUIStyle
	public GUIStyle menuLabelStyleC;//label GUIStyle left align
	public GUIStyle menuLabelStyleD;//label GUIStyle center align
	public GUIStyle menuToggleStyle;//toggle GUIStyle


	//private LeapControl variables = null;
	
	bool sound;
	bool music;
	float volume;
	bool twoHands;
	float sensitivity;

	float playerSpeedOA;
	float playerSpeedWF;

	bool b_Oculus=true;

	private bool b_playTutorials=true;
	private bool b_objectInteraction=false;
	private bool b_objectAvoidance=false;
	private bool b_wayFinding=false;


	Texture2D background;
	Texture2D profileTitle;
	Texture2D settingsTitle;
	Texture2D aboutTitle;
	Texture2D exerciseTitle;
//	Texture2D controlsTitle;
	Texture2D audioTitle;
//	Texture2D helpTitle;
	Texture2D helpHowToPlayTitle;
	Texture2D settingsConfigTitle;
	Texture2D neuromendIcon;
	Texture2D setupTitle;
	Texture2D usageTitle;

	Texture2D titleTexture = null;

	//oculus setup images
	Texture2D orimg1;
	Texture2D orimg2;
	Texture2D orimg3;
	Texture2D orimg4;
	Texture2D orimg5;
	Texture2D orimg6;
	//oculus setup images

	//kinect setup images
	Texture2D kinect1;
	Texture2D kinect2;
	Texture2D kinect3;
	//kinect setup images


	//leap setup images
	Texture2D leap1;
	Texture2D leap2;
	Texture2D leap3;
	Texture2D leap4;
	Texture2D leap5;
	Texture2D leap6;
	Texture2D leap7;
	//leap setup images

	//hydra setup images
	//hydra setup images


	private Rect profileTitlePosition;
	private Rect backgroundPosition;
	private Rect iconPosition;

	private Rect videoPosition;
	public MovieTexture OATutorial = null;
	public MovieTexture WFTutorial = null;
	public MovieTexture OITutorial = null;

	private MovieTexture video=null;

	private bool b_OATutVid = true;
	private bool b_WFTutVid = false;
	private bool b_OITutVid = false;

	private bool b_VideoStarted = false;
	private bool b_VideoFinished = false;
	private bool b_VideoPaused = false;

	private string playButtonText="PLAY";
	private float videoHeight=0.0f;
	private float videoWidth=0.0f;

	void Awake()
	{
		GameObject gameControl=null;
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			GameControl gameControlScript=null;
			if(gameControlScript=gameControl.GetComponent<GameControl>())
			{
				gameControlScript.MenuActive();


				//should be using option here or detecting which device/s are connected...
				gameControlScript.SetControllerType(GameControl.ControllerType.MouseKeyboard);
				//gameControlScript.SetControllerType(GameControl.ControllerType.OculusLeap);
				//gameControlScript.SetControllerType(GameControl.ControllerType.OculusHydra);
				//gameControlScript.SetControllerType(GameControl.ControllerType.OculusKinect);
			}
			//variables=gameControl.GetComponent<LeapControl>();
		}

	}


	// Most of the ratios are to compensate having the vrgui 'square' look on a normal camera. If using actual VR HMDs you need to change the ratios.
	void Start() 
	{
		if(TempestUtil.OVRConnectionCheck())
		{
			b_Oculus=true;
		}
		else
		{
			b_Oculus=false;
		}
		
		sound = true;
		music = true;
		volume = 1.0f;
		twoHands = false;
		sensitivity = 5.0f;
		playerSpeedOA = 2.0f;
		playerSpeedWF = 5.0f;

		screenHeight = Screen.height;
		screenWidth = Screen.width;
		
		buttonHeight = Screen.height * 0.05f;
		buttonWidth = Screen.width * 0.2f;

		//videoWidth = Screen.width * 0.9f;
		videoWidth = Screen.width * 0.6f;
		videoHeight = Screen.height * 0.6f;

		Camera.main.backgroundColor = backgroundColour;
		//menuFunction = anyKey;
		menuFunction = mainMenu;

		background = Resources.Load<Texture2D>("TitleProxy01");
		profileTitle = Resources.Load<Texture2D>("Profile01");
		settingsTitle = Resources.Load<Texture2D>("Settings01");
		aboutTitle = Resources.Load<Texture2D>("About01");
		exerciseTitle = Resources.Load<Texture2D>("Exercise01");
//		controlsTitle = Resources.Load<Texture2D>("Controls01");
		audioTitle = Resources.Load<Texture2D>("Audio01");
//		helpTitle = Resources.Load<Texture2D>("Help");
		helpHowToPlayTitle = Resources.Load<Texture2D>("HelpHowToPlay");
		settingsConfigTitle = Resources.Load<Texture2D>("Config");
		setupTitle = Resources.Load<Texture2D>("Setup");
		usageTitle = Resources.Load<Texture2D>("Usage");

		orimg1 = Resources.Load<Texture2D>("or1");
		orimg2 = Resources.Load<Texture2D>("or2");
		orimg3 = Resources.Load<Texture2D>("or3");
		orimg4 = Resources.Load<Texture2D>("or4");
		orimg5 = Resources.Load<Texture2D>("or5");
		orimg6 = Resources.Load<Texture2D>("or6");

		kinect1 = Resources.Load<Texture2D>("kinectDevice1");
		kinect2 = Resources.Load<Texture2D>("kinectDevice2");
		kinect3 = Resources.Load<Texture2D>("kinectUsage1");

		leap1 = Resources.Load<Texture2D>("Leap1");
		leap2 = Resources.Load<Texture2D>("Leap2");
		leap3 = Resources.Load<Texture2D>("Leap3");
		leap4 = Resources.Load<Texture2D>("Leap4");
		leap5 = Resources.Load<Texture2D>("Leap5");
		leap6 = Resources.Load<Texture2D>("LeapIntArea");
		leap7 = Resources.Load<Texture2D>("LeapVisualiser");

		neuromendIcon = Resources.Load<Texture2D>("Neuromend_Icon01");

		profileMenu = new Tempest.Menu.ProfileMenu ();
		menuButtonStyle.fontSize = (int)(Screen.width/40.0f);
		menuLabelStyle.fontSize = menuButtonStyle.fontSize;
		menuLabelStyleA.fontSize = menuButtonStyle.fontSize;
		menuToggleStyle.fontSize = menuButtonStyle.fontSize;
		menuLabelStyleB.fontSize = menuButtonStyle.fontSize;
		menuLabelStyleC.fontSize = (int)(menuButtonStyle.fontSize * 0.5f);
		menuLabelStyleD.fontSize = (int)(menuButtonStyle.fontSize * 0.5f);

		profileMenu.Initialize ();
		ConfigMenuValues();
		//ConfigGameControl();
	}

	//run before loading levels...
	public void ConfigGameControl()
	{
		GameObject gameControl=null;
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			GameControl gameControlScript=null;
			if(gameControlScript=gameControl.GetComponent<GameControl>())
			{
				gameControlScript.wayFindingPlayerSpeed=playerSpeedWF;
				gameControlScript.objectAvoidancePlayerSpeed=playerSpeedOA;
				gameControlScript.inputSensitivity=sensitivity;
				gameControlScript.b_OVRCamMenuChoice=b_Oculus;
				gameControlScript.audioVolume=volume;
				gameControlScript.b_sound=sound;
				gameControlScript.b_music=music;
				gameControlScript.b_twohands=twoHands;
			}
		}
	}

	//public void ConfigMenuValues(float wfSpeed, float oaSpeed, float sensitive, bool rift, float vol, bool mus, bool sfx )
	public void ConfigMenuValues()
	{
		GameObject gameControl=null;
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			GameControl gameControlScript=null;
			if(gameControlScript=gameControl.GetComponent<GameControl>())
			{
				playerSpeedWF = gameControlScript.wayFindingPlayerSpeed;
				playerSpeedOA = gameControlScript.objectAvoidancePlayerSpeed;
				sensitivity = gameControlScript.inputSensitivity;
				b_Oculus = gameControlScript.b_OVRCamMenuChoice;
				volume = gameControlScript.audioVolume;
				sound = gameControlScript.b_sound;
				music = gameControlScript.b_music;
				twoHands = gameControlScript.b_twohands;
			}	

		}
	}

	void OnGUI()
	//public override void OnVRGUI()
	{
		//Screen.showCursor=false;
		Screen.showCursor=true;
		if(screenHeight != Screen.height||screenWidth != Screen.width)
		{
			screenHeight = Screen.height;
			screenWidth = Screen.width;
			buttonHeight = Screen.height * 0.05f;
			buttonWidth = Screen.width * 0.2f;

			//videoWidth = Screen.width * 0.9f;
			videoWidth = Screen.width * 0.6f;
			videoHeight = Screen.height * 0.6f;

			//font size scaling may need tweaking if standalone screen ratio is variable...
			menuButtonStyle.fontSize = (int)(Screen.width/40.0f);
			menuLabelStyle.fontSize = menuButtonStyle.fontSize;
			menuLabelStyleA.fontSize = menuButtonStyle.fontSize;
			menuToggleStyle.fontSize = menuButtonStyle.fontSize;
			menuLabelStyleB.fontSize = menuButtonStyle.fontSize;
			menuLabelStyleC.fontSize = (int)(menuButtonStyle.fontSize * 0.5f);
			menuLabelStyleD.fontSize = (int)(menuButtonStyle.fontSize * 0.5f);
		}

		DrawBackground();
		DrawTitle (titleTexture);
		menuFunction();
	}

	void DrawTitle(Texture2D name)
	{
		if(name!=null)
		{
			profileTitlePosition.Set(0, 0, Screen.height, Screen.height);
			GUI.DrawTexture(profileTitlePosition, name);
		}
	}

	void DrawBackground()
	{
		//backgroundPosition.Set(	(Screen.width/2.0f)-512, (Screen.height/2.0f)-320, 1024, 640);
		//GUI.DrawTexture(backgroundPosition,background);
		//backgroundPosition.Set(	(Screen.width-Screen.height)*0.5f, 0, Screen.height, Screen.height);
		backgroundPosition.Set(	0, 0, Screen.width, Screen.height);
		GUI.DrawTexture(backgroundPosition, background);
	}

	void mainMenu()
	{
		GUI.color = buttonColour;

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.64f, buttonWidth, buttonHeight), "PROFILE", menuButtonStyle ))
		{
			titleTexture = profileTitle;
			menuFunction = profile;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "HELP", menuButtonStyle ))
		{
			titleTexture = helpHowToPlayTitle;
			menuFunction = help;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "ABOUT", menuButtonStyle ))
		{
			titleTexture = aboutTitle;
			menuFunction = about;
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "EXIT", menuButtonStyle ))
		{
			menuFunction = exitMain;
		}

		GUI.color = cursorColour;

	}

	void help()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.64f, buttonWidth*2.0f, buttonHeight),"SETUP", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = setupMenu;
		}		

		if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.7f, buttonWidth*2.0f, buttonHeight), "VIDEO TUTORIALS", menuButtonStyle))
		{
			playButtonText="PLAY";
			video = OATutorial;
			if(video)
			{
				audio.clip = video.audioClip;
			}
			
			titleTexture = helpHowToPlayTitle;
			menuFunction = videoTutorials;
		}		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		GUI.color = cursorColour;
	}

	//Help menu device setup and usage menus...
	void setupMenu()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.64f, buttonWidth*2.0f, buttonHeight),"OCULUS RIFT", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = oculusMenu;
		}		


		//choose appropriate device name for this button for each project
		//if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.7f, buttonWidth*2.0f, buttonHeight),"KINECT", menuButtonStyle))
		if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.7f, buttonWidth*2.0f, buttonHeight),"LEAP MOTION", menuButtonStyle))
		//if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.7f, buttonWidth*2.0f, buttonHeight),"HYDRA", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = deviceMenu;
		}		

		if(GUI.Button(new Rect((screenWidth - buttonWidth*2.0f) * 0.5f, screenHeight * 0.76f, buttonWidth*2.0f, buttonHeight),"USAGE INSTRUCTIONS", menuButtonStyle))
		{
			titleTexture = usageTitle;
			menuFunction = usageMenu;
		}		

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = helpHowToPlayTitle;
			menuFunction = help;
		}
		GUI.color = cursorColour;
	}


	void oculusMenu()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);

		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/1500.0f)*287.0f))*0.5f, screenHeight*0.1f, (screenWidth/1500.0f)*287.0f, (screenWidth/1500.0f)*157.0f), orimg1);	
		GUI.DrawTexture(new Rect((screenWidth-screenWidth*0.9f)*0.5f, screenHeight*0.325f, (screenWidth/1500.0f)*139.0f, (screenWidth/1500.0f)*117.0f), orimg4);
		GUI.DrawTexture(new Rect((screenWidth-(screenWidth/1500.0f)*219.0f)*0.77f, screenHeight*0.3f, (screenWidth/1500.0f)*219.0f, (screenWidth/1500.0f)*140.0f), orimg2);
		GUI.DrawTexture(new Rect((screenWidth-(screenWidth/1500.0f)*148.0f)*0.9f, screenHeight*0.3f, (screenWidth/1500.0f)*190.0f, (screenWidth/1500.0f)*148.0f), orimg3);
		GUI.DrawTexture(new Rect((screenWidth-screenWidth*0.9f)*0.5f, screenHeight*0.55f, (screenWidth/1500.0f)*236.0f, (screenWidth/1500.0f)*173.0f), orimg5);
		GUI.DrawTexture(new Rect((screenWidth-(screenWidth/1500.0f)*194.0f)*0.9f, screenHeight*0.5f, (screenWidth/1500.0f)*194.0f, (screenWidth/1500.0f)*277.0f), orimg6);
		

		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Oculus Rift Setup", 
		          menuLabelStyleA);		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.25f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Connect one end of the video cable (DVI or HDMI) to your computer and the other end to the control box. Only one video input should be connected to the control box at a time. You can use the DVI Adapter with the HDMI cable.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.3125f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Connect one end of the USB cable to your computer and the other to the control box.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.35f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Plug the power cord into an outlet and connect the other end to the control box.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.5f, screenHeight * 0.55f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Press the power button to power on the control box and the headset. A blue LED on the top of the control box indicates whether the device is on or off."
		          +"\n\nAdjust the head strap so that it fits snugly around your head."
		          +"\n\nOpen the Oculus Configuration Utility and press 'Show Demo Scene'. Put the Oculus Rift on to check that the device is working properly.", 
		          menuLabelStyleD);
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.74f, screenWidth*0.9f, screenHeight*0.2f), 
		          "For more information on setting up the Oculus Rift, please visit the official Oculus Rift website at https://support.oculus.com/ and https://developer.oculus.com/ also http://static.oculusvr.com/sdk-downloads/documents/Oculus_Rift_Development_Kit_Instruction_Manual.pdf", 
		          menuLabelStyleC);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = setupMenu;
		}
		GUI.color = cursorColour;	
	}

	//Device specific setup instructions
	void deviceMenu()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);

		/*Kinect==========================================

		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/3000.0f)*480.0f))*0.875f, (screenHeight-(screenWidth/3000.0f)*480.0f)*0.3f, (screenWidth/3000.0f)*480.0f, (screenWidth/3000.0f)*480.0f), kinect1);	
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/3000.0f)*635.0f))*0.9f, (screenHeight-(screenWidth/3000.0f)*300.0f)*0.7f, (screenWidth/3000.0f)*635.0f, (screenWidth/3000.0f)*300.0f), kinect2);	
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Microsoft Kinect Setup", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.1f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Step 1-\nMake sure that your computer is running Windows 7. It will also need to have the Kinect for Windows drivers installed, which have been provided. If it doesn't have them installed you can locate them under the Drivers folder. Double-click the exes one at a time to install them. Follow the on-screen installation instructions. ", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.35f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Step 2-\nThe Kinect device has two cords, one for power and the other a USB connector. The power cord can be detached so make sure that it's attached to the main cord coming out of the Kinect sensor. The attachment socket is coloured coded orange and it will not connect to anything else. Connect the power adapter to a power outlet. Connect the USB connector into a USB port on your computer.\n\nIf it's the first time your computer has been connected to a Kinect sensor expect to see some background driver installation processes. Wait for these to finish before running Neuromend. You should see a notification in the bottom-right corner of your screen once the processes are done.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.3f, screenHeight * 0.6f, screenWidth*0.5f, screenHeight*0.2f), 
		          "Step 3-\nPlace the Kinect sensor on a flat stable non-vibrating surface away from any edge. Make sure there aren't any cables in the way of the sensor that may block the lens or prevent it from tilting freely. Do not manually tilt the sensor. The lens on the sensor should be kept clean for optimal recognition. There should be a fair amount of room space that is free of objects such as furniture. The room should also be well lit.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.74f, screenWidth*0.9f, screenHeight*0.2f), 
		          "For more information & help please visit http://support.xbox.com/en-AU/xbox-on-other-devices/kinect-for-windows/kinect-for-windows-setup", 
		          menuLabelStyleD);
*/

		/*Leap==========================================
		  */
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/3000.0f)*606.0f))*0.69f, (screenHeight-(screenWidth/3000.0f)*393.0f)*0.15f, (screenWidth/3000.0f)*606.0f, (screenWidth/3000.0f)*393.0f), leap1);	
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/2150.0f)*493.0f))*0.99f, (screenHeight-(screenWidth/2150.0f)*279.0f)*0.15f, (screenWidth/2150.0f)*493.0f, (screenWidth/2150.0f)*279.0f), leap4);	
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/3500.0f)*1262.0f))*0.93f, (screenHeight-(screenWidth/3500.0f)*780.0f)*0.65f, (screenWidth/3500.0f)*1262.0f, (screenWidth/3500.0f)*780.0f), leap7);	

		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Leap Motion Setup", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.05f, screenHeight * 0.1f, screenWidth*0.5f, screenHeight*0.7f), 
		          "\n\n\n\nPlug the USB end of Leap Motion cable into a USB socket on your computer."
		          +"\n\n\n\n\nPlug the other end into the Leap Motion."
		          +"\n\n\n\n\nAttach the Leap Motion horizontally onto the front of the Oculus Rift. Make sure the shiny side is facing out, and the green light is facing down."
		          +"\n\nIf you are not using an Oculus Rift, attach the leap motion to a headband made of velcro or another form of headwear."
		          +"\n\n\n\n\nMake sure the shiny screen isn't smudged."
		          +"\n\n\n\n\nOpen the Leap Motion Visualizer App and the Leap Motion Conrol Panel to check that the device is working properly and to adjust any necessary settings.\n\n\n\n\n", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.74f, screenWidth*0.9f, screenHeight*0.2f), 
		          "If the Leap Motion is not working properly please visit the official Leap Motion website at https://support.leapmotion.com/home", 
		          menuLabelStyleD);


		/*Hydra==========================================

		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Razer Hydra Setup", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.5f) * 0.05f, screenHeight * 0.1f, screenWidth*0.5f, screenHeight*0.7f), 
		          "Place centerpiece on top of a smooth surface table while leaving adequate space to move around"
		          +"\n\n\n\n\nSlot long cable attached to the two hand controllers into the right port of the centerpiece"
		          +"\n\n\n\n\nSlot cable not attached to hand controllers into the left port of the centerpiece"
		          +"\n\n\n\n\nSlot the other end of the same cable into the computer's USB port"
		          +"\n\n\n\n\nEnsure connected to internet so software drivers can install", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.74f, screenWidth*0.9f, screenHeight*0.2f), 
		          "For more information & help please visit the official Razer website at http://www.razersupport.com/gaming-controllers/razer-hydra/", 
		          menuLabelStyleD);
 */
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = setupMenu;
		}
		GUI.color = cursorColour;	
	}

	//Device specific usage instructions
	void usageMenu()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);

		/*Kinect==========================================

		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/1000.0f)*506.0f))*0.9f, (screenHeight-(screenWidth/1000.0f)*379.0f)*0.5f, (screenWidth/1000.0f)*506.0f, (screenWidth/1000.0f)*379.0f), kinect3);	
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Microsoft Kinect Usage", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.4f) * 0.05f, screenHeight * 0.1f, screenWidth*0.4f, screenHeight*0.2f), 
		          "Step 1-\nMake sure the Oculus Rift is set up by following the steps on the Oculus Rift page. Make sure the Kinect is set up by following the steps on the Kinect Page.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.4f) * 0.05f, screenHeight * 0.35f, screenWidth*0.4f, screenHeight*0.2f), 
		          "Step 2-\nPosition yourself at least 1-2 meters away from the front face of the Kinect sensor. Ensure that the cables for the Oculus Rift are secured and out of your way. If your are standing directly in front of a wall be sure that its colour is not dark. It also helps to wear light coloured clothing so that the sensor doesn't struggle to recognize your entire body. However, avoid wearing clothes that blend with the wall's colour.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.4f) * 0.05f, screenHeight * 0.6f, screenWidth*0.4f, screenHeight*0.2f), 
		          "Step 3-\nThe actual movements that you will be required to make are dependent on the current level. There are instructional videos for each of the three levels, which will demonstrate the relevant movements and how to perform them correctly. You can also refer to the User Manual document to get a general understanding of how to perform the movements.", 
		          menuLabelStyleC);
		*/


		/*Leap========================================== */
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/3250.0f)*1024.0f))*0.53f, (screenHeight-(screenWidth/3250.0f)*680.0f)*0.15f, (screenWidth/3250.0f)*1024.0f, (screenWidth/3250.0f)*680.0f), leap6);	
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/1900.0f)*586.0f))*0.99f, (screenHeight-(screenWidth/1900.0f)*403.0f)*0.15f, (screenWidth/1900.0f)*586.0f, (screenWidth/1900.0f)*403.0f), leap2);	
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/5500.0f)*1298.0f))*0.05f, (screenHeight-(screenWidth/5500.0f)*915.0f)*0.7f, (screenWidth/5500.0f)*1298.0f, (screenWidth/5500.0f)*915.0f), leap5);	//placeholder image
		GUI.DrawTexture(new Rect((screenWidth-((screenWidth/2000.0f)*561.0f))*0.475f, (screenHeight-(screenWidth/2000.0f)*326.0f)*0.7f, (screenWidth/2000.0f)*561.0f, (screenWidth/2000.0f)*326.0f), leap3);	

		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Leap Motion Usage", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.35f) * 0.025f, screenHeight * 0.1f, screenWidth*0.35f, screenHeight*0.7f), 
		          "The Leap Motion interaction space is 8 cubic feet, directly outwards from the face of the device."
		          +"\n\n\nPlace the Oculus Rift on your head and place hands directly in front of the Leap Motion, holding your hands in the interaction area."
		          +"\n\n\nThe best tracking is achieved by keeping hands in the middle of the interaction area with as little as possible movements that reach the edge of the area."
		          +"\n\n\nGentle and slow movements work much better than sudden or fast movements.\n\n\n\n\n\n\n\n\n\n\n\n", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.35f) * 0.99f, screenHeight * 0.1f, screenWidth*0.35f, screenHeight*0.7f), 
		          "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nKeep arms parrallel and do not cross hands or arms over. Keep a gap between hands."
		          +"\n\n\nIf hands lose tracking, try resetting the tracker by taking hands away and replacing them in the interaction space, or closing fists and reopening."
		          +"\n\n\nTry to stay comfortable and relaxed, after some practice you will be able to judge the interaction space a lot better.", 
		          menuLabelStyleC);
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.74f, screenWidth*0.9f, screenHeight*0.2f), 
		          "For more information on how to use the Leap Motion please visit the official Leap Motion website at https://www.leapmotion.com/product/vr", 
		          menuLabelStyleD);
		


		/*Hydra==========================================
		GUI.Label(new Rect((screenWidth-screenWidth*0.9f) * 0.5f, screenHeight * 0.005f, screenWidth*0.9f, screenHeight*0.1f), 
		          "Razer Hydra Usage", 
		          menuLabelStyleA);
		GUI.Label(new Rect((screenWidth-screenWidth*0.4f) * 0.05f, screenHeight * 0.1f, screenWidth*0.4f, screenHeight*0.7f), 
		          "Before a level starts, leave both hand controllers docked on the centerpiece's indented surface."
		          +"\n\nIf hand controllers were properly docked, it should prompt the you to press the start button. "
		          +"\n\nIf hand controllers were not docked, ensure they are situated comfortably in your hands before doing anything else"
		          +"\n\nFollow the sequence of steps as prompted by a text box that appears in the upper center of the screen"
		          +"\n\nPress the left controller's trigger button first "
		          +"\n\nPress the right controller's trigger button second"
		          +"\n\nPress start button "
		          +"\n\nHave Fun!", 
		          menuLabelStyleC);

		*/


		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = setupTitle;
			menuFunction = setupMenu;
		}
		GUI.color = cursorColour;	
	}


	void videoTutorials()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		videoPosition.Set((screenWidth - videoWidth) * 0.5f, (screenHeight - videoHeight) * 0.48f, videoWidth, videoHeight);
		GUI.DrawTexture(videoPosition, video);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.075f, buttonWidth, buttonHeight*2.0f), "", menuButtonStyle))
		{
			b_OATutVid = true;
			b_WFTutVid = false;
			b_OITutVid = false;
			b_VideoPaused = false;
			b_VideoStarted = false;
			b_VideoFinished = false;

			if(video)
			{
				video.Stop();
				audio.Stop();
			}
			video=OATutorial;
			if(video)
			{
				audio.clip = video.audioClip;
				video.Stop();
				audio.Stop();
				playButtonText="PLAY";
			}
		}	
		GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.075f, buttonWidth, buttonHeight*2.0f), b_OATutVid, "OBSTACLE AVOIDANCE", menuToggleStyle);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.075f, buttonWidth, buttonHeight*2.0f), "", menuButtonStyle))
		{
			b_OATutVid = false;
			b_WFTutVid = true;
			b_OITutVid = false;
			b_VideoPaused = false;
			b_VideoStarted = false;
			b_VideoFinished = false;

			if(video)
			{
				video.Stop();
				audio.Stop();
			}
			video=WFTutorial;
			if(video)
			{
				audio.clip = video.audioClip;
				video.Stop();
				audio.Stop();
				playButtonText="PLAY";
			}
		}		
		GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.075f, buttonWidth, buttonHeight*2.0f), b_WFTutVid, "WAY FINDING", menuToggleStyle);

		if(GUI.Button(new Rect((screenWidth - buttonWidth*1.2f) * 0.9f, screenHeight * 0.075f, buttonWidth*1.2f, buttonHeight*2.0f), "", menuButtonStyle))
		{
			b_OATutVid = false;
			b_WFTutVid = false;
			b_OITutVid = true;
			b_VideoPaused = false;
			b_VideoStarted = false;
			b_VideoFinished = false;

			if(video)
			{
				video.Stop();
				audio.Stop();
			}
			video=OITutorial;
			if(video)
			{
				audio.clip = video.audioClip;
				video.Stop();
				audio.Stop();
				playButtonText="PLAY";
			}
		}		
		GUI.Toggle(new Rect((screenWidth - buttonWidth*1.2f) * 0.9f, screenHeight * 0.075f, buttonWidth*1.2f, buttonHeight*2.0f), b_OITutVid, "OBJECT MANIPULATION", menuToggleStyle);


		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.8f, buttonWidth, buttonHeight), playButtonText, menuButtonStyle))
		{
			if(video)
			{
				if(!video.isPlaying)
				{
					b_VideoStarted = true;
					b_VideoFinished = false;
					video.Play();
					audio.Play();
					playButtonText = "PAUSE";
					b_VideoPaused = false;
				}
				else
				{
					video.Pause();
					audio.Pause();
					playButtonText = "PLAY";
					b_VideoPaused = true;
				}
			}
		}

		if (video) 
		{
			if(b_VideoStarted)
			{
				if (video.isPlaying) 
				{
				}
				else if(!b_VideoFinished&&!b_VideoPaused)
				{
					b_VideoFinished=true;
					video.Stop();
					audio.Stop();
					playButtonText="PLAY";
					b_VideoStarted=false;
				}
			}
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			if(video)
			{
				video.Stop();
				audio.Stop();
				playButtonText="PLAY";
			}
			titleTexture = helpHowToPlayTitle;
			menuFunction = help;
		}
		GUI.color = cursorColour;
	}

	void exitMain()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;


		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.4f, buttonWidth, buttonHeight), "Are you sure you want to exit?", menuLabelStyle);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.25f, screenHeight * 0.5f, buttonWidth, buttonHeight), "YES", menuButtonStyle))
		{
			Application.Quit ();
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.75f, screenHeight * 0.5f, buttonWidth, buttonHeight), "NO", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		GUI.color = cursorColour;
	}
	
	void profile()
	{
		profileMenu.Draw();

		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "SETTINGS", menuButtonStyle))
		{
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "START", menuButtonStyle))
		{
			titleTexture = exerciseTitle;
			menuFunction = levelSelect;
		}
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
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

		//need to display current devices?
		//

		//oculus rift toggle...
		b_Oculus = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.2f, buttonWidth*1.5f, buttonHeight), b_Oculus, "Oculus Rift", menuToggleStyle);
		//if the rift is not detected you cannot toggle this to 'on'
		if(b_Oculus&&!TempestUtil.OVRConnectionCheck())
		{
			b_Oculus=false;
			//should display error message instructing to connect OVR HMD
		}

		//b_playTutorials = GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.09f, screenHeight * 0.2f, buttonWidth, buttonHeight), b_playTutorials, "");
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Play Tutorials", menuLabelStyleA);
		b_playTutorials = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.3f, buttonWidth*1.5f, buttonHeight), b_playTutorials, "Play Tutorials", menuToggleStyle);
		b_objectAvoidance = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.4f, buttonWidth*1.5f, buttonHeight), b_objectAvoidance, "Obstacle Avoidance", menuToggleStyle);
		b_wayFinding = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.47f, buttonWidth*1.5f, buttonHeight), b_wayFinding, "Way Finding", menuToggleStyle);
		b_objectInteraction = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.54f, buttonWidth*1.5f, buttonHeight), b_objectInteraction, "Object Manipulation", menuToggleStyle);


		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.3f, buttonWidth, buttonHeight), "Object Interaction", menuLabelStyleA);
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.37f, buttonWidth, buttonHeight), "Object Avoidance", menuLabelStyleA);
		//GUI.Label(new Rect((screenWidth - buttonWidth) * 0.1f, screenHeight * 0.44f, buttonWidth, buttonHeight), "Way Finding", menuLabelStyleA);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "START", menuButtonStyle))
		{
			//variables.SetTwoHands(twoHands);
			//variables.SetSensitivity(sensitivity);


			//set first level
			if(b_objectAvoidance){firstLevelIndex = 3;}
			else if(b_wayFinding){firstLevelIndex = 5;}
			else if(b_objectInteraction){firstLevelIndex = 7;}
			//adjust first level index if tutorials are to be played
			if(b_playTutorials){firstLevelIndex--;}


			//count levels and...
			//populate level index array for game control...
			numberOfLevels=0;
			if(b_objectAvoidance)
			{
				if(b_playTutorials)
				{
					levelIndexes[numberOfLevels]=2;
					numberOfLevels++;
				}
				levelIndexes[numberOfLevels]=3;
				numberOfLevels++;

			}
			if(b_wayFinding)
			{
				if(b_playTutorials)
				{
					levelIndexes[numberOfLevels]=4;
					numberOfLevels++;
					
				}
				levelIndexes[numberOfLevels]=5;
				numberOfLevels++;
			}
			if(b_objectInteraction)
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
				ConfigGameControl();
				Application.LoadLevel (firstLevelIndex);
			}
			else
			{
				//display "please select at least one exercise..."
			}
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
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

		if(GUI.Button(new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.7f, buttonWidth, buttonHeight), "CONFIG", menuButtonStyle))
		{
			titleTexture = settingsConfigTitle;
			menuFunction = controls;
		}
		if(GUI.Button(new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.76f, buttonWidth, buttonHeight), "AUDIO", menuButtonStyle))
		{
			titleTexture = audioTitle;
			menuFunction = audioMenu;
		}
		if(GUI.Button(new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = profileTitle;
			menuFunction = profile;
		}
		GUI.color = cursorColour;
	}

	void audioMenu()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		//GUI.Label(new Rect(screenWidth * 0.45f, screenHeight * 0.3f, screenWidth * 0.1f, screenHeight * 0.1f), "*display audio options here*", menuButtonStyle);
		
		sound = GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.4f, buttonWidth, buttonHeight), sound, "SFX", menuToggleStyle);
		music = GUI.Toggle(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.47f, buttonWidth, buttonHeight), music, "MUSIC", menuToggleStyle);
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.54f, buttonWidth, buttonHeight), "Volume", menuLabelStyleB);
		volume = GUI.HorizontalSlider (new Rect ((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.61f, buttonWidth, buttonHeight), volume,0.0f,1.0f);
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
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

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.1f), "NEUROMEND", menuLabelStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "\n\nA Murdoch University School of IT and Engineering project." 
		          +	"\n\nBrought to you by TEMPEST\n\nAry Bizar\nAnopan Kandiah\nHannah Klinac\nAlex Mlodawski\nBryan Yu"
		          + "\n\nMusic by: Ayden-James Nolan" + "\nSounds by: Elly Thompson"
		          + "\n\nProject client and supervisor:\nShri Rai and Dr Fairuz Shiratuddin", menuLabelStyleD);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.82f, buttonWidth, buttonHeight), "MORE", menuButtonStyle ))
		{
			titleTexture = aboutTitle;
			menuFunction = aboutMore;
		}
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = null;
			menuFunction = mainMenu;
		}
		GUI.color = cursorColour;
	}	

	void aboutMore()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);
		GUI.Label(new Rect(0, screenHeight * 0.1f, screenWidth, screenHeight * 0.7f), "", menuButtonStyle);

		iconPosition.Set( (Screen.width-Screen.height/4.0f)/2.0f, Screen.height*0.1f, Screen.height/4.0f, Screen.height/4.0f);
		GUI.DrawTexture(iconPosition, neuromendIcon);

		GUI.Label(new Rect((screenWidth-screenWidth*0.5f)*0.5f, screenHeight * 0.1f, screenWidth*0.5f, screenHeight * 0.7f), "\n\n\n\nNEUROMEND\n\n"
		          +"-is a virtual simulation project focused on researching the possibility of using virtual environments in conjunction with various natural user interfaces including the Oculus Rift virtual reality head mounted display,  Microsoft Kinect, Leap Motion Controller, and Razer Hydra for the rehabilitation of stroke patients." , menuLabelStyleD);

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			titleTexture = aboutTitle;
			menuFunction = about;
		}
		GUI.color = cursorColour;
	}

	//config... device sensitivity, player speeds, oculus on/off
	void controls()
	{
		GUI.color = buttonColour;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		float min = 0.0f;
		float max = 10.0f;
		float minOASpeed = 1.0f;
		float maxOASpeed = 5.0f;
		float minWFSpeed = 1.0f;
		float maxWFSpeed = 5.0f;
		
		GUI.color = buttonColour;

		b_Oculus = GUI.Toggle(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.25f, buttonWidth*1.5f, buttonHeight), b_Oculus, "Oculus Rift", menuToggleStyle);
		//if the rift is not detected you cannot toggle this to 'on'
		if(b_Oculus&&!TempestUtil.OVRConnectionCheck())
		{
			b_Oculus=false;
			//should display error message instructing to connect OVR HMD
		}

		GUI.Label(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.3f, buttonWidth*1.5f, buttonHeight*2.0f),"Device Sensitivity",menuLabelStyle);
		sensitivity = GUI.HorizontalSlider(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.38f, buttonWidth*1.5f, buttonHeight),sensitivity, min, max);
		SliderLock(ref sensitivity,min,max);
	
		GUI.Label(new Rect((screenWidth- screenWidth * 0.4f)* 0.5f, screenHeight * 0.54f, screenWidth * 0.4f, screenHeight * 0.1f), "Player Movement Speed",menuButtonStyle);
		GUI.Label(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.62f, buttonWidth*1.5f, buttonHeight*2.0f),"Obstacle Avoidance",menuLabelStyle);
		playerSpeedOA = GUI.HorizontalSlider(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.7f, buttonWidth*1.5f, buttonHeight),playerSpeedOA, minOASpeed, maxOASpeed);
		SliderLock(ref playerSpeedOA,minOASpeed,maxOASpeed);

		GUI.Label(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.72f, buttonWidth*1.5f, buttonHeight*2.0f),"Way Finding",menuLabelStyle);
		playerSpeedWF = GUI.HorizontalSlider(new Rect((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.8f, buttonWidth*1.5f, buttonHeight),playerSpeedWF, minWFSpeed, maxWFSpeed);
		SliderLock(ref playerSpeedWF,minWFSpeed,maxWFSpeed);

		//twoHands = GUI.Toggle (new Rect ((screenWidth - buttonWidth*1.5f) * 0.5f, screenHeight * 0.45f, buttonWidth*1.5f, buttonHeight), twoHands, "Two hands", menuToggleStyle);//leap motion specific...

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.9f, buttonWidth, buttonHeight), "BACK", menuButtonStyle))
		{
			ConfigGameControl();
			titleTexture = settingsTitle;
			menuFunction = settings;
		}
		GUI.color = cursorColour;
	}

	//locks slider at intervals 0%, 25%, 50%, 75%, 100%
	void SliderLock(ref float sliderVal, float minVal, float maxVal)
	{
		if(sliderVal<=maxVal && sliderVal>=(maxVal-minVal)*0.875f+minVal){sliderVal=maxVal;}
		else if(sliderVal<(maxVal-minVal)*0.875f+minVal && sliderVal>=(maxVal-minVal)*0.625f+minVal){sliderVal=(maxVal-minVal)*0.75f+minVal;}
		else if(sliderVal<(maxVal-minVal)*0.625f+minVal && sliderVal>=(maxVal-minVal)*0.375f+minVal){sliderVal=(maxVal-minVal)*0.5f+minVal;}
		else if(sliderVal<(maxVal-minVal)*0.375f+minVal && sliderVal>=(maxVal-minVal)*0.125f+minVal){sliderVal=(maxVal-minVal)*0.25f+minVal;}
		else{sliderVal=minVal;}
	}
	/*
	// Update is called once per frame
	void Update() 
	{
		//Screen.showCursor=false;
	}
*/

}
