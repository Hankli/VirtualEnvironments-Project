using UnityEngine;
using System.Collections;

public class ThrowingObjective : MonoBehaviour 
{

	[Tooltip("The number of 'goals' the player must score to complete this objective")]
	public int maxNumberOfGoals=5;
	protected int currentScore=0;
	protected int currentErrors=0;//not really used
	protected bool b_isActive=false;
	
	//private Transform spawner;
	//private ThrowableSpawner spawnerScript;

	protected GameObject levelControl=null;
	protected LevelControl levelControlScript=null;

	protected string objectiveText="Objective:\nPick up and place the objects into thier corresponding windows";
	protected string objectiveTextUpdated="";

	public virtual void Awake() 
	{
		//spawner =gameObject.transform.GetChild(0);//may need to change to... get child with name 'ObjectSpawner'
		//spawnerScript = spawner.GetComponent<ThrowableSpawner>();
		
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
		}	
	}
	
	public virtual void Start() 
	{
		objectiveTextUpdated=objectiveText;
		//objectiveTextUpdated+="\n(x"+(maxNumberOfGoals-currentScore)+")";
		objectiveTextUpdated+="\n("+currentScore+"/"+maxNumberOfGoals+")";
	}
	
	public virtual void Update() 
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
			//objectiveTextUpdated+="\n(x"+(maxNumberOfGoals-currentScore)+")";
			objectiveTextUpdated+="\n("+currentScore+"/"+maxNumberOfGoals+")";
			levelControlScript.SetCurrentObjective(objectiveTextUpdated,true);
		}
	}
}
