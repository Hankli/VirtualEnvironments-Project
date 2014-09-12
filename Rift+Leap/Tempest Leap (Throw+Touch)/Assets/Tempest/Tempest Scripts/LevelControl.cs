using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour 
{

	public string nextLevelName="";
	
	
	//all variables here on to be public for debug only(Ary)
	public int totalTimePassed=0;
	public int timePassedSec=0;
	public int timePassedMin=0;
	public int timePassedHr=0;
	public string timePassedString="";
	public float levelCompletion=0.0f;
	
	public bool b_isLevelComplete = false;
	public bool b_showTimer = true;
	
	public GameObject gameControl;
	public GameControl gameControlScript;
	
	public float screenWidth=0.0f;
	public float screenHeight=0.0f;

	//'up' timer
	public Rect labelPosition;
	public string labelText;
	public GUIStyle labelStyle;

	public Rect labelShadowPosition;
	public GUIStyle labelStyleShadow;

	public float timerHeight;


	//next level load timer...
	public Rect countdownPosition;
	public string countdownText="";
	public GUIStyle countdownStyle;

	public Rect countdownShadowPosition;
	public GUIStyle countdownStyleShadow;

	public float countdownHeight;

	float countdown = 5.0f;//for next level load
	
		
	void Start()
	{
		labelStyle.normal.textColor=Color.white;
		labelStyle.fontSize=20;
		labelStyle.alignment=TextAnchor.MiddleCenter;
		labelStyle.fontStyle=FontStyle.Bold;
		
		labelStyleShadow.normal.textColor=Color.black;
		labelStyleShadow.fontSize=20;
		labelStyleShadow.alignment=TextAnchor.MiddleCenter;
		labelStyleShadow.fontStyle=FontStyle.Bold;
		
		countdownStyle.normal.textColor=Color.red;
		countdownStyle.fontSize=30;
		countdownStyle.alignment=TextAnchor.MiddleCenter;
		countdownStyle.fontStyle=FontStyle.Bold;

		countdownStyleShadow.normal.textColor=Color.black;
		countdownStyleShadow.fontSize=30;
		countdownStyleShadow.alignment=TextAnchor.MiddleCenter;
		countdownStyleShadow.fontStyle=FontStyle.Bold;

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

			labelText=timePassedString;
		}
		else
		{
			labelText="";
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
		GUI.Label(labelShadowPosition, labelText, labelStyleShadow);
		GUI.Label(labelPosition, labelText, labelStyle);
		GUI.Label(countdownShadowPosition, countdownText, countdownStyleShadow);
		GUI.Label(countdownPosition, countdownText, countdownStyle);
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
			//need to have countdown timer to signal going to next stage...		
			if(nextLevelName!="")
			{
				countdown-=Time.deltaTime;
				
				AdjustGUI();

				countdownText="";
				countdownText="Loading next level in: "+countdown.ToString("F2")+"...";
				
				if(countdown<=0.0f)
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
			
			labelPosition.Set(screenWidth/2.0f,timerHeight,0,0);
			labelShadowPosition.Set(screenWidth/2.0f+2,timerHeight+2,0,0);
			
			countdownPosition.Set(screenWidth/2.0f,countdownHeight,0,0);
			countdownShadowPosition.Set(screenWidth/2.0f+2,countdownHeight+2,0,0);
		}
	}

}
