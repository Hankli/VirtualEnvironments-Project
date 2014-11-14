﻿using UnityEngine;
using System.Collections;

public class OVRCamMoveIn : MonoBehaviour {

    private GameObject eyes;
    public Transform pos;

    //public ObjectMove rotMap; //rotational mapping
    //private GameObject cam;
    // Use this for initialization

    void Awake()
    {
        //cam = GameObject.FindGameObjectWithTag("MainCamera");
        eyes = GameObject.FindGameObjectWithTag("eyePos");
        //transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);//eyes.transform.rotation;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        eyes = GameObject.FindGameObjectWithTag("eyePos");
        //transform.rotation = eyes.transform.rotation;
        //transform.Rotate(0.0f, 90.0f, 0.0f);
        transform.position = eyes.transform.position + new Vector3(0.0f, 0.5f, 0.05f);
        //transform.rotation.Set (transform.rotation.x, transform.rotation.y + 180.0f, transform.rotation.z, transform.rotation.w);
        //transform.rotation = Quaternion.AngleAxis(rotMap.angle, Vector3.up);
        //rotMap.angle = transform.rotation.y;
    }
}