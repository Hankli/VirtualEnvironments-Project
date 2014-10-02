using UnityEngine;
using System.Collections;

public class RabbitClone : MonoBehaviour 
{
	public GameObject lastNode = null;
	GameObject splineRootObject = null;
	
	
	public Vector3 bleh;


	// Use this for initialization
	void Start() 
	{
	
	}
	
	// Update is called once per frame
	void Update() 
	{
		bleh=lastNode.transform.parent.TransformPoint(lastNode.transform.localPosition);
		if(lastNode!=null)
		{
		
			if(transform.position==lastNode.transform.parent.TransformPoint(lastNode.transform.localPosition))
			{
				//destroy spline root object and this object...
				if(splineRootObject!=null)
				{
					Destroy(splineRootObject);
					Destroy(gameObject);
				}
				
			}
		}
	}
	
	public void InitSplineRoot()
	{	
		SplineController splineControl = gameObject.GetComponent<SplineController>();
		splineRootObject = splineControl.SplineRoot;
		
		if(splineRootObject!=null)
		{
			lastNode = splineRootObject.transform.GetChild(splineRootObject.transform.childCount-1).gameObject;
		}
		
	}
}
