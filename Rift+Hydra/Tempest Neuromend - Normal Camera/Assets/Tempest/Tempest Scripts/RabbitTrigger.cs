using UnityEngine;
using System.Collections;

//should be animal trigger...
public class RabbitTrigger : HintTrigger 
{
	private GameObject splineRootObject = null;
	//new public string hintMessage="Sometimes you will notice small animals running through the maze.\nThey will usually be headed in the best direction.";
	//private GameObject[] animals;
	private GameObject animal;

	void Awake()
	{
		if(splineRootObject=this.transform.GetChild(0).gameObject)
		{
		}
	}
	
	void Start() 
	{
		hintMessage="Sometimes you will notice small animals running through the maze.\nThey will usually be headed in the best direction.";
		b_allowedMultipleTriggers=true;
		b_hasTriggered=false;
		LoadAnimals();
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		//if the player entered...
		if(other.gameObject.tag=="Player")
		{
			if(b_allowedMultipleTriggers||!b_hasTriggered)
			{
				GameObject animalClone;
				GameObject splineClone;
				Vector3 pos=Vector3.zero;
				pos.y=-1000.0f;
				Vector3 splinePos=splineRootObject.transform.position;
				//vector3 splinePos=splineRootObject.transform.position;
				animalClone=Instantiate(animal, pos, Quaternion.identity)as GameObject;
				splineClone=Instantiate(splineRootObject, splinePos, Quaternion.identity)as GameObject;
				SplineController splineControl=animalClone.GetComponent<SplineController>();
				RabbitClone rabbitScript = animalClone.GetComponent<RabbitClone>();
				if(splineControl!=null)
				{
					splineControl.SetSplineInfo(splineClone);
					rabbitScript.InitSplineRoot();
				}
			}
			
			//this next bit should check whether ANY rabbit triggers have shown a hint yet...
			//or select a similar but random hint associated to display...
			if(!b_hasTriggered)
			{
				b_hasTriggered=true;
			
				//send message to be displayed to level control
				GameObject levelControl = null;
				LevelControl levelControlScript = null;
				if(levelControl=GameObject.FindWithTag("Level"))
				{
					if(levelControlScript=levelControl.GetComponent<LevelControl>())
					{
						levelControlScript.SetCurrentObjective(hintMessage,true,true,7.0f);
					}
				}	
			}
			
		}
	}

	void LoadAnimals()
	{
		animal=Resources.Load<GameObject>("Prefabs/Misc/Rabbit");
	}

}
