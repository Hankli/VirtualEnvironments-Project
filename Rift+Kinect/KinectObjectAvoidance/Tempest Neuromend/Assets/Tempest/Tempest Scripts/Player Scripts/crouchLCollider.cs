using UnityEngine;
using System.Collections;

public class crouchLCollider : MonoBehaviour 
{
    public GameObject player;

    private crouchRCollider crouchRCol;
    private bool crouchR;
    private bool crouchL;

    private ObjectMoveAvoidance objMoveAvoid;

        // Use this for initialization
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.20f, 0.9f, 0.15f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        crouchRCol = GameObject.Find("crouchR").GetComponent<crouchRCollider>();
        crouchR = crouchRCol.getCrouchR();
        crouchL = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
    }

        // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.25f, 0.9f, 0.4f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        crouchRCol = GameObject.Find("crouchR").GetComponent<crouchRCollider>();
        crouchR = crouchRCol.getCrouchR();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

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
