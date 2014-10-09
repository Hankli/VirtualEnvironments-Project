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
			private float m_linearSensitivity = 1.0f; // Sixense units are in mm
			private float m_rotationSensitivity = 1.0f;

			public float MoveSensitivity
			{
				get { return m_linearSensitivity; }
				set { m_linearSensitivity = value; }
			}

			public float RotateSensitivity
			{
				get { return m_rotationSensitivity; }
				set { m_rotationSensitivity = value; }
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
						m_referencePoint += hand.Controller.Position;
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
					
					//localPosition
					Vector3 relPosToPar = (hand.Controller.Position - m_referencePoint) * (m_linearSensitivity / 1000.0f); 
				
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

					//amplify rotation 
					float angle;
					Vector3 axis;
					relRotToPar.ToAngleAxis(out angle, out axis);
					angle *= m_rotationSensitivity; 
					relRotToPar = Quaternion.AngleAxis(angle, axis);

					//get desired rotation
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
			
			private void OnGUI()
			{
				if ( !m_bInitialized && (HandInputController.ConfigurationState == ControllerManagerState.NONE))
				{
					GUIStyle style = new GUIStyle(GUI.skin.button);
					Font myFont = (Font)Resources.Load("linowrite", typeof(Font));
					//foreach(Font f in Resources.FindObjectsOfTypeAll<Font>()) Debug.Log(f.name);

					uint boxWidth = 150;
					uint boxHeight = 40;

					style.font = myFont;
					style.alignment = TextAnchor.MiddleCenter;
					style.fontStyle = FontStyle.Normal;
					style.normal.textColor = Color.Lerp(Color.red, Color.green, 0.90f);
					style.fontSize = 20;
		
					string boxText = "Press start";
					
					GUILayout.BeginArea( new Rect(( ( Screen.width / 2 ) - ( boxWidth / 2 ) ), 
					                              ( ( Screen.height / 2 ) - ( boxHeight / 2 ) ),
					                              boxWidth, boxHeight), "");
					
					GUILayout.Label(boxText, style);
					
					GUILayout.EndArea();

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