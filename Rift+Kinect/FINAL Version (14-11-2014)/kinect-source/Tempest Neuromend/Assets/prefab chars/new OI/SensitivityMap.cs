using UnityEngine;
using System.Collections;

public class SensitivityMap : MonoBehaviour {

    GameObject lHand;
    GameObject rHand;
    Vector3 prevL, prevR; //previous pos
    Vector3 curL, curR; //current pos
    public float moveSen; //movement sensitivity
    public bool isMove = false;
    bool found = false;

	// Use this for initialization
	void Start () 
    {
        lHand = GameObject.FindGameObjectWithTag("LeftHand");
        rHand = GameObject.FindGameObjectWithTag("RightHand"); 
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isMove == true)
        {
            lHand.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
            isMove = false;
        }
        if (/*GameObject.FindGameObjectWithTag("KinectMan").GetComponent<KinectManager>().IsUserDetected() == true &&*/ found == false)
        {
            prevL = lHand.transform.position;
            prevR = rHand.transform.position;
            found = true;
        }
        if (lHand.transform.position != prevL)
        {
            lHand.transform.position += lHand.transform.position * 0.1f;
            prevL = lHand.transform.position;
        }
        if (rHand.transform.position != prevR)
        {
            rHand.transform.position += rHand.transform.position * 2;
            prevR = rHand.transform.position;
        }
        
	}
}
