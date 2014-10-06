using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour 
{
	public string hintMessage="";
	public bool b_allowedMultipleTriggers=false;
	public bool b_hasTriggered=false;

	void Awake()
	{
	}
	
	void Start() 
	{
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		//if the player entered...
		if(!b_hasTriggered&&other.gameObject.tag=="Player")
		{
			if(!b_allowedMultipleTriggers)
			{
				b_hasTriggered=true;
			}
			//send message to be displayed to level control
			GameObject levelControl = null;
			LevelControl levelControlScript = null;
			if(levelControl=GameObject.FindWithTag("Level"))
			{
				
				if(levelControlScript=levelControl.GetComponent<LevelControl>())
				{
					levelControlScript.SetCurrentObjective(hintMessage,true,true);
				}
			}	
		}
	}
}
