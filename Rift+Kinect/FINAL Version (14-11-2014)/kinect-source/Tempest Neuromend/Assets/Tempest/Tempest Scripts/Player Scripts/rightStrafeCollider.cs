using UnityEngine;
using System.Collections;

public class rightStrafeCollider : MonoBehaviour 
{
    public GameObject player;

    private leftStrafeCollider lStrafeCol;
    private bool strafteLeft;
    private bool strafeRight;

    private ObjectMoveAvoidance objMoveAvoid;

    private colliderControl colControl;

    private SphereCollider sphereCol;

    private float colX, colY, colZ;

        // Use this for initialization
    void Start()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.65f, 1.4f, 0.0f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.35f, 1.25f, 0.25f);
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
        lStrafeCol = GameObject.Find("leftStrafe").GetComponent<leftStrafeCollider>();
        strafteLeft = lStrafeCol.getStrafeLeft();
        strafeRight = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("rightStrafe").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (0.35 - 0.65)) / 100) + 0.65f;
        colY = (float)((colControl.sensitivity * (1.25 - 1.4)) / 100) + 1.4f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.0)) / 100) + 0.0f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;
    }

        // Update is called once per frame
    void Update()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.65f, 1.4f, 0.0f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.35f, 1.25f, 0.25f);

        lStrafeCol = GameObject.Find("leftStrafe").GetComponent<leftStrafeCollider>();
        strafteLeft = lStrafeCol.getStrafeLeft();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("rightStrafe").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (0.35 - 0.65)) / 100) + 0.65f;
        colY = (float)((colControl.sensitivity * (1.25 - 1.4)) / 100) + 1.4f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.0)) / 100) + 0.0f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;

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
