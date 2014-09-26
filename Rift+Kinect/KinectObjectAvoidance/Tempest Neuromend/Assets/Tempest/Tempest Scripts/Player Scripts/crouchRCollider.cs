using UnityEngine;
using System.Collections;

public class crouchRCollider : MonoBehaviour 
{
    public GameObject player;

    private crouchLCollider crouchLCol;
    private bool crouchL;
    private bool crouchR;

    private ObjectMoveAvoidance objMoveAvoid;

        // Use this for initialization
    void Start()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.20f, 0.9f, 0.15f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        crouchLCol = GameObject.Find("crouchL").GetComponent<crouchLCollider>();
        crouchL = crouchLCol.getCrouchL();
        crouchR = false;

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();
    }

        // Update is called once per frame
    void Update()
    {
        transform.position = player.GetComponent<Transform>().position + new Vector3(0.25f, 0.9f, 0.4f);
        transform.rotation = player.GetComponent<Transform>().rotation;

        crouchLCol = GameObject.Find("crouchL").GetComponent<crouchLCollider>();
        crouchL = crouchLCol.getCrouchL();

        objMoveAvoid = GameObject.Find("Own Char").GetComponent<ObjectMoveAvoidance>();

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
