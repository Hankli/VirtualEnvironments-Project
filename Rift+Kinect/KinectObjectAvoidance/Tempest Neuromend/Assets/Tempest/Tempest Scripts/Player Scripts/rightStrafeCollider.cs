using UnityEngine;
using System.Collections;

public class rightStrafeCollider : MonoBehaviour 
{
    public GameObject player;

    private leftStrafeCollider lStrafeCol;
    private bool strafteLeft;
    private bool strafeRight;

    private ObjectMoveAvoidance objMoveAvoid;

        // Use this for initialization
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.65f, 1.4f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        lStrafeCol = GameObject.Find("leftStrafe").GetComponent<leftStrafeCollider>();
        strafteLeft = lStrafeCol.getStrafeLeft();
        strafeRight = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
    }

        // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.65f, 1.4f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        lStrafeCol = GameObject.Find("leftStrafe").GetComponent<leftStrafeCollider>();
        strafteLeft = lStrafeCol.getStrafeLeft();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        if((strafeRight == true) && (strafteLeft == false))
        {
            //Debug.Log("Strafing Right...");
            objMoveAvoid.isRotateRight = true;
        }//end of if.
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            strafeRight = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            strafeRight = false;

            objMoveAvoid.isRotateRight = false;
        }//end of if.
    }

    public bool getStrafeRight()
    {
        return(strafeRight);
    }
}
