//for use with VRGUI package...
//attach to camera within OVRCameraController
using UnityEngine;
using System.Collections;

public class TempestVRGUI : VRGUI 
{
	private float screenWidth=0.0f;
	private float screenHeight=0.0f;
	
	//'up' timer
	Rect timerPosition;
	private string timerText;
	public GUIStyle timer;
	Rect timerShadowPosition;
	public GUIStyle timerShadow;
	float timerHeight;

	//current objective text...
	Rect objectivePosition;
	private string currentObjective="";
	public GUIStyle objective;
	Rect objectiveShadowPosition;
	public GUIStyle objectiveShadow;
	float objectiveHeight;

	//next level load timer...
	Rect countdownPosition;
	private string countdownText="";
	public GUIStyle countdown;
	Rect countdownShadowPosition;
	public GUIStyle countdownShadow;
	float countdownHeight;
	
	//score screen...
	Rect scoresPosition;
	private string scoresText;
	public GUIStyle scores;
	Rect scoresShadowPosition;
	public GUIStyle scoresShadow;
	float scoresHeight;

	Rect pauseLabelPosition;



	private bool b_isScoreScreen=false;

	private LevelControl levelControlScript=null;
	private ScoreScreen scoreScreenScript=null;

	private GameControl gameControlScript=null;

	void Awake()
	{
		GameObject theLevel = null;
		GameObject theGame = null;
		GameObject theScores = null;
		if(theLevel = GameObject.FindWithTag("Level"))
		{
			levelControlScript = theLevel.GetComponent<LevelControl>();
		}
		if(theGame = GameObject.FindWithTag("Game"))
		{
			gameControlScript = theGame.GetComponent<GameControl>();
		}
		if(theScores = GameObject.FindWithTag("ScoreScreen"))
		{
			b_isScoreScreen=true;
			scoreScreenScript = theScores.GetComponent<ScoreScreen>();
		}

	}
	
	void Start() 
	{
		screenWidth=Screen.width;
		screenHeight=Screen.height;
						
		timer.normal.textColor=Color.white;
		timer.fontSize=20;
		timer.alignment=TextAnchor.MiddleCenter;
		timer.fontStyle=FontStyle.Bold;
		
		timerShadow.normal.textColor=Color.black;
		timerShadow.fontSize=20;
		timerShadow.alignment=TextAnchor.MiddleCenter;
		timerShadow.fontStyle=FontStyle.Bold;
		
		timerHeight=screenHeight-(screenHeight*0.95f);
		timerPosition.Set(screenWidth/2.0f,timerHeight,0,0);
		timerShadowPosition.Set(screenWidth/2.0f+2,timerHeight+2,0,0);
			
		objective.normal.textColor=Color.white;
		objective.fontSize=20;
		objective.alignment=TextAnchor.MiddleCenter;
		objective.fontStyle=FontStyle.Bold;
		
		objectiveShadow.normal.textColor=Color.black;
		objectiveShadow.fontSize=20;
		objectiveShadow.alignment=TextAnchor.MiddleCenter;
		objectiveShadow.fontStyle=FontStyle.Bold;
		
		
		objectiveHeight=screenHeight-(screenHeight*0.05f);
		objectivePosition.Set(screenWidth/2.0f,objectiveHeight,0,0);
		objectiveShadowPosition.Set(screenWidth/2.0f+2,objectiveHeight+2,0,0);	
			
		countdown.normal.textColor=Color.red;
		countdown.fontSize=30;
		countdown.alignment=TextAnchor.MiddleCenter;
		countdown.fontStyle=FontStyle.Bold;

		countdownShadow.normal.textColor=Color.black;
		countdownShadow.fontSize=30;
		countdownShadow.alignment=TextAnchor.MiddleCenter;
		countdownShadow.fontStyle=FontStyle.Bold;

		countdownHeight=screenHeight-(screenHeight*0.15f);
		countdownPosition.Set(screenWidth/2.0f,countdownHeight,0,0);
		countdownShadowPosition.Set(screenWidth/2.0f+2,countdownHeight+2,0,0);

		scores.normal.textColor=Color.white;
		scores.fontSize=20;
		scores.alignment=TextAnchor.MiddleCenter;
		scores.fontStyle=FontStyle.Bold;
		
		scoresShadow.normal.textColor=Color.black;
		scoresShadow.fontSize=20;
		scoresShadow.alignment=TextAnchor.MiddleCenter;
		scoresShadow.fontStyle=FontStyle.Bold;
		
		scoresHeight=screenHeight-(screenHeight*0.5f);
		scoresPosition.Set(screenWidth/2.0f,scoresHeight,0,0);
		scoresShadowPosition.Set(screenWidth/2.0f+2,scoresHeight+2,0,0);
	}
	
	void Update() 
	{
	}

	public override void OnVRGUI()
	{		
		if(gameControlScript.Paused())
		{
			pauseLabelPosition.Set(screenWidth/2.0f+2,scoresHeight+2,0,0);
			GUI.Label(pauseLabelPosition, "Paused - press 'p' to continue", objectiveShadow);
			pauseLabelPosition.Set(screenWidth/2.0f,scoresHeight,0,0);
			GUI.Label(pauseLabelPosition, "Paused - press 'p' to continue", objective);
		}
		else
		{
			//Screen.lockCursor=true;
			if(!b_isScoreScreen)
			{
				timerText = levelControlScript.GetTimerText();
				currentObjective = levelControlScript.GetObjectiveText();
				
				GUI.Label(timerShadowPosition, timerText, timerShadow);
				GUI.Label(timerPosition, timerText, timer);
				GUI.Label(objectiveShadowPosition, currentObjective, objectiveShadow);
				GUI.Label(objectivePosition, currentObjective, objective);
			}
			else
			{
				scoresText = scoreScreenScript.GetScoreText();
				
				GUI.Label(scoresShadowPosition, scoresText, scoresShadow);
				GUI.Label(scoresPosition, scoresText, scores);
			}
			countdownText = levelControlScript.GetCountDownText();
			GUI.Label(countdownShadowPosition, countdownText, countdownShadow);
			GUI.Label(countdownPosition, countdownText, countdown);
		}
	}
}
