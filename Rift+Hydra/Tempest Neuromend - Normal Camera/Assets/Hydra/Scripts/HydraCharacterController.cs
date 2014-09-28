using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		[RequireComponent(typeof(CharacterController))]
		public class HydraCharacterController : MonoBehaviour
		{
			private CharacterController m_controller;
			private CharacterMotor m_motor;

			public Vector3 m_constantVelocity;

			public float m_walkSpeed;
			public float m_strafeSpeed;
			public float m_moveSensitivity = 1.0f;
			
			public Hands m_crouchHand;
			public Buttons m_crouchButton;
			public float m_crouchHeightChange;
			public float m_crouchSpeed;
			private float m_preCrouchHeight;

			public Hands m_jumpHand;
			public Buttons m_jumpButton;
			public float m_jumpImpulse;
			public float m_airDrag = 0.0f;

			RaycastHit m_ceilingHit;
			private bool m_ceiling;
			
			private void Start()
			{
				m_motor = GetComponentInParent<CharacterMotor>();
				m_controller = GetComponentInParent<CharacterController> ();
				m_preCrouchHeight = m_controller.height;
			}
		
			private void CheckCeiling()
			{
				//check if ceiling above
				Vector3 up = m_controller.transform.up;
				Vector3 center = m_controller.transform.TransformPoint(m_controller.center);
				Vector3 endCapsulePoint = center + (up * m_controller.height * 0.5f);
				
				Ray ray = new Ray(endCapsulePoint, up);
				m_ceiling = Physics.SphereCast (ray, m_controller.radius, out m_ceilingHit, (m_preCrouchHeight + 0.1f) - m_controller.height, 
				                 ~(Physics.DefaultRaycastLayers << m_controller.gameObject.layer));

			}

			private void MoveBehaviour(HandInput inp)
			{
				//get scale factor for movement
				float jx = inp.JoystickX * m_moveSensitivity;
				float jy = inp.JoystickY * m_moveSensitivity;
			
				//calculate velocity
				Vector3 right = transform.right * jx * m_strafeSpeed;
				Vector3 front = transform.forward * jy * m_walkSpeed;		
				Vector3 force = right + front;

				if(!m_motor.grounded)
				{
					force.x *= m_airDrag;
					force.z *= m_airDrag;
				}

				//set velocity(except along y axis)
				m_motor.movement.velocity.x = force.x + m_constantVelocity.x;
				m_motor.movement.velocity.z = force.z + m_constantVelocity.z;
			}


			private void JumpBehaviour()
			{
				HandInput input = HandInputController.GetController (m_jumpHand);
			
				if(input != null)
				{
					if(input.GetButtonDown(m_jumpButton) && m_motor.grounded)
					{
						m_motor.inputMoveDirection = new Vector3(0.0f, m_jumpImpulse, 0.0f);
						m_motor.inputJump = true;
					}
					else
					{
						m_motor.inputJump = false;
					}
				}
			}

			private void CrouchBehaviour()
			{
				HandInput input = HandInputController.GetController (m_crouchHand);

				if(input != null)
				{
					if(input.GetButton(m_crouchButton) && m_motor.grounded)
					{
						//get to lowest crouch position while crouch button is down
						float targetHeight = m_preCrouchHeight - m_crouchHeightChange;
						m_controller.height = Mathf.Lerp(m_controller.height, targetHeight, Time.deltaTime * m_crouchSpeed);
					}

					else if(!input.GetButton(m_crouchButton) && !m_ceiling)
					{
						if(m_preCrouchHeight - m_controller.height > 0.01f)
						{
							//stand up while crouch button is up
							float lastHeight = m_controller.height;
							m_controller.height = Mathf.Lerp(m_controller.height, m_preCrouchHeight, Time.deltaTime * m_crouchSpeed);

							m_controller.Move(new Vector3(0.0f, (m_controller.height - lastHeight) * 0.5f, 0.0f));
						}
					}
				}
			}
			
			private void FixedUpdate()
			{
				HandInput movementHI = HandInputController.GetController (Hands.LEFT);

				CheckCeiling ();

				if(movementHI != null)
				{
					CrouchBehaviour ();
					JumpBehaviour ();
					MoveBehaviour (movementHI);
				}
			}

		}
	}
}
