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
	private bool b_noGravSpin=false;
	public WindowTrigger.WindowType windowType;

	//private bool b_isHeld=false;
	
	void Start() 
	{
		//NoGravSpin();
	}
	
	void Update() 
	{
	}

	/*
	private void IsHeld(bool val=true)
	{
		b_isHeld = val;
	}

	public void HoldThis(bool val=true)
	{
		Rigidbody rigidBodyComponent;
		if(val)
		{
			if(rigidBodyComponent=gameObject.GetComponent<Rigidbody>())
			{	
				rigidBodyComponent.freezeRotation=true;
				rigidBodyComponent.velocity=Vector3.zero;
			}
			IsHeld();
		}
		else
		{
			if(rigidBodyComponent=gameObject.GetComponent<Rigidbody>())
			{	
				rigidBodyComponent.freezeRotation=false;
			}
			IsHeld(false);
		}

	}
*/
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
		b_noGravSpin=true;
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

	void OnCollisionEnter(Collision collision)
	{
		if(b_noGravSpin)
		{
			Rigidbody rigidBodyComponent;
			if(rigidBodyComponent=gameObject.GetComponent<Rigidbody>())
			{	
				rigidBodyComponent.useGravity=true;
				//Vector3 rotationVector =Vector3.zero;
				//rigidBodyComponent.AddTorque(rotationVector);
				//rigidBodyComponent.freezeRotation=true;
			}
			b_noGravSpin=false;
		}
	}

}
