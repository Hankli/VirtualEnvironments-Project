using UnityEngine;
using System.Collections;

public class forwardBox : MonoBehaviour {

    public bool collided;
    public float radius = 0;
    public bool able = false;

    void Start()
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<hands>())
        {
            if (other.GetComponent<hands>().isHolding == true)
            {
                //other.GetComponent<CharacterController>().enabled = false;
                //if (able == true)
               // {
                    //if (GameObject.FindGameObjectWithTag("Throwable").GetComponent<ThrowableObject>().holding == true)
                    //{
                    Debug.Log("ANOPAN IS A FAGGOT");
                    Physics.IgnoreCollision(other.collider, other.GetComponent<hands>().throwObj.collider);
                    //other.GetComponent<hands>().throwObj.throwObject = true;
                    //able = false;
                    other.GetComponent<hands>().isHolding = false;
                    //}
               // }
            }
        }
        if (other.GetComponent<ThrowableObject>())
        {
            Debug.Log("FUCKERS");
        }
    }

}
