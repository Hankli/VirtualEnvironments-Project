// NOTE: File I/O has been added in this class. 17/09/2014 Anopan
//       All data will be saved in the Root Directory.
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

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
	private string OIPath = null;
	private string OAPath = null;
	private string WFPath = null;

		//Used for writing to files.
    private System.IO.StreamWriter fileWriter;


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
    
    void Start()
    {
		OIPath = userID + "_OIScore.xml";
		OAPath = userID + "_OAScore.xml";
		WFPath = userID + "_WFScore.xml";
	}
    
	void Update()
	{
		Screen.lockCursor=true;
	}
    
    public void SetUserID(int number)
    {
		userID = number;

		OIPath = userID + "_OIScore.xml";
		OAPath = userID + "_OAScore.xml";
		WFPath = userID + "_WFScore.xml";
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
		//WaitAFew();
		Application.LoadLevel(levelName);
    }
    /*
    IEnumerator WaitAFew()
    {
		yield return new WaitForSeconds(3);
    }
    */
    
    
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

	public void SaveScore(int level)
	{
		XmlWriterSettings setting = new XmlWriterSettings ();
		setting.Indent = true;
		XmlWriter writer;

		//ready variables that relate to the level completion report
		string path = null;
		string activity = null;
		string score = null;
		string profileID = null;
		string device = controllerType.ToString ();
		string date = System.DateTime.Now.ToLongDateString ();

		//all level specific data values are set below
		switch(level)
		{
			case 1:
			{
				path = @OIPath;
				activity = "Object Interaction";
				score = objectInteractionScore.ToString();
			}	
			break;

			case 2:
			{
				path = @OAPath;
				activity = "Object Avoidance";
				score = objectAvoidanceScore.ToString();
			}
			break;

			case 3:
			{
				path = @WFPath;
				activity = "Way Finding";
				score = wayFindingScore.ToString();
			}
			break;
		}

		//get last position of separator between client ID and activity type
		int idSection = path.LastIndexOf("_");
	    profileID = path.Substring(0, idSection);
	
		//write all data to file
		writer = XmlWriter.Create(path, setting);
		writer.WriteStartDocument ();
		writer.WriteStartElement ("Level Report");

		writer.WriteElementString ("Profile ID", profileID);
		writer.WriteElementString ("Device Type", device);
		writer.WriteElementString ("Activity Type", activity);
		writer.WriteElementString ("Completion Date", date);
		writer.WriteElementString ("Completion Score", score.ToString());

		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Flush();
		writer.Close();
	}

	public void WriteLevelReports()
	{
		const int TOTAL = 3;
		XElement[] xelems = new XElement[TOTAL];

		//try load all latest level data
		xelems[0] = XElement.Load (@OAPath);
		xelems[1] = XElement.Load (@OIPath);
		xelems[2] = XElement.Load (@WFPath);

		for(int i=0; i<TOTAL; i++)
		{
			if(xelems[i] != null)
			{
				foreach(XElement x in xelems[i].Elements("Level Report"))
				{
					string activityType = x.Element("Activity Type").Value;
					string profileID = x.Element("Profile ID").Value;
					string deviceType = x.Element ("Device Type").Value;
					string score = x.Element("Completion Score").Value;

					System.IFormatProvider format = new System.Globalization.CultureInfo("fr-FR", true);
					System.DateTime date = System.DateTime.Parse(x.Element ("Completion Date").Value, format);
					string sqlDate = date.Date.ToString("yyyy-MM-dd HH:mm:ss");


					//write to db
				}
			}
		}


	}

	/*
	public void SaveScore(int level)
	{
        switch (level)
        {
            //Object Interaction.
            case 1:
				ReadyFile(OIPath);
                fileWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   objectInteractionScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
				fileWriter.Close();
                break;

            //Object Avoidance.
            case 2:
				ReadyFile(OAPath);
                fileWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   objectAvoidanceScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
				fileWriter.Close();
                break;

            //Way Finding.
            case 3:
				ReadyFile(WFPath);
                fileWriter.WriteLine("{0}\t{1}\t{2} {3}",
                                   userID,
                                   wayFindingScore,
                                   System.DateTime.Now.ToShortDateString(),
                                   System.DateTime.Now.ToLongTimeString());
				fileWriter.Close();
                break;
        }
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
	*/

}
