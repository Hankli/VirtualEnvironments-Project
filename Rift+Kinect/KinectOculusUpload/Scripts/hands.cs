using UnityEngine;
using System.Collections;

public class hands : MonoBehaviour {

    public bool isHolding = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ThrowableObject>())
        {
            Debug.Log("HIT");
            ThrowableObject throwObj = other.GetComponent<ThrowableObject>();
            throwObj.transPos = gameObject;
            throwObj.doTrans = true;
            isHolding = true;
        }
    }
}
