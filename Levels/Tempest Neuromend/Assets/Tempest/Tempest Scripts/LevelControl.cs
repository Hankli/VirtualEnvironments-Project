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
	public float levelCompletion=0.0f;
	
	public bool b_isLevelComplete = false;
	public bool b_showTimer = true;
	
	private GameObject gameControl;
	private GameControl gameControlScript;
	
	private float screenWidth=0.0f;
	private float screenHeight=0.0f;

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
	
	
	//Current objective...
	
		
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
		
		countdown.normal.textColor=Color.red;
		countdown.fontSize=30;
		countdown.alignment=TextAnchor.MiddleCenter;
		countdown.fontStyle=FontStyle.Bold;

		countdownShadow.normal.textColor=Color.black;
		countdownShadow.fontSize=30;
		countdownShadow.alignment=TextAnchor.MiddleCenter;
		countdownShadow.fontStyle=FontStyle.Bold;

	}
	
	void Update() 
	{
		//display the timer if needed
		if(b_showTimer)
		{
			//get screen dimensions if changed, adjust timer position
			AdjustGUI();
						
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
	
	//display GUI (timer)
	void OnGUI()
	{
		GUI.Label(timerShadowPosition, timerText, timerShadow);
		GUI.Label(timerPosition, timerText, timer);
		GUI.Label(countdownShadowPosition, countdownText, countdownShadow);
		GUI.Label(countdownPosition, countdownText, countdown);
	}
	
	void EndLevel()
	{
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			gameControlScript=gameControl.GetComponent<GameControl>();
			string levelTag = gameObject.tag.ToString();
			switch(levelTag)
			{
				case "ObjectInteraction":
					gameControlScript.setOIScore(totalTimePassed);
					break;				
				case "ObjectAvoidance":
					gameControlScript.setOAScore(totalTimePassed);				
					break;
				case "WayFinding":
					gameControlScript.setWFScore(totalTimePassed);				
					break;
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
	
	public void LevelComplete(bool complete)
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
			countdownHeight=screenHeight-(screenHeight*0.15f);
			
			timerPosition.Set(screenWidth/2.0f,timerHeight,0,0);
			timerShadowPosition.Set(screenWidth/2.0f+2,timerHeight+2,0,0);
			
			countdownPosition.Set(screenWidth/2.0f,countdownHeight,0,0);
			countdownShadowPosition.Set(screenWidth/2.0f+2,countdownHeight+2,0,0);
		}
	}

}
