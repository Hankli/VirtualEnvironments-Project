using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraCharacterController : MonoBehaviour
		{
			private float m_sensitivity = 0.0f;
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
			}

			private void MoveBehaviour(FPSControl fpsControl)
			{
				//get scale factor for movement

				HandInput input = HandInputController.GetController (m_controlHand);

				if(input != null)
				{
					float jx = input.JoystickX;
					float jy = input.JoystickY;
					if(m_sensitivity > 0.0f)
					{
						if(Mathf.Abs(jy) > 0.0f)
						{
							jy += jy < 0.0f ? -(1.0f + jy) * m_sensitivity : (1.0f - jy) * m_sensitivity;
						}

						if(Mathf.Abs(jx) > 0.0f)
						{
							jx += jx < 0.0f ? -(1.0f + jx) * m_sensitivity : (1.0f - jx) * m_sensitivity;
						}
					}
					jx *= Time.timeScale;
					jy *= Time.timeScale;

					Vector3 right = transform.right * jx;
					Vector3 front = transform.forward * jy;

					fpsControl.Direction = right + front;
				}
			}


			private void JumpBehaviour(FPSControl fpsControl)
			{
				HandInput input = HandInputController.GetController (m_jumpHand);
			
				if(input != null)
				{
					if(input.GetButton(m_jumpButton))
					{
						fpsControl.ForceJump();
					}
					else
					{
						fpsControl.StopJump();
					}
				}
			}

			private void CrouchBehaviour(FPSControl fpsControl)
			{
				HandInput input = HandInputController.GetController (m_crouchHand);

				if(input != null)
				{
					if(input.GetButton(m_crouchButton))
					{
						fpsControl.ForceCrouch();
					}
					else 
					{
						fpsControl.StopCrouch();
					}
				}
			}
			
			private void FixedUpdate()
			{
				FPSControl fpsControl = GetComponentInParent<FPSControl> ();

				if(fpsControl != null)
				{
					CrouchBehaviour (fpsControl);
					JumpBehaviour (fpsControl);
					MoveBehaviour (fpsControl);
				}
			}

		}
	}
}
