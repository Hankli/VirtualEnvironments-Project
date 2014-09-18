using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectMove : MonoBehaviour {

    public bool isMove = false;
    public bool isRotateLeft = false;
    public bool isRotateRight = false;
    public bool isCrouch = false;
    public bool isJump = false;
    public float speed = 1.0f;
    public float angle = 0.0f;
    private CharacterMotor motor;
    private CharacterController control;
    private GameObject cameraMove;
    private float camAngle;
    private float finalHeight;
    private float initHeight;
    private float time = 0;
	// Use this for initialization
	void Start () 
    {
        //get the listener for gestures
        //gestureListener = Camera.main.GetComponent<GestureListener>();
        //gestureListener = gameObj.GetComponent<GestureListener>();
        motor = GetComponent<CharacterMotor>();
        control = GetComponent<CharacterController>();
        initHeight = control.height;
        finalHeight = initHeight * 0.5f;
        cameraMove = GameObject.FindGameObjectWithTag("MainCamera");
    }
	
	// Update is called once per frame
	void Update () 
    {
        time += Time.deltaTime;
        if (isMove == true)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            //Debug.Log("Moving");
        }
        if (isRotateRight == true)
        {
            //angle = Quaternion.Euler(0.0f, 1.0f, 0.0f);
            //transform.rotation.Set(transform.rotation.x, transform.rotation.y + (speed * Time.deltaTime), transform.rotation.z, transform.rotation.w);
            //transform.rotation.Set(transform.rotation.x, transform.rotation.y + 1.0f, transform.rotation.z, transform.rotation.w);
            
            angle += 90;
            camAngle = cameraMove.transform.rotation.y;
            camAngle += angle;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            cameraMove.transform.rotation = Quaternion.AngleAxis(camAngle, Vector3.up);
            //transform.rotation = new Quaternion(0.0f, angle, 0.0f, 0.0f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);
            isRotateRight = false;
            Debug.Log("RIGHT");
        }
        else if (isRotateLeft == true)
        {
            //angle = Quaternion.Euler(0.0f, -1.0f, 0.0f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);
            angle -= 90;
            camAngle = cameraMove.transform.rotation.y;
            camAngle -= angle;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            cameraMove.transform.rotation = Quaternion.AngleAxis(camAngle, Vector3.up);
            Debug.Log("LEFT");
            isRotateLeft = false;
        }
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);
        //transform.rotation = new Quaternion(0.0f, angle, 0.0f, 0.0f);
        if (isCrouch == true)
        {
            control.height = finalHeight;
            time = 0;
            
        }
        if (isJump == true)
        {
            motor.IsJumping();
            isJump = false;
        }
        if (time > 2 && isCrouch == true)
        {
            control.height = initHeight;
            isCrouch = false;
        }

    }
}
