﻿// NOTE: File I/O has been added in this class. 17/09/2014 Anopan
//       All data will be saved in the Root Directory.
// ...File I/O has been updated to Bryan's xml IO and formatting
using UnityEngine;
using System.Collections;
using System.Xml;

public class GameControl : MonoBehaviour 
{
	public enum ControllerType
	{
		MouseKeyboard,
		OculusLeap,
		OculusHydra,
		OculusKinect
	};
	
	public enum PlaythroughType
	{	
		NewGame,//full playthrough from beginning
		LoadGame,//continue previously saved full playthrough
		SingleLevel,//select single level to play
		LoadSingle//continue previously saved single level
	};
	
	private ControllerType controllerType;
	private PlaythroughType playthroughType;//type of playthrough... not needed

	public float objectInteractionScore = 0.0f;
	private float objectInteractionCheckpoint = 0.0f;//not used?
	private bool b_objectInteraction = false;
	
	public float objectAvoidanceScore = 0.0f;
	private float objectAvoidanceCheckpoint = 0.0f;//not used?
	private bool b_objectAvoidance = false;

	public float wayFindingScore = 0.0f;
	private float wayFindingCheckpoint = 0.0f;//not used?
	private bool b_wayFinding = false;

	public int[] levelIndexes=new int[7];//indexes of levels in current playthrough
	public int numberOfLevels=0;//number of levels in current playthrough
	public int currentLevelIndex = 0;//current level index for current playthrough

	//for use with networked database data...
	private int userID = 0;
	/*
	float objectInteractionHighScore = 0.0f;
	float objectAvoidanceHighScore = 0.0f;
	float wayFindingHighScore = 0.0f;
    */

		//Paths to files for saving of scores.
	private string OIPath = "";
	private string OAPath = "";
	private string WFPath = "";

		//Used for writing to files.
    private System.IO.StreamWriter fileWriter;

	
	public bool b_paused=false;
	public bool b_OVRCam=false;//used to toggle OVRCam... toggling not stable

	public bool b_OVRCamMode=true;
	
	public bool b_menuActive=false;


	//PLAYER SETTINGS==============================================
	public float objectAvoidancePlayerSpeed=2.0f;
	public float wayFindingPlayerSpeed=5.0f;
	public float inputSensitivity=5.0f;
	//END PLAYER SETTINGS==============================================


	public void MenuActive(bool choice=true)
	{
		b_menuActive=choice;
	}

	public bool OVRCam()
	{
		return b_OVRCam;
	}
	
    void Awake() 
    {
        DontDestroyOnLoad(transform.gameObject);   
    }
    
    void Start()
    {
		/*
		OIPath = userID+"_OIScore.txt";
		OAPath = userID+"_OAScore.txt";
		WFPath = userID+"_WFScore.txt";
		*/
		OIPath = userID+"_OIScore.xml";
		OAPath = userID+"_OAScore.xml";
		WFPath = userID+"_WFScore.xml";
	}
        
	void Update()
	{
	
		/*//debugging...	
		if(Input.GetMouseButtonDown(2))
		{
			OVRCamera(!b_OVRCam);
		}
		*/
		if(!b_menuActive)
		{
			Screen.lockCursor=true;
			Screen.showCursor=false;
		}
		else
		{
			Screen.lockCursor=false;
			Screen.showCursor=false;
		}

		//pause... still need to stop all actions on player and world...
		if(Input.GetKeyDown("p"))
		{
			PauseGame(!b_paused);
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			//if not main menu skip to next level...
			GameObject tempLevel=null;
			if(tempLevel=GameObject.FindWithTag("Level"))
			{
				LoadNextLevel();
			}
		}
	}

	//set oculus camera mode if selectd in menu
	//this function will act as initialiser since this object should be persistent
	void OnLevelWasLoaded(int level)
	{
		//if menu, don't use Oculus rift...
		if(level==1)
		{
			OVRCamera(false);
		}
		else
		{
			OVRCamera(b_OVRCamMode);
		}

		if(controllerType==ControllerType.MouseKeyboard)
		{
			GameObject tempLevel=null;
			if(tempLevel=GameObject.FindWithTag("Level"))
			{
				LevelControl tempLevelControl =null;
				if(tempLevelControl=tempLevel.GetComponent<LevelControl>())
				{

					if(tempLevelControl.levelType==LevelControl.LevelType.Video||tempLevelControl.levelType==LevelControl.LevelType.Scores)
					{
						tempLevelControl.ShowCrosshairs(false);
					}
					else
					{
						tempLevelControl.ShowCrosshairs();
					}

					//tempLevelControl.ShowCrosshairs(!(tempLevelControl.levelType==LevelControl.LevelType.Video));

				}
			}
			GameObject tempPlayer = null;
			if(tempPlayer=GameObject.FindWithTag("Player"))
			{
				MouseLook tempMouseLook = null;
				if(tempMouseLook=tempPlayer.GetComponent<MouseLook>())
				{
					tempMouseLook.sensitivityX=inputSensitivity+0.5f;
					tempMouseLook.sensitivityY=inputSensitivity+0.5f;
				}

				GameObject tempMainCam = null;
				foreach(Transform child in tempPlayer.transform)
				{
					if(child.tag=="MainCamera")
					{
						tempMainCam=child.gameObject;
					}
				}

				if(tempMainCam)
				{
					MouseLook tempMouseLookB = null;
					if(tempMouseLookB=tempMainCam.GetComponent<MouseLook>())
					{
						tempMouseLookB.sensitivityX=inputSensitivity+0.5f;
						tempMouseLookB.sensitivityY=inputSensitivity+0.5f;
					}
				}
			}
		}


	}

	//switches between OVRcam and normal... does not work well or at all if toggled more than once?? needs more testing...
	public void OVRCamera(bool OVRcam=true)
	{
		b_OVRCam = OVRcam;

		GameObject tempPlayer=null;
		if (tempPlayer = GameObject.FindWithTag ("Player")) 
		{
			GameObject tempOVRCam = null;
			GameObject tempMainCam = null;
			foreach(Transform child in tempPlayer.transform)
			{
				if(child.tag=="OVRCam")
				{
					tempOVRCam=child.gameObject;
				}
				if(child.tag=="MainCamera")
				{
					tempMainCam=child.gameObject;
				}
			}
			if(tempOVRCam!=null&&tempMainCam!=null)
			{
				if(b_OVRCam)
				{
					tempMainCam.camera.enabled=false;
					tempOVRCam.gameObject.SetActive(true);
				}
				else
				{
					tempMainCam.camera.enabled=true;
					tempOVRCam.gameObject.SetActive(false);
				}
			}
		}
	}
	

    public void SetUserID(int number)
    {
		userID=number;
		OIPath = userID+"_OIScore.xml";
		OAPath = userID+"_OAScore.xml";
		WFPath = userID+"_WFScore.xml";
    }
    
	//used to reset once new game selected etc...
	public void ResetCurrentScores()
	{		
		objectInteractionScore = 0.0f;
		objectInteractionCheckpoint = 0.0f;//not used?
		b_objectInteraction = false;
		
		objectAvoidanceScore = 0.0f;
		objectAvoidanceCheckpoint = 0.0f;//not used?
		b_objectAvoidance = false;
		
		wayFindingScore = 0.0f;
		wayFindingCheckpoint = 0.0f;//not used?
		b_wayFinding = false;
	}

	public void ResetCurrentPlaythrough()
	{
		b_objectInteraction = false;
		b_objectAvoidance = false;
		b_wayFinding = false;
	}

    //returns object interaction score float value
    public float GetOIScore()
    {
		return objectInteractionScore;
    }

    public void SetOIScore(float score)
    {
		objectInteractionScore=score;
		b_objectInteraction = true;
    }
    
    public void SetOICheckpoint(float percentageComplete)
    {
		objectInteractionCheckpoint=percentageComplete;
    }
    
    public float GetOICheckpoint()
    {
		return objectInteractionCheckpoint;
    }
    
    //returns object avoidance score float value
    public float GetOAScore()
    {
		return objectAvoidanceScore;
    }
    
    public void SetOAScore(float score)
    {
		objectAvoidanceScore=score;
		b_objectAvoidance = true;
    }
    
    public void SetOACheckpoint(float percentageComplete)
    {
		objectAvoidanceCheckpoint=percentageComplete;
    }
    
    public float GetOACheckpoint()
    {
		return objectAvoidanceCheckpoint;
    }
    
    //returns way finding score float value
    public float GetWFScore()
    {
		return wayFindingScore;
    }
    
    public void SetWFScore(float score)
    {
		wayFindingScore=score;
		b_wayFinding = true;
    }
    
    public void SetWFCheckpoint(float percentageComplete)
    {
		wayFindingCheckpoint=percentageComplete;
    }
    
    public float GetWFCheckpoint()
    {
		return wayFindingCheckpoint;
    }
    
	//check if score was changed.
	public bool IsObjectInteractionScore()
	{
		return b_objectInteraction;
	}
	
	public bool IsObjectAvoidanceScore()
	{
		return b_objectAvoidance;
	}
	
	public bool IsWayFindingScore()
	{
		return b_wayFinding;
	}
	

    /*
    //returns userID
    uint getUserID()
    {
		return userID;
    }

    public void LoadNextLevel(string levelName)
    {
		Application.LoadLevel(levelName);
    }
*/
	public void LoadNextLevel()
	{
		//unpause if paused
		if(b_paused)
		{
			PauseGame(false);
		}
		currentLevelIndex++;
		//if no more levels go to main menu otherwise load next level in queue
		if(currentLevelIndex>=numberOfLevels)
		{
			currentLevelIndex=0;
			ResetCurrentPlaythrough();
			MenuActive();
			Application.LoadLevel ("Main Menu");//load main menu...
		}
		else
		{
			MenuActive(false);
			Application.LoadLevel (levelIndexes[currentLevelIndex]);//load the next level...
		}
	}
    
    public void SetControllerType(ControllerType type)
    {
		controllerType=type;
    }
    
    public ControllerType GetControllerType()
    {
		return controllerType;
    }
    
    public void SetPlaythroughType(PlaythroughType type)
    {
		playthroughType=type;
    }

    public PlaythroughType GetPlaythroughType()
    {
		return playthroughType;
    }

	public void SaveScore(LevelControl.LevelType levelType)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		XmlWriter writer = null;
		string filename = "";

		switch(levelType)
		{
			case LevelControl.LevelType.ObjectAvoidance: 
			{
				filename = @OAPath;
				writer = XmlWriter.Create(@OAPath, settings);
			}
			break;

			case LevelControl.LevelType.ObjectInteraction:
			{
				filename = @OIPath;
				writer = XmlWriter.Create(@OIPath, settings);
			}
			break;

			case LevelControl.LevelType.WayFinding:
			{
				filename = @WFPath;
				writer = XmlWriter.Create(@WFPath, settings);
			}
			break;
		}

		GameObject tempDBObj = null;
		Tempest.Database.TempestDB tempDatabase =null;
		
		if (!(tempDBObj = GameObject.Find ("Database")) ||
		    !(tempDatabase = tempDBObj.GetComponent<Tempest.Database.TempestDB> ()) ||
		    !tempDatabase.Profile.HasValue)
		{

			writer.Flush();
			writer.Close ();
			return;
		}
		

		writer.WriteStartDocument ();
		writer.WriteStartElement("Level_Summary");

		writer.WriteElementString("Username", tempDatabase.Profile.Value.m_username);
	
		switch(controllerType)
		{
			case ControllerType.MouseKeyboard:
			{
				writer.WriteElementString("Controller", "Mouse and Keyboard");
			}
			break;

			case ControllerType.OculusHydra:
			{
				writer.WriteElementString("Controller", "Razer Hydra");
			}
			break;
			
			case ControllerType.OculusKinect:
			{
				writer.WriteElementString("Controller", "Kinect");
			}
			break;
			
			case ControllerType.OculusLeap:
			{
				writer.WriteElementString("Controller", "Leap Motion");
			}
			break;
		}

		switch(levelType)
		{
			case LevelControl.LevelType.None: break;
			
			case LevelControl.LevelType.ObjectInteraction:
			{
				writer.WriteElementString("Level", "Object Interaction");
				writer.WriteElementString("Score", objectInteractionScore.ToString());
				writer.WriteElementString("Speed", (0.0f).ToString()); //speed not applicable
			}
			break;
			
			case LevelControl.LevelType.ObjectAvoidance:
			{
				writer.WriteElementString("Level", "Object Avoidance");
				writer.WriteElementString("Score", objectAvoidanceScore.ToString());
				writer.WriteElementString("Speed", objectAvoidancePlayerSpeed.ToString());
			}
			break;

			case LevelControl.LevelType.WayFinding:
			{
				writer.WriteElementString("Level", "Way Finding");
				writer.WriteElementString("Score", wayFindingScore.ToString());
				writer.WriteElementString("Speed", wayFindingPlayerSpeed.ToString());
			}
			break;
		}
		
		writer.WriteElementString("Sensitivity", inputSensitivity.ToString());
		writer.WriteElementString ("Timestamp", System.DateTime.Now.ToString ("d/M/yyyy HH:mm:ss"));

		writer.WriteEndElement();
		writer.WriteEndDocument();
	
		writer.Flush ();
		writer.Close ();

		tempDatabase.ReportDatabase.AddReport(filename);
	}

	private void ReadyFile(string pathName)
	{
        if (System.IO.File.Exists(pathName))
        {
            fileWriter = System.IO.File.AppendText(pathName);
        }
        else
        {
            fileWriter = System.IO.File.AppendText(pathName);
            fileWriter.WriteLine("UserID\tScore\tDate");
        }
	}

	//still need to implement this properly...
	//need to pause most player interaction etc...
	public void PauseGame(bool pause=true)
	{
		b_paused=pause;
		if(b_paused)
		{
			Time.timeScale = 0.0f;
		}
		else
		{
			Time.timeScale = 1.0f;
		}
 	}
 	
 	public bool Paused()
 	{
		return b_paused;
 	}
	
}
