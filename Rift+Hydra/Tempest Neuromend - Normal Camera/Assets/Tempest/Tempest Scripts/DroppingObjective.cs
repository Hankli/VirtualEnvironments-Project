using UnityEngine;
using System.Collections;

public class DroppingObjective : ThrowingObjective 
{
	new void Awake() 
	{
		base.Awake();
	}
	
	new void Start() 
	{
		objectiveText="Objective:\nPick up and drop the objects into thier corresponding portals";
		
		objectiveTextUpdated=objectiveText;
		//objectiveTextUpdated+="\n(x"+(maxNumberOfGoals-currentScore)+")";
		objectiveTextUpdated+="\n("+currentScore+"/"+maxNumberOfGoals+")";
	}
	
	new void Update() 
	{
		base.Update();
	}
}
