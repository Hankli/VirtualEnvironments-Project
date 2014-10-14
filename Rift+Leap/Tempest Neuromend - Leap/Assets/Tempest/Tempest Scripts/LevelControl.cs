﻿// NOTE: File I/O has been added in this class. 17/09/2014 Anopan
//       All data will be saved in the Root Directory.

/*
This contains GUI information and functionality including timer, scores, objectives, hints, crosshairs etc..
For VR GUI see TempestVRGUI.cs
*/
using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour 
{

	public enum LevelType
	{	
		None,
		Video,
		ObjectInteraction,
		ObjectAvoidance,
		WayFinding
	};
	
	[Tooltip("Type of level...")]
	public LevelType levelType;
	[Tooltip("Name of the next scene to load after this level is complete (if any)")]
	public string nextLevelName="";
	
	private int totalTimePassed=0;
	private int timePassedSec=0;
	private int timePassedMin=0;
	private int timePassedHr=0;
	private string timePassedString="";
	
	private int levelObjectives=0;
	private float levelCompletion=0.0f;//0-1 percentage
	private int objectivesCompleted=0;
	
	private bool b_isLevelComplete = false;
	private bool b_showTimer = true;
	private bool b_showObjective = true;
	
	//private GameObject gameControl = null;
	private GameControl gameControlScript = null;
	
	[Tooltip("The duration a hint will be displayed once triggered")]
	public float hintDuration=5.0f;
	
	private float screenWidth=0.0f;
	private float screenHeight=0.0f;
	
	//current objective text...
	Rect objectivePosition;
	private string currentObjective="";
	//private string previousObjective="";
	public GUIStyle objective;
	Rect objectiveShadowPosition;
	public GUIStyle objectiveShadow;
	float objectiveHeight;
	
	//hints...
	private string hintOverride="";//fallback for objective text
	bool b_showHint=false;
	private string hintMessage="";
	private float hintTimerA=0.0f;
	private float hintTimerB=0.0f;
	
	//'up' timer
	Rect timerPosition;
	string timerText;
	public GUIStyle timer;
	Rect timerShadowPosition;
	public GUIStyle timerShadow;
	float timerHeight;

	//next level load timer...
	Rect countdownPosition;
	string countdownText="";
	public GUIStyle countdown;
	Rect countdownShadowPosition;
	public GUIStyle countdownShadow;
	float countdownHeight;

	private float loadCountdown = 5.0f;//for next level load
	//bool b_endingLevel=false;
	
	//Crosshairs
	private bool b_showCrosshairs=true;
	private Rect crosshairsPosition;
	private float crosshairsDimensions=64;
	private Texture2D crosshairsTexture;
	
	private bool b_saved=false;

	void Awake()
	{
		GameObject gameControl = null;
		if(gameControl = GameObject.FindWithTag("Game"))
		{
			gameControlScript = gameControl.GetComponent<GameControl>();
		}
		
		CountObjectives();
	}
	
	void Start()
	{
		timer.normal.textColor=Color.white;
		timer.fontSize=50;
		timer.alignment=TextAnchor.MiddleCenter;
		timer.fontStyle=FontStyle.Bold;
		
		timerShadow.normal.textColor=Color.black;
		timerShadow.fontSize=50;
		timerShadow.alignment=TextAnchor.MiddleCenter;
		timerShadow.fontStyle=FontStyle.Bold;
		
		objective.normal.textColor=Color.white;
		objective.fontSize=20;
		objective.alignment=TextAnchor.MiddleCenter;
		objective.fontStyle=FontStyle.Bold;
		
		objectiveShadow.normal.textColor=Color.black;
		objectiveShadow.fontSize=20;
		objectiveShadow.alignment=TextAnchor.MiddleCenter;
		objectiveShadow.fontStyle=FontStyle.Bold;
		
		
		countdown.normal.textColor=Color.red;
		countdown.fontSize=30;
		countdown.alignment=TextAnchor.MiddleCenter;
		countdown.fontStyle=FontStyle.Bold;

		countdownShadow.normal.textColor=Color.black;
		countdownShadow.fontSize=30;
		countdownShadow.alignment=TextAnchor.MiddleCenter;
		countdownShadow.fontStyle=FontStyle.Bold;
		
		crosshairsTexture=Resources.Load<Texture2D>("Crosshairs01");
	}
	
	void Update() 
	{
	
		if(levelType==LevelType.Video)
		{
			//run video...
			//load next level when done...
			
			
			
			AdjustGUI();
			timerText="";
			timerText=nextLevelName+" Video Tutorial Screen";
			EndLevel();
			//b_endingLevel=true;
		}
		else
		{
			if(b_showTimer||b_showObjective||b_showHint)
			{
				//get screen dimensions if changed, adjust text positions
				AdjustGUI();
			}

			//display the timer if needed
			if(b_showTimer)
			{
				//reset time string
				timePassedString="";			
				if(timePassedHr<=9)
					timePassedString="0";				
				timePassedString+=timePassedHr+":";			
				if(timePassedMin<=9)
					timePassedString+="0";				
				timePassedString+=timePassedMin+":";			
				if(timePassedSec<=9)
					timePassedString+="0";				
				timePassedString+=timePassedSec;
				timerText=timePassedString;
			}
			else
			{
				timerText="";
			}
			
			//display current objective...
			if(b_showObjective)
			{
			}
			else
			{
				currentObjective="";
			}		
			//show hint...
			if(b_showHint)
			{
				hintTimerB=Time.time;
				
				if((hintTimerB-hintTimerA)>hintDuration)
				{
					b_showHint=false;
					SetCurrentObjective(hintOverride);
				}
			}
					


			if(levelCompletion>=1.0f)
			{
				LevelComplete();
			}
			//if the level tasks are not done, keep counting time...
			if(!b_isLevelComplete)
			{
				//get amount of time passed since level began
				//may need to offset if there is a pause before start or anywhere in between
				totalTimePassed=(int)Time.timeSinceLevelLoad;
				
				//format seconds into common time format...
				//if it's been more than a minute...
				if(totalTimePassed>=60)
				{
					timePassedMin=totalTimePassed/60;
					//if it's been more than an hour...
					if(timePassedMin>=60)
					{
						timePassedHr=totalTimePassed/3600;
						timePassedMin=timePassedMin%60;
					}
				}
				timePassedSec=totalTimePassed%60;
			}
			else
			{
				EndLevel();
				//b_endingLevel=true;

			} 
		}
		
		if(levelType==LevelType.None)
		{
			EndLevel(true);
			//b_endingLevel=true;
		}
	}
	
	//called during Start() to check the level for any gameObjects tagged as objectives
	void CountObjectives()
	{
		levelObjectives=GameObject.FindGameObjectsWithTag("Objective").Length;
	}
	
	//Objectives call this to say when they have been completed
	public void ObjectiveCompleted()
	{
		objectivesCompleted++;
		levelCompletion=objectivesCompleted/(float)levelObjectives;
	}
	
	//Manually add to objective count (e.g. if objective added after the level beginning...)
	public void AddObjective()
	{
		levelObjectives++;
		levelCompletion=objectivesCompleted/(float)levelObjectives;
	}
	
	
	//display GUI (timer)
	void OnGUI()
	{
		if(gameControlScript)
		{
			if(!gameControlScript.OVRCam())
			{
				crosshairsPosition.Set(	Input.mousePosition.x-(crosshairsDimensions/2.0f), Screen.height-Input.mousePosition.y-(crosshairsDimensions/2.0f), crosshairsDimensions, crosshairsDimensions);
			
				//need to check whether player has oculus active...
				GUI.Label(timerShadowPosition, timerText, timerShadow);
				GUI.Label(timerPosition, timerText, timer);
				GUI.Label(objectiveShadowPosition, currentObjective, objectiveShadow);
				GUI.Label(objectivePosition, currentObjective, objective);
				GUI.Label(countdownShadowPosition, countdownText, countdownShadow);
				GUI.Label(countdownPosition, countdownText, countdown);
				
				if(b_showCrosshairs&&crosshairsTexture!=null)
				{
					//GUI.DrawTexture(crosshairsPosition, crosshairsTexture);
				}
			}
		}
	}
	
	void EndLevel(bool loadMenu=false)
	{
		if(gameControlScript)
		{

			if(!b_saved)
			{
				b_saved=true;
				switch(levelType)
				{
					case LevelType.ObjectInteraction:
						gameControlScript.SetOIScore(totalTimePassed);
						gameControlScript.SaveScore(levelType);//Save score to "OIScore.xml" file.
						break;		

					case LevelType.ObjectAvoidance:
						gameControlScript.SetOAScore(totalTimePassed);				
						gameControlScript.SaveScore(levelType);//Save score to "OAScore.xml" file.
						break;

					case LevelType.WayFinding:
						gameControlScript.SetWFScore(totalTimePassed);				
						gameControlScript.SaveScore(levelType);//Save score to "WFScore.xml" file.
						break;
						/*
					case LevelType.Video:
						
						break;
						*/
				}
			}
			
			loadCountdown-=Time.deltaTime;
			
			AdjustGUI();

			if(loadCountdown<=0.0f)
			{
				loadCountdown=0.0f;
			}
			
			countdownText="";
			if(!loadMenu&&nextLevelName!="")
			{
				countdownText="Loading next level in: "+loadCountdown.ToString("F2")+"...";
			}
			else
			{
				countdownText="Loading main menu in: "+loadCountdown.ToString("F2")+"...";
			}
			
			if(loadCountdown<=0.0f)
			{
				if(nextLevelName!="")
				{
					gameControlScript.LoadNextLevel(nextLevelName);
				}
				else
				{
					gameControlScript.LoadNextLevel("Main Menu");
				}
			}
		}
	}
	
	public void LevelComplete(bool complete=true)
	{
		b_isLevelComplete=complete;
	}
	
	public void ToggleTimer()
	{
		b_showTimer=!b_showTimer;
	}
	
	void AdjustGUI()
	{
		if(screenWidth!=Screen.width||screenHeight!=Screen.height)
		{
			screenWidth=Screen.width;
			screenHeight=Screen.height;
						
			timerHeight=screenHeight-(screenHeight*0.95f);
			timerPosition.Set(screenWidth/2.0f,timerHeight,0,0);
			timerShadowPosition.Set(screenWidth/2.0f+2,timerHeight+2,0,0);
			
			objectiveHeight=screenHeight-(screenHeight*0.05f);
			objectivePosition.Set(screenWidth/2.0f,objectiveHeight,0,0);
			objectiveShadowPosition.Set(screenWidth/2.0f+2,objectiveHeight+2,0,0);	
			
			countdownHeight=screenHeight-(screenHeight*0.15f);
			countdownPosition.Set(screenWidth/2.0f,countdownHeight,0,0);
			countdownShadowPosition.Set(screenWidth/2.0f+2,countdownHeight+2,0,0);
			
			/*
			crosshairsPosition.Set(	Input.mousePosition.x, 
									Input.mousePosition.y, 
									crosshairsDimensions, crosshairsDimensions);
			crosshairsPosition.Set(	(Screen.width-crosshairsDimensions)/2.0f, 
									(Screen.height-crosshairsDimensions)/2.0f, 
									crosshairsDimensions, crosshairsDimensions);
			*/
		}
	}
	
	public void SetCurrentObjective(string objectiveText, bool choice=true ,bool hint=false, float hintTime=5.0f)
	{
		hintDuration=hintTime;
		
		//should fade between previous objective to current objective first
		
		//if already showing hint
		if(b_showHint)
		{
			if(hint)//if new hint, keep previous objective with new hint...
			{
				hintTimerA=Time.time;

				hintMessage=objectiveText;
				b_showHint=hint;
				currentObjective=hintOverride;
				currentObjective+="\n"+hintMessage;
			}
			else//keep previous hint with new objective...
			{
				currentObjective=objectiveText;
				hintOverride=objectiveText;
				currentObjective+="\n"+hintMessage;
			}
		}
		else //if not showing hint already
		{
			if(hint)
			{
				hintTimerA=Time.time;
				
				hintMessage=objectiveText;
				b_showHint=hint;
				hintOverride=currentObjective;
				currentObjective+="\n"+hintMessage;
			}
			else
			{
				//previousObjective=currentObjective;
				currentObjective=objectiveText;
				hintOverride=objectiveText;
			}
		}
		DisplayCurrentObjective(choice);
	}
	
	public void DisplayCurrentObjective(bool choice=true)
	{	
		b_showObjective=choice;
	}
	
	//need to fade text in and out for objectivces, scores, flashing timers?...
	public void FadeText()
	{
	}

	public string GetTimerText()
	{
		return timerText;
	}
	
	public string GetCountDownText()
	{
		return countdownText;
	}
	
	public string GetObjectiveText()
	{
		return currentObjective;
	}
}