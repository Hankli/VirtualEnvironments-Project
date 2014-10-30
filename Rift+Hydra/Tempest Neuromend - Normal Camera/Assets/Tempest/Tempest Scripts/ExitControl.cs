using UnityEngine;
using System.Collections;

public class ExitControl : MonoBehaviour 
{

	public GameObject theTarget = null;
	public Vector3 exitOffset = new Vector3(0,5,0);
	
	void Start() 
	{
	
	}
	
	void Update() 
	{
		theTarget=GameObject.FindWithTag("Player");
		transform.position = theTarget.transform.position+exitOffset;
	}
}
