using UnityEngine;
using System.Collections;

public class camControl : MonoBehaviour 
{
    public bool fpsCam;

    public Camera FPSCamera;
    public Camera OVRCamLeft;
    public Camera OVRCamRight;

	    // Use this for initialization
	void Start () 
    {
        FPSCamera = GameObject.Find("FPS Camera").GetComponent<Camera>();
        OVRCamLeft = GameObject.Find("CameraLeft").GetComponent<Camera>();
        OVRCamRight = GameObject.Find("CameraRight").GetComponent<Camera>();
        fpsCam = GameObject.Find("Game Control").GetComponent<GameControl>().b_OVRCam;
        if (fpsCam == true)
            fpsCam = false;
        else
            fpsCam = true;
	}
	
	    // Update is called once per frame
	void Update () 
    {
        if (fpsCam == true)
        {
            if(FPSCamera.enabled == false)
            {
                FPSCamera.enabled = true;
            }//end of if.

            if(OVRCamLeft.enabled == true)
            {
                OVRCamLeft.enabled = false;
            }//end of if.

            if (OVRCamRight.enabled == true)
            {
                OVRCamRight.enabled = false;
            }//end of if.
        }//end of if.
        else
        {
            if(FPSCamera.enabled == true)
            {
                FPSCamera.enabled = false;
            }//end of if.

            if(OVRCamLeft.enabled == false)
            {
                OVRCamLeft.enabled = true;
            }//end of if.

            if(OVRCamRight.enabled == false)
            {
                OVRCamRight.enabled = true;
            }//end of if.
        }//end of else.
	}
}