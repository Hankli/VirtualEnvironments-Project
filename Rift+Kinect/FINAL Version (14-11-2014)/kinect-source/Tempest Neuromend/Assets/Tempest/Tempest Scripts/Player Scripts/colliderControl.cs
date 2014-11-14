using UnityEngine;
using System.Collections;

public class colliderControl : MonoBehaviour 
{
    public float sensitivity = 0.0f;
    public float radius = 0.2f;

	    // Use this for initialization
	void Start () 
    {
        sensitivity = GameObject.Find("Game Control").GetComponent<GameControl>().inputSensitivity * 10;
	}
	
	    // Update is called once per frame
	void Update () 
    {

	}

    void OnMouseDown()
    {
        Application.CaptureScreenshot("Screen_Left.png");
    }
}