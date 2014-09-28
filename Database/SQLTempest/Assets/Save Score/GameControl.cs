// NOTE: File I/O has been added in this class. 17/09/2014 Anopan
//       All data will be saved in the Root Directory.

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

	
	//for use with networked database data...
	private int userID = 0;
    /*
	float objectInteractionHighScore = 0.0f;
	float objectAvoidanceHighScore = 0.0f;
	float wayFindingHighScore = 0.0f;
    */

        //Paths to files for saving of scores.
    private string OIpath = "OIScore.txt";
    private string OApath = "OAScore.txt";
    private string WFpath = "WFScore.txt";

        //Used for writing to files.
    private System.IO.StreamWriter OIWriter;
    private System.IO.StreamWriter OAWriter;
    private System.IO.StreamWriter WFWriter;

    void Start()
    {
        //File initialisation.
        if (System.IO.File.Exists(OIpath))
        {
            OIWriter = System.IO.File.AppendText(OIpath);
        }//end of if.
        else
        {
            OIWriter = System.IO.File.AppendText(OIpath);
            OIWriter.WriteLine("UserID\tScore\tDate");
        }//end of else.

        if (System.IO.File.Exists(OApath))
        {
            OAWriter = System.IO.File.AppendText(OApath);
        }//end of if.
        else
        {
            OAWriter = System.IO.File.AppendText(OApath);
            OAWriter.WriteLine("UserID\tScore\tDate");
        }//end of else.

        if (System.IO.File.Exists(WFpath))
        {
            WFWriter = System.IO.File.AppendText(WFpath);
        }//end of if.
        else
        {
            WFWriter = System.IO.File.AppendText(WFpath);
            WFWriter.WriteLine("UserID\tScore\tDate");
        }//end of else.
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
    
        //returns userID
    public int getUserID()
    {
		return userID;
    }
    
    public void loadNextLevel(string levelName)
    {
		waitAFew();
		Application.LoadLevel(levelName);
    }
    
    IEnumerator waitAFew()
    {
		yield return new WaitForSeconds(3);
    }

    public void saveScore(int level)
    {
        switch (level)
        {
            //Object Interaction.
            case 1:
                OIWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   objectInteractionScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
                break;

            //Object Avoidance.
            case 2:
                OAWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   objectAvoidanceScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
                break;

            //Way Finding.
            case 3:
                WFWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   wayFindingScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
                break;
        }//end of switch.
    }//end of saveScore.

        //Close the file writers.
    public void OnApplicationQuit()
    {
        OIWriter.Close();
        OAWriter.Close();
        WFWriter.Close();
    }
}