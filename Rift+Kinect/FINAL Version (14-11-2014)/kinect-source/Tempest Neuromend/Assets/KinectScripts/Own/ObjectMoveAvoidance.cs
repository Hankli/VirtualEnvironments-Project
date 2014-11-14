using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectMoveAvoidance : MonoBehaviour
{

    public bool isMove = false;
    public bool isRotateLeft = false;
    public bool isRotateRight = false;
    public bool isCrouch = false;
    public bool isJump = false;
    public float speed = 1.0f;
    public float angle = 0.0f;
    private CharacterMotor motor;
    private CharacterController control;
    
        //Crouch
    private float finalHeight;
    private float initHeight;
    private Vector3 finalCenter;
    private Vector3 initCenter;

    private float time = 0;

    Vector3 translationAll = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        //get the listener for gestures
        //gestureListener = Camera.main.GetComponent<GestureListener>();
        //gestureListener = gameObj.GetComponent<GestureListener>();
        motor = GetComponent<CharacterMotor>();
        control = GetComponent<CharacterController>();
            //Crouch
        initHeight = control.height;
        finalHeight = initHeight * 0.5f;
        initCenter = control.center;
        finalCenter = new Vector3(0.0f, 0.45f, 0.0f);
        speed = GameObject.Find("Game Control").GetComponent<GameControl>().objectAvoidancePlayerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        motor.movement.maxForwardSpeed = 0;
        motor.movement.maxBackwardsSpeed = 0;

        time += Time.deltaTime;
        if (isMove == true)
        {
            translationAll = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 1.0f);//1.0f is constant 'forward' movement
            translationAll = transform.TransformDirection(translationAll);//direction from world to local
            control.SimpleMove(translationAll * speed);
            //transform.position += transform.forward * speed * Time.deltaTime;
            //Debug.Log("Moving");
        }
        if (isRotateRight == true)
        {
            //angle = Quaternion.Euler(0.0f, 1.0f, 0.0f);
            //transform.rotation.Set(transform.rotation.x, transform.rotation.y + (speed * Time.deltaTime), transform.rotation.z, transform.rotation.w);
            //transform.rotation.Set(transform.rotation.x, transform.rotation.y + 1.0f, transform.rotation.z, transform.rotation.w);

            translationAll = new Vector3(1.0f, 0.0f, 0.0f);//1.0f is constant 'right' movement
            translationAll = transform.TransformDirection(translationAll);//direction from world to local
            control.SimpleMove(translationAll * speed);
            //transform.position += new Vector3(1.0f, 0.0f, 0.0f);

            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            //transform.rotation = new Quaternion(0.0f, angle, 0.0f, 0.0f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);
            isRotateRight = false;
            Debug.Log("RIGHT");
        }
        else if (isRotateLeft == true)
        {
            //angle = Quaternion.Euler(0.0f, -1.0f, 0.0f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);

            translationAll = new Vector3(-1.0f, 0.0f, 0.0f);//1.0f is constant 'left' movement
            translationAll = transform.TransformDirection(translationAll);//direction from world to local
            control.SimpleMove(translationAll * speed);
            
            //transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Debug.Log("LEFT");
            isRotateLeft = false;
        }
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime);
        //transform.rotation = new Quaternion(0.0f, angle, 0.0f, 0.0f);
        if (isCrouch == true)
        {
            Debug.Log("CROUCH");
            control.center = finalCenter;
            control.height = finalHeight;
            //time = 0;

        }
        if (isJump == true)
        {
            Debug.Log("JUMP");
            //Debug.Log(motor.IsJumping());
            motor.inputJump = true;
            //isJump = false;
        }
        if (isJump == false)
        {
            motor.inputJump = false;
        }
        if (isCrouch == false/*time > 2 && isCrouch == true*/)
        {
            control.center = initCenter;
            control.height = initHeight;
            //isCrouch = false;
        }

    }
}
