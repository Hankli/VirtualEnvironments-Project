using UnityEngine;
using System.Collections;

public class leftHandCollider : MonoBehaviour 
{
    public GameObject leftHand;

	    // Use this for initialization
	void Start () 
    {
        Physics.IgnoreCollision(GameObject.Find("Own Char").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("leftHand").GetComponent<CharacterController>(), GameObject.Find("Own Char").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("leftHand").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());

        transform.position = leftHand.transform.position;
        transform.rotation = leftHand.transform.rotation;
	}
	
	    // Update is called once per frame
	void Update () 
    {
        Physics.IgnoreCollision(GameObject.Find("Own Char").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("leftHand").GetComponent<CharacterController>(), GameObject.Find("Own Char").GetComponent<CharacterController>());
        Physics.IgnoreCollision(GameObject.Find("leftHand").GetComponent<CharacterController>(), GameObject.Find("OVRPlayerController").GetComponent<CharacterController>());

        transform.position = leftHand.transform.position;
        transform.rotation = leftHand.transform.rotation;
	}
}