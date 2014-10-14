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
	private PlaythroughType playthroughType;//type of playthrough

	public float objectInteractionScore = 0.0f;
	private float objectInteractionCheckpoint = 0.0f;
	
	public float objectAvoidanceScore = 0.0f;
	private float objectAvoidanceCheckpoint = 0.0f;
	
	public float wayFindingScore = 0.0f;
	private float wayFindingCheckpoint = 0.0f;


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
	public bool b_OVRCam=false;
	
	public bool b_menuActive=false;


	//PLAYER SETTINGS==============================================
	public float objectAvoidancePlayerSpeed=2.0f;
	public float wayFindingPlayerSpeed=5.0f;
	public float inputSensitivity=100;
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
	
		/*//debugging...*/
		if(Input.GetMouseButtonDown(2))
		{
			OVRCamera();
		}

		if(!b_menuActive)
		{
			Screen.lockCursor=true;
		}
		else
		{
			Screen.lockCursor=false;
			Screen.showCursor=false;
		}
		
		if(Input.GetKey(KeyCode.Escape))
		{
			PauseGame(!b_paused);
		}
	}
	
	//toggles OVRCam mode... still needs testing etc.
	public void OVRCamera()
	{
		b_OVRCam = !b_OVRCam;
		
		GameObject tempPlayer=null;
		if (tempPlayer = GameObject.FindWithTag ("Player")) 
		{
			//Debug.Log ("PLAYER");
			//float tempOVRCamIndex = 0;
			GameObject tempOVRCam = null;
			GameObject tempMainCam = null;
			//int i=0;
			foreach(Transform child in tempPlayer.transform)
			{
				//Debug.Log (i);
				if(child.tag=="OVRCam")
				{
					//Debug.Log ("OVRCAM");
					tempOVRCam=child.gameObject;
					//tempOVRCamIndex=i;
				}
				if(child.tag=="MainCamera")
				{
					//Debug.Log ("MAINCAM");
					tempMainCam=child.gameObject;
				}
				//i++;
			}
			if(tempOVRCam!=null&&tempMainCam!=null)
			{
				Debug.Log (b_OVRCam);
				
				Camera tempCameraComponent = null;
				if(tempCameraComponent=tempMainCam.GetComponent<Camera>())
				{
					if(b_OVRCam)
					{
						tempCameraComponent.gameObject.SetActive(false);
						tempOVRCam.gameObject.SetActive(true);
					}
					else
					{
						tempCameraComponent.gameObject.SetActive(true);
						tempOVRCam.gameObject.SetActive(false);
					}
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
    
    //returns object interaction score float value
    public float GetOIScore()
    {
		return objectInteractionScore;
    }

    public void SetOIScore(float score)
    {
		objectInteractionScore=score;
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
    }
    
    public void SetWFCheckpoint(float percentageComplete)
    {
		wayFindingCheckpoint=percentageComplete;
    }
    
    public float GetWFCheckpoint()
    {
		return wayFindingCheckpoint;
    }
    
    /*
    //returns userID
    uint getUserID()
    {
		return userID;
    }
    */
    public void LoadNextLevel(string levelName)
    {
		Application.LoadLevel(levelName);
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

		switch(levelType)
		{
			case LevelControl.LevelType.ObjectAvoidance: writer = XmlWriter.Create(@OAPath, settings); break;
			case LevelControl.LevelType.ObjectInteraction: writer = XmlWriter.Create(@OIPath, settings); break;
			case LevelControl.LevelType.WayFinding: writer = XmlWriter.Create(@WFPath, settings); break;
		}

		writer.WriteStartDocument ();
		writer.WriteStartElement("Level Summary");
		writer.WriteElementString("Username", userID.ToString());
		writer.WriteElementString("Level", levelType.ToString());
		writer.WriteElementString("Controller", controllerType.ToString());
		writer.WriteElementString("Score", objectInteractionScore.ToString());
		writer.WriteElementString("Timestamp", System.DateTime.Now.ToLongDateString());
		writer.WriteEndElement();
		writer.WriteEndDocument();
	
		writer.Flush ();
		writer.Close ();
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
		
		/*
		if(!b_paused)
		{
			Time.timeScale = 0.0f;
			b_paused = true;
		}
		else
		{
			Time.timeScale = 1.0f;
			b_paused = false;
		}
		*/
 	}
 	
 	public bool Paused()
 	{
		return b_paused;
 	}
	
}