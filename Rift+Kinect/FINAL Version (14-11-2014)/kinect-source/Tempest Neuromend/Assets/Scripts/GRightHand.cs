﻿using UnityEngine;
using System.Collections;

public class GRightHand : MonoBehaviour
{
    public bool collided;
    Vector3 col;
    float sen;

    void Start()
    {
        collided = false;
        sen = GameObject.Find("Own Char").GetComponent<controlSen>().sensitivity;
        Physics.IgnoreCollision(collider, GameObject.Find("Own Char").collider);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("KinectMan").GetComponent<KinectManager>().IsUserDetected() == true)
        {
            if (GameObject.Find("GLeftTurn").GetComponent<GLeftHand>().collided == false)
                rightGesture();
        }

        //col.x = (float)((sen * (newPos.x - origin.x)) / 100) + origin.x;
        //col.y = (float)((sen * (newPos.y - origin.y)) / 100) + origin.y;
        //col.z = (float)((sen * (newPos.z - origin.z)) / 100) + origin.z;
        //transform.position = GameObject.Find("Own Char").transform.position;// + origin;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Right Hit");
        if (other.GetComponent<GHand>())
        {
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Right out");
        if (other.GetComponent<GHand>())
        {
            collided = false;
        }
    }

    private void rightGesture()
    {
        if (collided == true)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectMove>().isRotateRight = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectMove>().isRotateRight = false;
        }
    }
}