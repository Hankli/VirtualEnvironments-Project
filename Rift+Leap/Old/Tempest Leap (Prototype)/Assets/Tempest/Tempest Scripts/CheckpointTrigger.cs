using UnityEngine;
using System.Collections;

//used for checkpoint and game score updating...
public class CheckpointTrigger : MonoBehaviour 
{

	[Tooltip("The tag of the object intended to trigger this checkpoint")]
	public string triggerObjectTag ="";//trigger object tag must be input in editor
	[Tooltip("The overall level completion percentage this checkpoint represents (may change to precentage value rather than overall percentage...)")]
	public float checkpointValue = 0.0f;//level complete percentage set in unity editor
	[Tooltip("Freeze the player when this object is triggered?")]
	public bool b_freezePlayerOnTrigger = false;
	private bool b_hasBeenTriggered=false;
		
	void Start() {}
	
	void Update() {}
	
	void OnTriggerEnter(Collider theObject)
	{
		if(!b_hasBeenTriggered)
		{
			//need to check if correct object triggered it...
			if(theObject.gameObject.tag==triggerObjectTag)
			{
				GameObject gameControl = null;
				if(gameControl=GameObject.FindWithTag("Game"))
				{
					GameObject levelControl = null;
					if(levelControl=GameObject.Find("Level Control"))
					{
						//maybe move this check somewhere else and have checkpoint values add up?
						if(checkpointValue>=100)
						{
							levelControl.GetComponent<LevelControl>().LevelComplete(true);
						}
						
						GameControl gameControlScript = null;
						if(gameControlScript=gameControl.GetComponent<GameControl>())
						{
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
				}
				b_hasBeenTriggered=true;
				
				if(b_freezePlayerOnTrigger)
				{
					GameObject player = null;
					
					if(player=GameObject.FindWithTag("Player"))
					{
						CharacterMotor playerControl = null;
						if(playerControl=player.GetComponent<CharacterMotor>())
						{
							playerControl.SetControllable(false);
						}
					}
				}
			}
		}
	}
}
