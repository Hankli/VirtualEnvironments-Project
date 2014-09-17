using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour 
{
	public enum ControllerType
	{
		MouseKeyboard,
		OculusLeap,
		OculusHydra,
		OculusKinect
	};
	
	public ControllerType controllerType;

	//all variables here on to be public for debug only(Ary)
	public float objectInteractionScore = 0.0f;
	public float objectInteractionCheckpoint = 0.0f;
	public float objectAvoidanceScore = 0.0f;
	public float objectAvoidanceCheckpoint = 0.0f;
	public float wayFindingScore = 0.0f;
	public float wayFindingCheckpoint = 0.0f;

	/*
	//for use with networked database data...
	uint userID = 0.0f;
	float objectInteractionHighScore = 0.0f;
	float objectAvoidanceHighScore = 0.0f;
	float wayFindingHighScore = 0.0f;
    */
    void Start()
    {
    }
    
    void Awake() 
    {
        DontDestroyOnLoad(transform.gameObject);
		//need this to find any extra Game Control objects on new level loads and destroy...
		/*
		if(GameObject.Find("Game Control DEBUG"))
        {
			Destroy(GameObject.Find("Game Control DEBUG"));
        }
        */
    }
    
    void Update()
    {
    }
    
    //returns object interaction score float value
    public float GetOIScore()
    {
		return objectInteractionScore;
    }

    public void setOIScore(float score)
    {
		objectInteractionScore=score;
    }
    
    public void setOICheckpoint(float percentageComplete)
    {
		objectInteractionCheckpoint=percentageComplete;
    }
    
    
    //returns object avoidance score float value
    public float GetOAScore()
    {
		return objectAvoidanceScore;
    }
    
    public void setOAScore(float score)
    {
		objectAvoidanceScore=score;
    }
    
    public void setOACheckpoint(float percentageComplete)
    {
		objectAvoidanceCheckpoint=percentageComplete;
    }
    
    //returns way finding score float value
    public float GetWFScore()
    {
		return wayFindingScore;
    }
    
    public void setWFScore(float score)
    {
		wayFindingScore=score;
    }
    
    public void setWFCheckpoint(float percentageComplete)
    {
		wayFindingCheckpoint=percentageComplete;
    }
    
    /*
    //returns userID
    uint getUserID()
    {
		return userID;
    }
    */
    public void loadNextLevel(string levelName)
    {
		waitAFew();
		Application.LoadLevel(levelName);
    }
    
    IEnumerator waitAFew()
    {
		yield return new WaitForSeconds(3);
    }
    
}