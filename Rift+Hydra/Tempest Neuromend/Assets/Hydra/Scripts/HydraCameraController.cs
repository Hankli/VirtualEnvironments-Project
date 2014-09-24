using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraCameraController : MonoBehaviour
		{		
			//movement and rotation related variables
			public Vector3 m_rotateForce;
			public float m_inputSensitivity = 1.0f;

			//y axis related variables
			private Quaternion m_leftYRotation;
			private Quaternion m_rightYRotation;
			public float m_leftYLimit;
			public float m_rightYLimit;
			public bool m_yLimitFlag;

			//x axis related variables
			private float m_xAccumulator = 0.0f;
			private const float m_xLimit = 89.0f;
			
			private void Start()
			{
				if(m_yLimitFlag)
				{
					SetYawLimitation();
				}
				
				OVRDevice.ResetOrientation ();
			}
			
			private void SetYawLimitation()
			{
				Quaternion initialRotation = transform.parent.rotation;
				m_leftYRotation = initialRotation * Quaternion.AngleAxis (m_leftYLimit, Vector3.up);
				m_rightYRotation = initialRotation * Quaternion.AngleAxis (m_rightYLimit, Vector3.up);
			}
			
			private void UpdateYaw(CharacterController cc, HandInput inp)
			{
				if(inp != null)
				{
					float jx = inp.JoystickX * m_inputSensitivity;
					Quaternion rot = Quaternion.AngleAxis(m_rotateForce.y * jx, Vector3.up) * cc.transform.rotation;
					
					//if yaw limit is applied, then clamp rotation
					if(m_yLimitFlag)
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
				//update pitch of camera and always restrict to 89-90 degree pitch
				if(inp != null)
				{
					float jy = inp.JoystickY * m_inputSensitivity;
					
					Quaternion xQuat;
					Camera cam = transform.parent.GetComponentInChildren<Camera>();
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

			private void FixedUpdate()
			{
				CharacterController cc = transform.parent.GetComponent<CharacterController> ();
				HandInput rh = HandInputController.GetController (Hands.RIGHT);

				UpdateYaw(cc, rh);
				UpdatePitch(cc, rh);
			}
			
		}
	}
}
