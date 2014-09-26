using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour 
{
	/*
	public enum ThrowableObjectType
	{
		Sphere,//circle
		Cube,//square
		Tetrahedron//triangle
	};
	public ThrowableObjectType objectType;
	*/
    public GameObject transPos;
    public bool doTrans = false;
	
	public WindowTrigger.WindowType windowType;
	
	void Start() 
	{
        //
	}
	
	void Update() 
	{
        if (doTrans == true)
        {
            //transform.position = transPos.transform.position;
            ThrowObj();
        }
	}
	
	/*
	public ThrowableObjectType GetObjectType()
	{
		return objectType;
	}
	*/
	public WindowTrigger.WindowType GetWindowType()
	{
		return windowType;
	}

    public void ThrowObj()
    {
        Vector3 p = new Vector3(0, 0, 0);
        Transform throwDir = transform;
        collider.attachedRigidbody.useGravity = true;
        GameObject lHand = GameObject.FindGameObjectWithTag("LeftHand");
        GameObject rHand = GameObject.FindGameObjectWithTag("RightHand");
        if (lHand.GetComponent<hands>().isHolding == true)
        {
            p = lHand.transform.position;
            lHand.GetComponent<hands>().isHolding = false;
        }
        else if (rHand.GetComponent<hands>().isHolding == true)
        {
            p = rHand.transform.position;
            rHand.GetComponent<hands>().isHolding = false;
        }
        Ray ray3 = Camera.main.ScreenPointToRay(p);
        collider.attachedRigidbody.AddForce(ray3.direction * 10, ForceMode.Impulse);
        doTrans = false;
    }
}
