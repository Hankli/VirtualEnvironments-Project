using UnityEngine;
using System.Collections;

public class rightHandCollider : MonoBehaviour 
{
    public GameObject rightHand;

	    // Use this for initialization
	void Start () 
    {
        Physics.IgnoreCollision(GameObject.Find("Own Char").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("rightHand").GetComponent<CharacterController>(), GameObject.Find("Own Char").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("rightHand").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
        transform.position = rightHand.transform.position;
        transform.rotation = rightHand.transform.rotation;
	}
	
	    // Update is called once per frame
	void Update () 
    {
        Physics.IgnoreCollision(GameObject.Find("Own Char").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("rightHand").GetComponent<CharacterController>(), GameObject.Find("Own Char").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("rightHand").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());

        transform.position = rightHand.transform.position;
        transform.rotation = rightHand.transform.rotation;
	}
}