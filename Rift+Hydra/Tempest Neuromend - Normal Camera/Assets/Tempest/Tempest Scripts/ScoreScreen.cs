using UnityEngine;
using System.Collections;

public class ScoreScreen : VRGUI
{
	private float screenWidth=0.0f;
	private float screenHeight=0.0f;
	
	public float objectInteractionScore = 0.0f;
	public float objectAvoidanceScore = 0.0f;
	public float wayFindingScore = 0.0f;

	Rect scoresPosition;
	string scoresText;
	public GUIStyle scores;
	Rect scoresShadowPosition;
	public GUIStyle scoresShadow;
	float scoresHeight;


	GameObject gameControl=null;
	GameControl gameControlScript=null;
	GameObject levelControl=null;
	LevelControl levelControlScript=null;

	void Awake() 
	{
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			gameControlScript=gameControl.GetComponent<GameControl>();
			if(gameControlScript)
			{
				objectInteractionScore = gameControlScript.GetOIScore();
				objectAvoidanceScore = gameControlScript.GetOAScore();
				wayFindingScore = gameControlScript.GetWFScore();
			}
		}
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
			if(levelControlScript)
			{
				levelControlScript.ShowTimer(false);
			}
		}
	}
	
	void Start() 
	{
		scores.normal.textColor=Color.white;
		scores.fontSize=20;
		scores.alignment=TextAnchor.MiddleCenter;
		scores.fontStyle=FontStyle.Bold;
		
		scoresShadow.normal.textColor=Color.black;
		scoresShadow.fontSize=20;
		scoresShadow.alignment=TextAnchor.MiddleCenter;
		scoresShadow.fontStyle=FontStyle.Bold;
	}

	void Update() 
	{
		AdjustGUI();
		if(gameControlScript)
		{
			scoresText="SCORES";
			if(gameControlScript.IsObjectInteractionScore())
			{
				scoresText+="\nObject Interaction: "+TempestUtil.FormatSeconds((int)objectInteractionScore);
			}
			if(gameControlScript.IsObjectAvoidanceScore())
			{
				scoresText+="\nObject Avoidance: "+TempestUtil.FormatSeconds((int)objectAvoidanceScore);
			}
			if(gameControlScript.IsWayFindingScore())
			{
				scoresText+="\nWay Finding: "+TempestUtil.FormatSeconds((int)wayFindingScore);
			}
		}
		//scoresText="SCORES\nObject Interaction: "+TempestUtil.FormatSeconds((int)objectInteractionScore)+"\nObject Avoidance: "+TempestUtil.FormatSeconds((int)objectAvoidanceScore)+"\nWay Finding: "+TempestUtil.FormatSeconds((int)wayFindingScore);
	}

	new void OnGUI()
	{
		GUI.Label(scoresShadowPosition, scoresText, scoresShadow);
		GUI.Label(scoresPosition, scoresText, scores);
	}

	public override void OnVRGUI()
	{
		GUI.Label(scoresShadowPosition, scoresText, scoresShadow);
		GUI.Label(scoresPosition, scoresText, scores);
	}

	void AdjustGUI()
	{
		if(screenWidth!=Screen.width||screenHeight!=Screen.height)
		{
			screenWidth=Screen.width;
			screenHeight=Screen.height;
						
			scoresHeight=screenHeight-(screenHeight*0.5f);
			scoresPosition.Set(screenWidth/2.0f,scoresHeight,0,0);
			scoresShadowPosition.Set(screenWidth/2.0f+2,scoresHeight+2,0,0);
		}
	}
	
	public string GetScoreText()
	{
		return scoresText;
	}
}
