using UnityEngine;
using System.Collections;

public class PlayerClick : MonoBehaviour
{

	void Start(){
	
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 1.0f))
			{
				//Debug.DrawLine(ray.origin, hit.point);
				if(hit.collider!=null)
				{
					if(hit.transform.tag=="Button")
					{
						TouchPanelButton buttonScript = hit.transform.GetComponent<TouchPanelButton>();
						buttonScript.onClick();
					}
				}
			}
		}	
    }
}
