using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour 
{
	private GameObject levelControl;
	private LevelControl levelControlScript;
	public string hintMessage="";
	public bool b_allowedMultipleTriggers=false;
	private bool b_hasTriggered=false;

	void Start() 
	{
	}
	
	void Awake()
	{
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
		}	
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		//if the rightHand entered...
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
