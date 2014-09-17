using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour 
{

	[Tooltip("The tag of the object intended to trigger this checkpoint")]
	public string triggerObjectTag ="";//trigger object tag must be input in editor
	[Tooltip("The overall level completion percentage this checkpoint represents (may change to precentage value rather than overall percentage...)")]
	public float checkpointValue = 0.0f;//level complete percentage set in unity editor
	
	private GameObject levelControl;
	private GameObject gameControl;
	private GameControl gameControlScript;
	private bool b_hasBeenTriggered=false;
	
		
	void Start() 
	{
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider theObject)
	{
		if(!b_hasBeenTriggered)
		{
			//need to check if correct object triggered it...
			if(theObject.gameObject.tag==triggerObjectTag)
			{
				if(gameControl=GameObject.FindWithTag("Game"))
				{
					if(levelControl=GameObject.Find("Level Control"))
					{
						//maybe move this check somewhere else and have checkpoint values add up?
						if(checkpointValue>=100)
						{
							levelControl.GetComponent<LevelControl>().LevelComplete(true);
						}
						
						gameControlScript=gameControl.GetComponent<GameControl>();
						string levelTag = levelControl.tag.ToString();
						switch(levelTag)
						{
							case "ObjectInteraction":
								gameControlScript.SetOICheckpoint(checkpointValue);
								break;				
							case "ObjectAvoidance":
								gameControlScript.SetOACheckpoint(checkpointValue);				
								break;
							case "WayFinding":
								gameControlScript.SetWFCheckpoint(checkpointValue);				
								break;
						}
					}
				}
				b_hasBeenTriggered=true;
			}
		}
	}
}
