using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HandsCameraController : MonoBehaviour
		{

			private float m_xAccumulator;
			public float m_xLimit;
			public Vector3 m_moveForce;
			public Vector3 m_rotateForce;

			protected void Start()
			{		
				m_xAccumulator = 0.0f;
			}

			protected void Update()
			{
				CharacterController cc = gameObject.GetComponent<CharacterController> ();
				HandInput lhc = HandInputController.GetController (Hands.LEFT);
				HandInput rhc = HandInputController.GetController (Hands.RIGHT);
				float jx = 0.0f, jy = 0.0f;

				if(lhc != null)
				{
					jx = lhc.JoystickX;
					jy = lhc.JoystickY;

					Vector3 move = (cc.transform.forward * jy) + (cc.transform.right * jx);
					cc.Move(Vector3.Scale(move, m_moveForce) * Time.fixedDeltaTime);
				}

				if(rhc != null)
				{
					jx = rhc.JoystickX;
					jy = rhc.JoystickY;
			
					//rotate
					Quaternion y = Quaternion.AngleAxis(m_rotateForce.y * jx, Vector3.up);
					cc.transform.rotation =  y * cc.transform.rotation;


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

					//Debug.Log (dir);
				}
	 		}
		}
	}
}
