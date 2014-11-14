using UnityEngine;
using System.Collections;

public class jumpLCollider : MonoBehaviour 
{
    public GameObject player;

    private jumpRCollider jumpRCol;
    private bool jumpR;
    private bool jumpL;

    private ObjectMoveAvoidance objMoveAvoid;

    private colliderControl colControl;

    private SphereCollider sphereCol;

    private float colX, colY, colZ;

        // Use this for initialization
    void Start()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.25f, 1.8f, 0.0f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.15f, 1.55f, 0.25f);

        //Formula
        //  (sensitiivty * diff(x, y, z)) / 100 = value
        // Add value to current position.
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
        jumpRCol = GameObject.Find("jumpR").GetComponent<jumpRCollider>();
        jumpR = jumpRCol.getJumpR();
        jumpL = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
        objMoveAvoid.isJump = false;

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("jumpL").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (-0.15 - -0.25)) / 100) + -0.25f;
        colY = (float)((colControl.sensitivity * (1.55 - 1.8)) / 100) + 1.8f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.0)) / 100) + 0.0f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;
    }

        // Update is called once per frame
    void Update()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.25f, 1.8f, 0.0f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.15f, 1.55f, 0.25f);

        jumpRCol = GameObject.Find("jumpR").GetComponent<jumpRCollider>();
        jumpR = jumpRCol.getJumpR();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("jumpL").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (-0.15 - -0.25)) / 100) + -0.25f;
        colY = (float)((colControl.sensitivity * (1.55 - 1.8)) / 100) + 1.8f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.0)) / 100) + 0.0f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            jumpL = true;
        }//end of if.

        if((jumpR == true) && (jumpL == true))
        {
            //Debug.Log("Jumped.");
            objMoveAvoid.isJump = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            jumpL = false;

            objMoveAvoid.isJump = false;
        }//end of if.
    }

    public bool getJumpL()
    {
        return (jumpL);
    }
}
