using UnityEngine;
using System.Collections;

public class controlSen : MonoBehaviour {

    public float sensitivity = 0;
    Vector3 pos1L; //no sensitivity
    Vector3 pos2L; //50%
    Vector3 pos3L; //100%
    Vector3 pos1R; //no sensitivity
    Vector3 pos2R; //50%
    Vector3 pos3R; //100%
	// Use this for initialization
	void Start () 
    {
        sensitivity = GameObject.Find("Game Control").GetComponent<GameControl>().inputSensitivity * 10;
	    //setup our sensitivity
        //L
        //1
        float yy = GameObject.Find("GLeftTurn").transform.position.y;
        pos1L.x = -0.6189378f;
        pos1L.y = yy;
        pos1L.z = 0.01466238f;
        //2
        pos2L.x = -0.386758f;
        pos2L.y = yy - 0.1f;
        pos2L.z = 0.2959175f;
        //3
        pos3L.x = -0.189607f;
        pos3L.y = yy - 0.1f;
        pos3L.z = 0.3632382f;
        //R
        //1
        pos1R.x = 0.6716395f;
        pos1R.y = yy;
        pos1R.z = 0.01466238f;
        //2
        pos2R.x = 0.402079f;
        pos2R.y = yy - 0.1f;
        pos2R.z = 0.3241357f;
        //3
        pos3R.x = 0.2524447f;
        pos3R.y = yy - 0.1f;
        pos3R.z = 0.3672076f;

        if (sensitivity <= 25)
        {
            GameObject.Find("GLeftTurn").transform.position = pos1L;
            GameObject.Find("GRightTurn").transform.position = pos1R;
        }
        else if (sensitivity >= 70)
        {
            GameObject.Find("GLeftTurn").transform.position = pos3L;
            GameObject.Find("GRightTurn").transform.position = pos3R;
        }
        else
        {
            GameObject.Find("GLeftTurn").transform.position = pos2L;
            GameObject.Find("GRightTurn").transform.position = pos2R;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
