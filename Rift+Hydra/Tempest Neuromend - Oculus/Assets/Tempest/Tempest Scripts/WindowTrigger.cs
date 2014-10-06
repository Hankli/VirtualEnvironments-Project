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
		ThrowableObject thrownObjectScript;
		if(thrownObjectScript=other.gameObject.GetComponent<ThrowableObject>())
		{
				//ThrowableObject thrownObjectScript=other.gameObject.GetComponent<ThrowableObject>();
				if(thrownObjectScript.GetWindowType()==windowType)
				{
					if(thrownObjectScript.Scorable())
					{
						objectiveScript.AdjustScore(1);
						thrownObjectScript.Scorable(false);
						//thrownObjectScript.Scored();
						//thrownObjectScript.SelfDestruct();
					}	
				}
				else
				{
					if(thrownObjectScript.Mistake())
					{
						objectiveScript.AdjustErrors(1);
						thrownObjectScript.Mistake(false);
					}
				}
		}
	}
}
