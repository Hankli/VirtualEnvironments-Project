using UnityEngine;
using System.Collections;

public class PlayerClick : MonoBehaviour
{
	private bool b_isHolding=false;
	private Transform heldObject;
	private float holdDistance=1.0f;
	
	void Start()
	{
	}
	
	void Update()
	{
		//need to make this more universal...
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 2.0f))
			{
				Debug.DrawLine(ray.origin, hit.point);
				if(hit.collider!=null)
				{
					if(hit.collider.attachedRigidbody)
						hit.collider.attachedRigidbody.useGravity = false;
					
					if(hit.transform.GetComponent<TouchPanelButton>())
					{
						TouchPanelButton buttonScript = hit.transform.GetComponent<TouchPanelButton>();
						buttonScript.OnClick();
					}
					else if(hit.transform.tag=="Throwable")
					{
						heldObject=hit.transform;
						b_isHolding=true;
					}
				}
			}
		}
		
		//hold object
		if(Input.GetMouseButton(0))
		{
			if(heldObject!=null)
			{
				Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
				heldObject.position = ray2.GetPoint(holdDistance);//still need to account for object x object x environment collisions while being held... ie. object needs to not go through other objects while being held
				b_isHolding=true;
			}
		}
		
		//'throw' object
		if(Input.GetMouseButton(1))
		{
			if(heldObject!=null)
			{
				if(heldObject.collider.attachedRigidbody)
				{
					heldObject.collider.attachedRigidbody.useGravity = true;
					Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
					heldObject.collider.attachedRigidbody.AddForce(ray3.direction*10, ForceMode.Impulse);
				}
				heldObject=null;
				b_isHolding=false;
			}
		}
		
		//release/drop object
		if(Input.GetMouseButtonUp(0))
		{
			if(heldObject!=null)
			{
				if(heldObject.collider.attachedRigidbody)
					heldObject.collider.attachedRigidbody.useGravity = true;
				heldObject=null;
				b_isHolding=false;
			}
		}
    }
    
    public bool IsHoldingObject()
    {
		return b_isHolding;
    }
}
