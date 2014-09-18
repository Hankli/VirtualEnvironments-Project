using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour 
{
	private GameObject levelControl = null;
	private LevelControl levelControlScript = null;
	public string hintMessage="";
	public bool b_allowedMultipleTriggers=false;
	private bool b_hasTriggered=false;

	void Awake()
	{
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
		}	
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
			if(levelControl!=null)
			{
				levelControlScript.SetCurrentObjective(hintMessage,true,true);
			}
		}
	}
}
