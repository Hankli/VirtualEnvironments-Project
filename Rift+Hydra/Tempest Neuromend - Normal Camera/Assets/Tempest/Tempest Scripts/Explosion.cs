using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	private float m_destructionTimer=3.0f;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		m_destructionTimer-=1*Time.deltaTime;
		if (m_destructionTimer <= 0.0f) 
		{
			GameObject.Destroy (this.gameObject);
		}
	}
}
