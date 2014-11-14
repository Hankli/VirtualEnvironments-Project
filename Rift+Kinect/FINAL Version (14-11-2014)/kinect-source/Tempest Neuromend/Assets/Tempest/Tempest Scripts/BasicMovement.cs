using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour 
{
	
	
	public bool b_translation=false;
	public bool b_pointToPoint=false;
	public Vector3 translationDirection = Vector3.zero;
	public float translationSpeed=1.0f;
	
	public bool b_rotation=false;
	public Vector3 rotationAngle = Vector3.zero;
	public float rotationSpeed=1.0f;
	//public Vector3 startPoint=Vector3.zero;
	//public Vector3 endPoint=Vector3.zero;
	//private bool b_currentPoint=true;
	

	void Start(){}
	
	void Update() 
	{
		if(b_translation)
		{
			if(b_pointToPoint)
			{
				
			}
			else
			{
				transform.Translate(Time.deltaTime*translationDirection.x*translationSpeed, Time.deltaTime*translationDirection.y*translationSpeed, Time.deltaTime*translationDirection.z*translationSpeed);
			}
		}
		
		
		if(b_rotation)
		{
			transform.Rotate(Time.deltaTime*rotationAngle.x*rotationSpeed, Time.deltaTime*rotationAngle.y*rotationSpeed, Time.deltaTime*rotationAngle.z*rotationSpeed);
		}
	}
}
