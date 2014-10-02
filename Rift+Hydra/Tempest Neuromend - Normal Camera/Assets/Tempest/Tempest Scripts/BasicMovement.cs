using UnityEngine;
using System.Collections;

public class BasicMovement : MonoBehaviour 
{
	public float speed=1.0f;
	public Vector3 direction = Vector3.zero;
	
	public bool b_pointToPoint=false;
	//public Vector3 startPoint=Vector3.zero;
	//public Vector3 endPoint=Vector3.zero;
	//private bool b_currentPoint=true;
	

	void Start(){}
	
	void Update() 
	{
		if(b_pointToPoint)
		{
			
		}
		else
		{
			transform.Translate(Time.deltaTime*direction.x*speed, Time.deltaTime*direction.y*speed, Time.deltaTime*direction.z*speed);
		}
	}
}
