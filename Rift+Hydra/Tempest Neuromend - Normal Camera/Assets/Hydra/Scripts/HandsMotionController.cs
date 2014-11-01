using UnityEngine;
using System.Collections;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HandsMotionController : VRGUI 
		{
			private Hand[] m_hands;
			private Vector3	m_referencePoint;
			private bool m_bInitialized;
			private float m_linearSensitivity = 1.0f; // Sixense units are in mm
			private float m_angularSensitivity = 2.0f;
			public GUIStyle m_messageStyle = new GUIStyle();

			public float LinearSensitivity
			{
				get { return m_linearSensitivity; }
				set { m_linearSensitivity = value; }
			}

			public float AngularSensitivity
			{
				get { return m_angularSensitivity; }
				set { m_angularSensitivity = value; }
			}

			// Use this for initialization
			private void Start () 
			{
				m_hands = GetComponentsInChildren<Hand>();
			}


			// Update is called once per frame
			private void FixedUpdate () 
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
						hand.Controller.Smoother.ClearSamples();

						hand.Position = hand.Controller.PositionRaw;
						hand.Rotation = hand.Controller.RotationRaw;

						m_referencePoint += hand.Controller.PositionRaw;
					}
					
					m_referencePoint *= 0.5f; //midway point serve as a reference point for each hand motion
				}
			}
			
			private void UpdateHandMotion( Hand hand )
			{
				bool bControllerActive = IsControllerActive( hand.Controller );
				
				if ( bControllerActive )
				{
					Rigidbody rb = hand.rigidbody;
					Transform tr = hand.transform;

					if(m_linearSensitivity != 0.0f)
					{
						hand.Position += (hand.Controller.Position - hand.Controller.LastPosition) * m_linearSensitivity;
					    
					}
					else
					{
						hand.Position = hand.Controller.PositionRaw;
					}

		
					//localPosition
					Vector3 relPosToPar = (hand.Position - m_referencePoint) * 0.001f;

					//localPosition to worldPosition
					Vector3 desiredPos = tr.parent.TransformDirection(relPosToPar) + tr.parent.position; 

					//directional vector
					Vector3 v = (desiredPos - tr.position);


					//force
					float f = (v.magnitude / Time.deltaTime) * Time.timeScale;
			
					//apply force in place of previous value
					rb.AddForce(f * v.normalized - rb.velocity, ForceMode.VelocityChange);

					if(m_angularSensitivity != 0.0f)
					{
						Quaternion dr =  Quaternion.Inverse(hand.Controller.LastRotation) * hand.Controller.Rotation;
//						float angle;
//						Vector3 axis;
//						dr.ToAngleAxis(out angle, out axis);
//						angle *= m_angularSensitivity;
//
						hand.Rotation *= Quaternion.Euler(dr.eulerAngles * m_angularSensitivity);
					}
					else 
					{
						hand.Rotation = hand.Controller.Rotation;
					}

					//get desired orientation
					Quaternion desiredRot = tr.parent.rotation * (hand.Rotation * hand.ModelRotation);  //localRotation to worldRotation

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
			
			public override void OnVRGUI()
			{
				if ( !m_bInitialized && (HandInputController.ConfigurationState == ControllerManagerState.NONE))
				{
					uint boxWidth = 500;
					uint boxHeight = 550;

					string boxText = "Press start";
					
					GUI.Box( new Rect( (Screen.width / 2) - ( boxWidth / 2), 
					                    (Screen.height / 2) - ( boxHeight / 2),
					                    boxWidth, boxHeight), boxText, m_messageStyle);
		

				}
			}

			private Texture2D MakeTex( int width, int height, Color col )
			{
				Color[] pix = new Color[width * height];
				for( int i = 0; i < pix.Length; ++i )
				{
					pix[ i ] = col;
				}
				Texture2D result = new Texture2D( width, height );
				result.SetPixels( pix );
				result.Apply();
				return result;
			}

			
			
			/** returns true if a controller is enabled and not docked */
			private bool IsControllerActive( HandInput controller )
			{
				return ( controller != null &&
				        controller.Enabled &&
				        !controller.Docked );
			}
		}
	}
}