using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour 
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


	GameObject gameControl;
	GameControl gameControlScript;
	
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
	
	void Awake() 
	{
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			gameControlScript=gameControl.GetComponent<GameControl>();
			objectInteractionScore = gameControlScript.GetOIScore();
			objectAvoidanceScore = gameControlScript.GetOAScore();
			wayFindingScore = gameControlScript.GetWFScore();
		}
	}

	void Update() 
	{
		AdjustGUI();
		
		scoresText="SCORES\nObject Interaction: "+objectInteractionScore+"\nObject Avoidance: "+objectAvoidanceScore+"\nWay Finding: "+wayFindingScore;
	}

	void OnGUI()
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
}
