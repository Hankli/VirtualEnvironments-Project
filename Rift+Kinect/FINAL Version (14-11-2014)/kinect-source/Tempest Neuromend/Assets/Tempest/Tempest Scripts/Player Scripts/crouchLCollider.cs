using UnityEngine;
using System.Collections;

public class crouchLCollider : MonoBehaviour 
{
    public GameObject player;

    private crouchRCollider crouchRCol;
    private bool crouchR;
    private bool crouchL;

    private ObjectMoveAvoidance objMoveAvoid;

    private colliderControl colControl;

    private SphereCollider sphereCol;

    private float colX, colY, colZ;

        // Use this for initialization
    void Start()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.20f, 0.9f, 0.15f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.15f, 0.95f, 0.25f);
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
        crouchRCol = GameObject.Find("crouchR").GetComponent<crouchRCollider>();
        crouchR = crouchRCol.getCrouchR();
        crouchL = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("crouchL").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (-0.15 - -0.20)) / 100) + -0.20f;
        colY = (float)((colControl.sensitivity * (0.95 - 0.9)) / 100) + 0.9f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.15)) / 100) + 0.15f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;
    }

        // Update is called once per frame
    void Update()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.20f, 0.9f, 0.15f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(-0.15f, 0.95f, 0.25f);

        crouchRCol = GameObject.Find("crouchR").GetComponent<crouchRCollider>();
        crouchR = crouchRCol.getCrouchR();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("crouchL").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (-0.15 - -0.20)) / 100) + -0.20f;
        colY = (float)((colControl.sensitivity * (0.95 - 0.9)) / 100) + 0.9f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.15)) / 100) + 0.15f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;

        if ((crouchL == true) && (crouchR == true))
        {
            //Debug.Log("Crouching...");
            objMoveAvoid.isCrouch = true;
        }//end of if.
    }

    void OnTriggerEnter(Collider other)
    {
        if(other = GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            crouchL = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("leftHand").GetComponent<CharacterController>())
        {
            crouchL = false;

            objMoveAvoid.isCrouch = false;
        }//end of if.
    }

    public bool getCrouchL()
    {
        return (crouchL);
    }
}
