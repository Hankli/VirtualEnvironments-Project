using UnityEngine;
using System.Collections;

public class TouchPanelIgnore : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Physics.IgnoreCollision(collider, GameObject.Find("joint_HandLT").collider);
        Physics.IgnoreCollision(collider, GameObject.Find("joint_HandRT").collider);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
