using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour 
{
	private bool b_scorable=true;
	private bool b_error=false;
	private bool b_noGravSpin=false;
	public WindowTrigger.WindowType windowType;

	private bool b_destroy=false;
	private float m_destructionTimer=2.0f;
	private GameObject m_explosion=null;

	void Awake()
	{
		m_explosion = Resources.Load<GameObject> ("Prefabs/Explosion01");
	}

	void Update() 
	{
		if(b_destroy)
		{
			m_destructionTimer-=1*Time.deltaTime;
		}
		if(m_destructionTimer<=0.0f)
		{
			//spawn particles and then destroy this object...
			Vector3 pos=transform.position;
			GameObject death;
			if(m_explosion)
			{
				death=Instantiate(m_explosion, pos, Quaternion.identity)as GameObject;
			}

			GameObject.Destroy(this.gameObject);
		}
			
	}

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


	public void SelfDestruct(float x=2.0f, bool destroy=true)
	{
		//need to activate timer and destroy after x seconds
		m_destructionTimer = x;
		b_destroy = destroy;
	}

	void OnCollisionEnter(Collision collision)
	{
		if(b_noGravSpin)
		{
			Rigidbody rigidBodyComponent;
			if(rigidBodyComponent=gameObject.GetComponent<Rigidbody>())
			{	
				rigidBodyComponent.useGravity=true;
			}
			b_noGravSpin=false;
		}
	}

}
