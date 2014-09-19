using UnityEngine;
using System.Collections;

public class WindowTrigger : MonoBehaviour 
{

	public enum WindowType
	{
		Circle,
		Square,
		Triangle
	};

	public WindowType windowType;
	private ThrowingObjective objectiveScript=null;
	
	void Awake() 
	{
		if(transform.parent.GetComponent<ThrowingObjective>())
		{
			objectiveScript = transform.parent.GetComponent<ThrowingObjective>();
		}
	}
	
	void Start() 
	{
	}
	
	void Update() 
	{
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<ThrowableObject>())
		{
			
			ThrowableObject thrownObjectScript=other.gameObject.GetComponent<ThrowableObject>();
			if(thrownObjectScript.GetWindowType()==windowType)
			{
				objectiveScript.AdjustScore(1);
			}
			else
			{
				objectiveScript.AdjustErrors(1);
			}
		}
	}
}
