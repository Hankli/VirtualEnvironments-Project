using UnityEngine;
using System.Collections;

public class jumpLCollider : MonoBehaviour 
{
    public GameObject player;

    private jumpRCollider jumpRCol;
    private bool jumpR;
    private bool jumpL;

    private ObjectMoveAvoidance objMoveAvoid;

        // Use this for initialization
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.25f, 1.8f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        jumpRCol = GameObject.Find("jumpR").GetComponent<jumpRCollider>();
        jumpR = jumpRCol.getJumpR();
        jumpL = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
        objMoveAvoid.isJump = false;
    }

        // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(-0.25f, 1.8f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        jumpRCol = GameObject.Find("jumpR").GetComponent<jumpRCollider>();
        jumpR = jumpRCol.getJumpR();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
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
