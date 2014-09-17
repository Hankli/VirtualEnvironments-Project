using UnityEngine;
using System.Collections;
namespace Tempest
{
	namespace RazorHydra
	{
		public class HandsMotionController : MonoBehaviour 
		{
			private Hand[] m_hands;
			private Vector3	m_referencePoint;
			private bool m_bInitialized;
			private float m_sensitivity = 0.001f; // Sixense units are in mm
			
			// Use this for initialization
			protected void Start () 
			{
				m_hands = GetComponentsInChildren<Hand>();
			}
			
			// Update is called once per frame
			protected void FixedUpdate () 
			{
				bool bResetHandPosition = false;
				
				foreach ( Hand hand in m_hands )
				{
					//active controller and start button is pushed
					if ( IsControllerActive( hand.Controller ) &&
					     HandInputController.CalibrationState == ControllerManagerState.NONE &&
					     hand.Controller.GetButtonDown( Buttons.START ) )
					{
						bResetHandPosition = true;
					}
					
					if ( m_bInitialized )
					{
						UpdateHandMotion( hand );
					}
				}
				
				if ( bResetHandPosition )
				{
					m_bInitialized = true;
					
					m_referencePoint = Vector3.zero;
					
					// Get the base offset assuming forward facing down the z axis of the base
					foreach ( Hand hand in m_hands )
					{
						m_referencePoint += hand.Controller.Position;
					}
					
					m_referencePoint *= 0.5f; //midway point serve as a reference point for each hand motion
				}
			}
			
			protected void UpdateHandMotion( Hand hand )
			{
				bool bControllerActive = IsControllerActive( hand.Controller );
				
				if ( bControllerActive )
				{
					Rigidbody rb = hand.rigidbody;
					Transform tr = hand.transform;
					
					//localPosition
					Vector3 relPosToPar = (hand.Controller.Position - m_referencePoint) * m_sensitivity; 
					
					//localPosition to worldPosition
					Vector3 desiredPos = tr.parent.TransformPoint(relPosToPar) ; 
					
					//directional vector
					Vector3 v = desiredPos - tr.position;

					//force
					float f = (v.magnitude / Time.deltaTime) * Time.timeScale;

					//apply force in place of previous value
					rb.AddForce(f * v.normalized - rb.velocity, ForceMode.VelocityChange);



					//handle rotational movement with torque
					Quaternion relRotToPar = hand.Controller.Rotation * hand.ModelRotation; //localRotation
					Quaternion desiredRot = tr.parent.rotation * relRotToPar;  //localRotation to worldRotation
					
					//calc axis of rotation between desired rotation's (xyz) axis and current rotation's (xyz) axis
					Vector3 sCrossE_Z = Vector3.Cross (tr.forward, desiredRot * Vector3.forward);
					Vector3 sCrossE_X = Vector3.Cross (tr.right, desiredRot * Vector3.right);
					Vector3 sCrossE_Y = Vector3.Cross (tr.up, desiredRot * Vector3.up);
					
					//get angular acceleration about x,y and z axis
					float aZ = sCrossE_Z.magnitude;
					float aX = sCrossE_X.magnitude;
					float aY = sCrossE_Y.magnitude;
					
					float thetaThresh = 1.0f - Mathf.Epsilon;
					float halfPI = Mathf.PI / 2.0f;
					
					aZ = aZ >= thetaThresh ? halfPI : aZ <= -thetaThresh ? -halfPI : Mathf.Asin(aZ);
					aX = aX >= thetaThresh ? halfPI : aX <= -thetaThresh ? -halfPI : Mathf.Asin(aX);
					aY = aY >= thetaThresh ? halfPI : aY <= -thetaThresh ? -halfPI : Mathf.Asin(aY);
					
					//get desired change in angular velocity about x,y and z axis
					Vector3 wZ = sCrossE_Z.normalized * (aZ / Time.deltaTime);
					Vector3 wX = sCrossE_X.normalized * (aX / Time.deltaTime);
					Vector3 wY = sCrossE_Y.normalized * (aY / Time.deltaTime);

					//bring hand's rotation to local coordinates of the inertia tensor
					Quaternion q = tr.rotation * rb.inertiaTensorRotation;
					
					Vector3 w = wX + wZ + wY; 

					//calculate the needed torque needed to get to desired rotation in one frame
					Vector3 t = q * Vector3.Scale (rb.inertiaTensor / rb.mass, Quaternion.Inverse(q) * w) * Time.timeScale;

					rb.AddTorque(t, ForceMode.Impulse);	
					rb.maxAngularVelocity = t.magnitude / Time.fixedDeltaTime;
					
				}
				else
				{
					// use the inital position and orientation because the controller is not active
					hand.transform.localPosition = hand.ModelPosition;
					hand.transform.localRotation  = hand.ModelRotation;
				}
			}
			
			protected void OnGUI()
			{
				if ( !m_bInitialized )
				{
					GUI.Box( new Rect( Screen.width * 0.5f - 50, Screen.height - 40, 100, 30 ),  "Press Start" );
				}
			}
			
			
			/** returns true if a controller is enabled and not docked */
			protected bool IsControllerActive( HandInput controller )
			{
				return ( controller != null &&
				        controller.Enabled &&
				        !controller.Docked );
			}
		}
	}
}