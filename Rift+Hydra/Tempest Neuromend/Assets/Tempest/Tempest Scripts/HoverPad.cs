using UnityEngine;
using System.Collections;

public class HoverPad : MonoBehaviour 
{
    public float hoverForce = 0.5f;
    public float gravity = 20.0f;

	void Start()
	{
	}
	
	void Update() 
	{
	}
	
    //void OnTriggerStay(Collider other)
	void OnTriggerStay(Collider other)
    {
		if(other.GetComponent<CharacterController>())
		{
			CharacterController controller = other.GetComponent<CharacterController>();
			Vector3 hoverForceV = new Vector3(Input.GetAxis("Horizontal"),hoverForce,Input.GetAxis("Vertical"));
			hoverForceV=transform.TransformDirection(hoverForceV);
			hoverForceV.y -= gravity * Time.deltaTime / 2.0f;
			controller.Move(hoverForceV * Time.deltaTime);
			//Debug.Log(hoverForceV);
		}
		//CharacterController controller = other.FindComponent<CharacterController>();
		//Debug.Log(other.CharacterController);
		//Debug.Log(other.attachedRigidbody);
		//if (other.attachedRigidbody)
			//other.attachedRigidbody.AddForce(Vector3.up * hoverForce, ForceMode.Acceleration);
		//transform tr = other.transform;
		//if(other.CharacterController)
		//{
			//Debug.Log(other.CharacterController);

		//}
		
		//other.transform.position += new Vector3(0,hoverForce,0)*Time.deltaTime;
    }
}
