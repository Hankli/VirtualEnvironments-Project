using UnityEngine;
using System.Collections;

public class crouchRCollider : MonoBehaviour 
{
    public GameObject player;

    private crouchLCollider crouchLCol;
    private bool crouchL;
    private bool crouchR;

    private ObjectMoveAvoidance objMoveAvoid;

    private colliderControl colControl;

    private SphereCollider sphereCol;

    private float colX, colY, colZ;

        // Use this for initialization
    void Start()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.20f, 0.9f, 0.15f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.15f, 0.95f, 0.25f);
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
        crouchLCol = GameObject.Find("crouchL").GetComponent<crouchLCollider>();
        crouchL = crouchLCol.getCrouchL();
        crouchR = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("crouchR").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (0.15 - 0.20)) / 100) + 0.20f;
        colY = (float)((colControl.sensitivity * (0.95 - 0.9)) / 100) + 0.9f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.15)) / 100) + 0.15f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;
    }

        // Update is called once per frame
    void Update()
    {
            //1(lowest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.20f, 0.9f, 0.15f);
            //10(highest) sensitivity.
        //transform.position = player.GetComponent<Transform>().position + new Vector3(0.15f, 0.95f, 0.25f);

        crouchLCol = GameObject.Find("crouchL").GetComponent<crouchLCollider>();
        crouchL = crouchLCol.getCrouchL();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

        colControl = GameObject.Find("Colliders").GetComponent<colliderControl>();

        sphereCol = GameObject.Find("crouchR").GetComponent<SphereCollider>();

        sphereCol.radius = colControl.radius;

        colX = (float)((colControl.sensitivity * (0.15 - 0.20)) / 100) + 0.20f;
        colY = (float)((colControl.sensitivity * (0.95 - 0.9)) / 100) + 0.9f;
        colZ = (float)((colControl.sensitivity * (0.25 - 0.15)) / 100) + 0.15f;

        transform.position = player.GetComponent<Transform>().position + new Vector3(colX, colY, colZ);
        transform.rotation = player.GetComponent<Transform>().rotation;

        if ((crouchR == true) && (crouchL == true))
        {
            //Debug.Log("Crouching...");
            objMoveAvoid.isCrouch = true;
        }//end of if.
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            crouchR = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            crouchR = false;

            objMoveAvoid.isCrouch = false;
        }//end of if.
    }

    public bool getCrouchR()
    {
            return(crouchR);
    }
}
