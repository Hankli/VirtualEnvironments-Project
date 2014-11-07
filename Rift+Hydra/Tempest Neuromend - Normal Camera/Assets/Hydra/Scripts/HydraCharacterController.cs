using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraCharacterController : MonoBehaviour
		{
			private FPSControl m_fpsControl = null;

			private float m_sensitivity = 1.0f;

			public Hands m_controlHand;
			public Hands m_jumpHand;
			public Buttons m_jumpButton;
			public Hands m_crouchHand;
			public Buttons m_crouchButton;
		


			public float Sensitivity
			{
				get { return m_sensitivity; }
				set { m_sensitivity = value; }
			}
			
			private void Start()
			{
				m_fpsControl = GetComponentInParent<FPSControl> ();
			}

			private void MoveBehaviour()
			{
				//get scale factor for movement

				HandInput input = HandInputController.GetController (m_controlHand);

				if(input != null && m_fpsControl != null)
				{
					float jx = input.JoystickX * m_sensitivity;
					float jy = input.JoystickY * m_sensitivity;

					Vector3 right = transform.right * jx;
					Vector3 front = transform.forward * jy;

					m_fpsControl.Direction = right + front;
				}
			}


			private void JumpBehaviour()
			{
				HandInput input = HandInputController.GetController (m_jumpHand);
			
				if(input != null && m_fpsControl != null)
				{
					if(input.GetButton(m_jumpButton))
					{
						m_fpsControl.ForceJump();
					}
					else
					{
						m_fpsControl.StopJump();
					}
				}
			}

			private void CrouchBehaviour()
			{
				HandInput input = HandInputController.GetController (m_crouchHand);

				if(input != null && m_fpsControl != null)
				{
					if(input.GetButton(m_crouchButton))
					{
						m_fpsControl.ForceCrouch();
					}
					else 
					{
						m_fpsControl.StopCrouch();
					}
				}
			}
			
			private void FixedUpdate()
			{
				CrouchBehaviour ();
				JumpBehaviour ();
				MoveBehaviour ();
			}

		}
	}
}
