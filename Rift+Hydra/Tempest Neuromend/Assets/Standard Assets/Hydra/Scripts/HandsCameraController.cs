using UnityEngine;
using UnityEditor;

namespace Tempest
{
	namespace RazorHydra
	{
		[CustomEditor(typeof(HandsCameraController))]
		[CanEditMultipleObjects]
		public class HandsCameraEditor : Editor
		{
			SerializedProperty m_YLimitToggle, m_leftYLimit, m_rightYLimit, m_xLimit, 
							   m_moveForce, m_rotateForce, m_constantMoveForce;

			public void OnEnable()
			{
				m_YLimitToggle = serializedObject.FindProperty ("m_toggleYLimit");
				m_leftYLimit = serializedObject.FindProperty ("m_leftYLimit");
				m_rightYLimit = serializedObject.FindProperty ("m_rightYLimit");
				m_xLimit = serializedObject.FindProperty ("m_xLimit");
				m_moveForce = serializedObject.FindProperty ("m_moveForce");
				m_constantMoveForce = serializedObject.FindProperty ("m_constantMoveForce");
				m_rotateForce = serializedObject.FindProperty ("m_rotateForce");
			}

			public override void OnInspectorGUI()
			{
				serializedObject.Update ();

				HandsCameraController hc = target as HandsCameraController;

				m_YLimitToggle.boolValue = EditorGUILayout.Toggle ("Apply Yaw Limit", m_YLimitToggle.boolValue);
				if(m_YLimitToggle.boolValue)
				{
					hc.ApplyYawLimit();
				}

				m_leftYLimit.floatValue = EditorGUILayout.FloatField ("Left Yaw Limit", m_leftYLimit.floatValue);
				m_rightYLimit.floatValue = EditorGUILayout.FloatField ("Right Yaw Limit", m_rightYLimit.floatValue);
				m_xLimit.floatValue = EditorGUILayout.FloatField ("Pitch Limit", m_xLimit.floatValue);
				m_moveForce.vector3Value = EditorGUILayout.Vector3Field ("Move Force", m_moveForce.vector3Value);
				m_constantMoveForce.vector3Value = EditorGUILayout.Vector3Field ("Constant Move Force", m_constantMoveForce.vector3Value);
				m_rotateForce.vector3Value = EditorGUILayout.Vector3Field ("Rotation Speed", m_rotateForce.vector3Value);

				serializedObject.ApplyModifiedProperties ();
			}
		}

		public class HandsCameraController : MonoBehaviour
		{		
			//movement and rotation related variables
			public Vector3 m_moveForce;
			public Vector3 m_constantMoveForce;
			public Vector3 m_rotateForce;

			//y axis related variables
			private Quaternion m_leftYRotation;
			private Quaternion m_rightYRotation;
			public float m_leftYLimit;
			public float m_rightYLimit;
			public bool m_toggleYLimit;

			//x axis related variables
			private float m_xAccumulator;
			public float m_xLimit;

			protected void Start()
			{		
				m_xAccumulator = 0.0f;
				m_toggleYLimit = false;
			}

			public void ApplyYawLimit()
			{
				Quaternion initialRotation = transform.rotation;
				m_leftYRotation = initialRotation * Quaternion.AngleAxis (m_leftYLimit, Vector3.up);
				m_rightYRotation = initialRotation * Quaternion.AngleAxis (m_rightYLimit, Vector3.up);
			}

			private void UpdateYaw(CharacterController cc, HandInput inp)
			{
				if(inp != null)
				{
					float jx = inp.JoystickX;
					Quaternion rot = Quaternion.AngleAxis(m_rotateForce.y * jx, Vector3.up) * cc.transform.rotation;

				    if(m_toggleYLimit)
					{
						if(rot.eulerAngles.y < m_leftYRotation.eulerAngles.y &&
						   rot.eulerAngles.y > m_rightYRotation.eulerAngles.y)
						{
							float angleLeft = Quaternion.Angle (rot, m_leftYRotation);
							float angleRight = Quaternion.Angle(rot, m_rightYRotation);
					
							if(angleLeft > angleRight)
							{
								rot.eulerAngles = new Vector3(rot.eulerAngles.x, m_rightYRotation.eulerAngles.y, rot.eulerAngles.z);
							}
							else
							{
								rot.eulerAngles = new Vector3(rot.eulerAngles.x, m_leftYRotation.eulerAngles.y, rot.eulerAngles.z);
							}
						}
					}
					cc.transform.rotation = rot;
				}
			}

			private void UpdatePitch(CharacterController cc, HandInput inp)
			{
				if(inp != null)
				{
					float jy = inp.JoystickY;

					Quaternion xQuat;
					Camera cam = gameObject.GetComponentInChildren<Camera>();
					float xPitch = -m_rotateForce.x * jy;
					m_xAccumulator += xPitch;

					if(m_xAccumulator > m_xLimit)
					{
						m_xAccumulator = m_xLimit;
						xQuat = Quaternion.AngleAxis(m_xLimit - m_xAccumulator, Vector3.right);
					}
					else if(m_xAccumulator < -m_xLimit)
					{
						m_xAccumulator = -m_xLimit;
						xQuat = Quaternion.AngleAxis(-m_xLimit - m_xAccumulator, Vector3.right);
					}
					else xQuat = Quaternion.AngleAxis(xPitch, Vector3.right);
					
					cam.transform.rotation *= xQuat;
				}
			}

			private void UpdatePosition(CharacterController cc, HandInput inp)
			{
				if(inp != null)
				{
					float jx = inp.JoystickX;
					float jy = inp.JoystickY;
					
					Vector3 move = (cc.transform.forward * jy) + (cc.transform.right * jx);

					cc.Move(Vector3.Scale(move, m_moveForce) * Time.fixedDeltaTime + 
					        cc.transform.InverseTransformDirection(m_constantMoveForce));
				}
			}

			private void Update()
			{
				CharacterController cc = gameObject.GetComponent<CharacterController> ();
				HandInput rh = HandInputController.GetController (Hands.RIGHT);
				HandInput lh = HandInputController.GetController(Hands.LEFT);
			
				UpdateYaw(cc, rh);
				UpdatePitch(cc, rh);
				UpdatePosition(cc, lh);
			}

		}
	}
}
