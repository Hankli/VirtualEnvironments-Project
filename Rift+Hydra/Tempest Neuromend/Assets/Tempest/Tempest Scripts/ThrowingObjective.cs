using UnityEngine;
using System.Collections;

public class ThrowingObjective : MonoBehaviour 
{

	[Tooltip("The number of 'goals' the player must score to complete this objective")]
	public int maxNumberOfGoals=5;
	private int currentScore=0;
	private int currentErrors=0;//not really used
	private bool b_isActive=false;
	
	//private Transform spawner;
	//private ThrowableSpawner spawnerScript;

	private GameObject levelControl=null;
	private LevelControl levelControlScript=null;

	private string objectiveText="Objective:\nPick up and throw the objects into thier corresponding windows";
	private string objectiveTextUpdated="";

	void Awake() 
	{
		//spawner =gameObject.transform.GetChild(0);//may need to change to... get child with name 'ObjectSpawner'
		//spawnerScript = spawner.GetComponent<ThrowableSpawner>();
		
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
		}	
	}
	
	void Start() 
	{
		objectiveTextUpdated=objectiveText;
		objectiveTextUpdated+="\n(x"+(maxNumberOfGoals-currentScore)+")";
	}
	
	void Update() 
	{
	}
	
	public void SetActive(bool choice=true)
	{
		b_isActive=choice;
		UpdateObjectiveText();
	}
	
	public void AdjustScore(int value=1)
	{
		if(b_isActive)
		{
			currentScore+=value;
			UpdateObjectiveText();
			//if goal score is reached...
			if(currentScore>=maxNumberOfGoals)
			{
				b_isActive=false;
				if(levelControlScript!=null)
				{
					levelControlScript.ObjectiveCompleted();
				}
				
			}
		}
	}
	
	public void AdjustErrors(int value=1)
	{
		if(b_isActive)
		{
			currentErrors+=value;
		}
	}
	
	public void UpdateObjectiveText()
	{
		if(levelControlScript!=null)
		{
			objectiveTextUpdated=objectiveText;
			objectiveTextUpdated+="\n(x"+(maxNumberOfGoals-currentScore)+")";
			levelControlScript.SetCurrentObjective(objectiveTextUpdated,true);
		}
	}
}
