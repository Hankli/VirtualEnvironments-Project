using UnityEngine;
using System.Collections;

public class PlayerOAControl : MonoBehaviour 
{
	Vector3 translationAll = Vector3.zero;
	CharacterController control;
	public float speed = 3.0f;
	
	void Start() 
	{
		control = GetComponent<CharacterController>();
	}
	
	void Update() 
	{
		translationAll = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 1.0f);//1.0f is constant 'forward' movement
		translationAll = transform.TransformDirection(translationAll);//direction from world to local
		control.SimpleMove(translationAll*speed);
	}
}
