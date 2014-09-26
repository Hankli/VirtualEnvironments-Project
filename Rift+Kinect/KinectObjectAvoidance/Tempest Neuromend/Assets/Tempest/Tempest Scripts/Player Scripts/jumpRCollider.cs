using UnityEngine;
using System.Collections;

public class jumpRCollider : MonoBehaviour 
{
    public GameObject player;

    private jumpLCollider jumpLCol;
    private bool jumpL;
    private bool jumpR;

    private ObjectMoveAvoidance objMoveAvoid;

        // Use this for initialization
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.25f, 1.8f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        jumpLCol = GameObject.Find("jumpL").GetComponent<jumpLCollider>();
        jumpL = jumpLCol.getJumpL();
        jumpR = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
        objMoveAvoid.isJump = false;
    }

        // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.25f, 1.8f, 0.0f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        jumpLCol = GameObject.Find("jumpL").GetComponent<jumpLCollider>();
        jumpL = jumpLCol.getJumpL();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            jumpR = true;
        }//end of if.

        if((jumpR == true) && (jumpL == true))
        {
            //Debug.Log("Jumped.");
            objMoveAvoid.isJump = true;
        }//end of if.
    }

    void OnTriggerExit(Collider other)
    {
        if(other == GameObject.Find("rightHand").GetComponent<CharacterController>())
        {
            jumpR = false;

            objMoveAvoid.isJump = false;
        }//end of if.
    }

    public bool getJumpR()
    {
        return (jumpR);
    }
}
