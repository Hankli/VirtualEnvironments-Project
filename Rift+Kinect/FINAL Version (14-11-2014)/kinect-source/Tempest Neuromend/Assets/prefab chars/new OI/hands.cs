using UnityEngine;
using System.Collections;

public class hands : MonoBehaviour {

    public bool isHolding = false;
    public ThrowableObject throwObj;
	// Use this for initialization
	void Start () 
    {
        Physics.IgnoreCollision(collider, GameObject.Find("TouchPanelMain01").collider);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isHolding == true)
        {
            throwObj.transform.position = transform.position;
            throwObj.rigidbody.freezeRotation.Equals(true);
            throwObj.rigidbody.velocity = Vector3.zero;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowableObject>())
        {
            //GameObject.FindGameObjectWithTag("LeftHand").GetComponent<hands>().throwObj = other.GetComponent<ThrowableObject>();
            //Debug.Log("HIT");
            if (throwObj != other.GetComponent<ThrowableObject>())
            {
                if (GameObject.FindGameObjectWithTag("LeftHand").GetComponent<hands>().isHolding == false && GameObject.FindGameObjectWithTag("RightHand").GetComponent<hands>().isHolding == false)
                {
                    throwObj = other.GetComponent<ThrowableObject>();
                    if (isHolding == false)
                    {
                        isHolding = true;
                        Physics.IgnoreCollision(other.collider, GameObject.Find("ForwardBox1").collider);
                        Physics.IgnoreCollision(other.collider, GameObject.Find("ForwardBox2").collider);
                        Physics.IgnoreCollision(other.collider, GameObject.Find("ForwardBox3").collider);
                        Physics.IgnoreCollision(other.collider, GameObject.Find("Own Char").collider);
                    }
                }
            }
        }
    }
}
