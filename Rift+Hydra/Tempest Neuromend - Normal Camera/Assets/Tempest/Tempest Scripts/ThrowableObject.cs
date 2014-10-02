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
	private bool b_scorable=true;
	private bool b_error=false;
	public WindowTrigger.WindowType windowType;
	
	void Start() 
	{
		//NoGravSpin();
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
	
	public void NoGravSpin(bool b_spin=true)
	{
		Rigidbody rigidBodyComponent;
		if(rigidBodyComponent=gameObject.GetComponent<Rigidbody>())
		{	
			rigidBodyComponent.useGravity=false;
			if(b_spin)
			{
				Vector3 rotationVector = new Vector3(1.0f,1.0f,0.0f);
				rigidBodyComponent.AddTorque(rotationVector);
			}
			else
			{
				rigidBodyComponent.velocity=Vector3.zero;
			}
		}
	}


	public bool Scorable()
	{
		return b_scorable;
	}
	
	public void Scorable(bool b_canScore)
	{
		b_scorable=b_canScore;
	}
	
	public bool Mistake()
	{
		return b_error;
	}
	
	public void Mistake(bool b_canMistake)
	{
		b_error=b_canMistake;
	}
	/*
	public void Scored()
	{
		NoGravSpin(false);
	}
	*/
	public void SelfDestruct(float x=2.0f)
	{
		//need to activate timer and destroy after x seconds
	}
}
