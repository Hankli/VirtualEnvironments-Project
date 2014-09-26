using UnityEngine;
using System.Collections;

public class leftStrafeCollider : MonoBehaviour 
{
    public GameObject player;

    private rightStrafeCollider rStrafeCol;
    private bool strafeRight;
    private bool straftLeft;

    private ObjectMoveAvoidance objMoveAvoid;

	    // Use this for initialization
	void Start () 
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.65f, 1.4f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        rStrafeCol = GameObject.Find("rightStrafe").GetComponent<rightStrafeCollider>();
        strafeRight = rStrafeCol.getStrafeRight();
        straftLeft = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
	}
	
	    // Update is called once per frame
	void Update () 
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.65f, 1.4f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        rStrafeCol = GameObject.Find("rightStrafe").GetComponent<rightStrafeCollider>();
        strafeRight = rStrafeCol.getStrafeRight();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        if((straftLeft == true) && (strafeRight == false))
        {
            //Debug.Log("Strafing Left...");
            objMoveAvoid.isRotateLeft = true;
        }//end of if.
	}

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            straftLeft = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            straftLeft = false;

            objMoveAvoid.isRotateLeft = false;
        }//end of if.
    }

    public bool getStrafeLeft()
    {
        return (straftLeft);
    }
}
