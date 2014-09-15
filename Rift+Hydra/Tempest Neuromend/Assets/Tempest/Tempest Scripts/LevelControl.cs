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
	
	public LevelType levelType;//PUBLIC Set in editor... not used yet
	public string nextLevelName="";//PUBLIC Set in editor
	
	private int totalTimePassed=0;
	private int timePassedSec=0;
	private int timePassedMin=0;
	private int timePassedHr=0;
	private string timePassedString="";
	
	public int levelObjectives=0;
	public float levelCompletion=0.0f;//0-1 percentage
	private int objectivesCompleted=0;
	
	public bool b_isLevelComplete = false;
	public bool b_showTimer = true;
	public bool b_showObjective = true;
	
	private GameObject gameControl;
	private GameControl gameControlScript;
	
	public float hintDuration=5.0f;
	
	private float screenWidth=0.0f;
	private float screenHeight=0.0f;
	
	//current objective text...
	Rect objectivePosition;
	public string currentObjective="";
	public string previousObjective="";
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

	float loadCountdown = 5.0f;//for next level load
	
	void Start()
	{
		timer.normal.textColor=Color.white;
		timer.fontSize=20;
		timer.alignment=TextAnchor.MiddleCenter;
		timer.fontStyle=FontStyle.Bold;
		
		timerShadow.normal.textColor=Color.black;
		timerShadow.fontSize=20;
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
	}
	
	void Awake()
	{
		CountObjectives();
	}
	
	void Update() 
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
		else EndLevel();
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
		GUI.Label(timerShadowPosition, timerText, timerShadow);
		GUI.Label(timerPosition, timerText, timer);
		GUI.Label(objectiveShadowPosition, currentObjective, objectiveShadow);
		GUI.Label(objectivePosition, currentObjective, objective);
		GUI.Label(countdownShadowPosition, countdownText, countdownShadow);
		GUI.Label(countdownPosition, countdownText, countdown);
	}
	
	void EndLevel()
	{
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			gameControlScript=gameControl.GetComponent<GameControl>();
			switch(levelType)
			{
				case LevelType.ObjectInteraction:
					gameControlScript.setOIScore(totalTimePassed);
					break;				
				case LevelType.ObjectAvoidance:
					gameControlScript.setOAScore(totalTimePassed);				
					break;
				case LevelType.WayFinding:
					gameControlScript.setWFScore(totalTimePassed);				
					break;
				/*
				case LevelType.video:
					
					break;
				*/
			}
			//need to have loadCountdown timer to signal going to next stage...		
			if(nextLevelName!="")
			{
				loadCountdown-=Time.deltaTime;
				
				AdjustGUI();

				countdownText="";
				countdownText="Loading next level in: "+loadCountdown.ToString("F2")+"...";
				
				if(loadCountdown<=0.0f)
				{
					gameControlScript.loadNextLevel(nextLevelName);
				}
			}
		}
	}
	
	public void LevelComplete(bool complete=true)
	{
		b_isLevelComplete=complete;
	}
	
	public void toggleTimer()
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
		}
	}
	
	public void SetCurrentObjective(string objectiveText, bool choice=true ,bool hint=false)
	{
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
				previousObjective=currentObjective;
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

}
