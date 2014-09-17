using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
	

		public class HydraCharacterController : MonoBehaviour
		{
			private CharacterController m_controller;

			public Hands m_crouchHand;
			public Buttons m_crouchButton;
			public float m_crouchHeightChange;
			public float m_crouchSpeed;
			private float m_preCrouchHeight;

			private bool m_isGrounded;
			private bool m_ceilingAbove;

			public Hands m_jumpHand;
			public Buttons m_jumpButton;
			private bool m_isJumping;
			public float m_jumpImpulse;
			private float m_accumulatedJumpImpulse;
			private float m_preJumpHeight;

			private void Start()
			{
				m_controller = GetComponentInParent<CharacterController> ();
				m_preCrouchHeight = m_controller.height;
				m_accumulatedJumpImpulse = 0.0f;
			}

			private void CheckGrounded()
			{
				//check if ground below
				RaycastHit hit;
				m_isGrounded = Physics.Raycast(m_controller.transform.position, 
				                               Vector3.down, out hit, m_controller.collider.bounds.extents.y + 0.1f,
				                               ~(Physics.DefaultRaycastLayers << m_controller.gameObject.layer));
			}

		
			private void CheckCeiling()
			{
				//check if ceiling above
				Vector3 up = m_controller.transform.up;
				Vector3 center = m_controller.transform.TransformPoint(m_controller.center);
				Vector3 endCapsulePoint = center + (up * m_controller.height * 0.5f);
				
				Ray ray = new Ray(endCapsulePoint, up);
	
				Debug.DrawLine (endCapsulePoint, endCapsulePoint + Vector3.up * (m_preCrouchHeight - m_controller.height));

				RaycastHit hit;
				m_ceilingAbove = Physics.SphereCast (ray, m_controller.radius, out hit, m_preCrouchHeight - m_controller.height, 
				                           ~(Physics.DefaultRaycastLayers << m_controller.gameObject.layer));
	
			}


			private void JumpBehaviour()
			{
				HandInput input = HandInputController.GetController (m_jumpHand);

				if(input != null)
				{
					if(input.GetButtonDown(m_jumpButton) && m_isGrounded)
					{
						m_preJumpHeight = m_controller.transform.position.y;
						m_isJumping = true;
					}

					if(m_isJumping)
					{
						float y1 = m_controller.transform.position.y;
						float maxHeight = m_preJumpHeight + m_jumpImpulse;
						float y2 = Mathf.Lerp (y1, maxHeight, Time.deltaTime * m_jumpImpulse);

						m_accumulatedJumpImpulse += (y2 - y1);
						if(m_accumulatedJumpImpulse > m_jumpImpulse)
						{
							y2 = maxHeight;
							m_isJumping = false;
							m_accumulatedJumpImpulse = 0.0f;
						}
						else
						{
							float move = (y2 - y1);
							m_controller.Move(new Vector3(0.0f, move, 0.0f));
						}
					}
				}
			}

			private void CrouchBehaviour()
			{
				HandInput input = HandInputController.GetController (m_crouchHand);

				if(input != null)
				{
					if(input.GetButton(m_crouchButton))
					{
						//get to lowest crouch position while crouch button is down
						float targetHeight = m_preCrouchHeight - m_crouchHeightChange;
						m_controller.height = Mathf.Lerp(m_controller.height, targetHeight, Time.deltaTime * m_crouchSpeed);
					}

					else if(!input.GetButton(m_crouchButton))
					{
						if(!m_ceilingAbove)
						{
							//stand up while crouch button is up
							float lastHeight = m_controller.height;
							m_controller.height = Mathf.Lerp(m_controller.height, m_preCrouchHeight, Time.deltaTime * m_crouchSpeed);

							Vector3 move = new Vector3(0.0f, (m_controller.height - lastHeight) * 0.5f, 0.0f);
							m_controller.Move(move); 
						}
					}
				}
			}

			private void FixedUpdate()
			{
				CheckGrounded ();
				CheckCeiling ();

				CrouchBehaviour ();
				JumpBehaviour ();
			}

		}
	}
}
