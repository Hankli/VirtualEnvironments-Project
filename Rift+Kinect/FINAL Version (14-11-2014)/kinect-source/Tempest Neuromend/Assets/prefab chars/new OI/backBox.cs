using UnityEngine;
using System.Collections;

public class backBox : MonoBehaviour {

    public bool collided;
    public float radius = 0;

    void Start()
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<hands>())
        {
            if (other.GetComponent<hands>().isHolding == true)
            {
                Debug.Log("Can now throw object");
                GameObject.Find("ForwardBox").GetComponent<forwardBox>().able = true;
            }
        }
        if (other.GetComponent<ThrowableObject>())
        {
            Debug.Log("FUCKERS");
        }
    }

}
