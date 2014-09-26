using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour 
{
	/*
	public enum ThrowableObjectType
	{
		Sphere,//circle
		Cube,//square
		Tetrahedron//triangle
	};
	public ThrowableObjectType objectType;
	*/
	
	public WindowTrigger.WindowType windowType;
	
	void Start() 
	{
	}
	
	void Update() 
	{
	}
	
	/*
	public ThrowableObjectType GetObjectType()
	{
		return objectType;
	}
	*/
	public WindowTrigger.WindowType GetWindowType()
	{
		return windowType;
	}

}
