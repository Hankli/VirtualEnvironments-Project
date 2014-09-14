using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour 
{
	private GameObject levelControl;
	private LevelControl levelControlScript;
	public string hintMessage="";

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
		//if the player entered...
		if(other.gameObject.tag=="Player")
		{
			//send message to be displayed to level control
			if(levelControl!=null)
			{
				levelControlScript.SetCurrentObjective(hintMessage,true,true);
			}
		}
	}
}
