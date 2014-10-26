using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraCharacterController : MonoBehaviour
		{
			private CharacterController m_controller;
			private CharacterMotor m_motor;

			public float m_walkSpeed = 2.0f;
			public float m_strafeSpeed = 2.0f;

			private float m_constantWalkSpeed = 0.0f;

			private float m_moveSensitivity = 1.0f;

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

			public float MoveSensitivity
			{
				get { return m_moveSensitivity; }
				set { m_moveSensitivity = value; }
			}

			public float ConstantWalkSpeed
			{
				get { return m_constantWalkSpeed; }
				set { m_constantWalkSpeed = value; }
			}

			public float WalkSpeed
			{
				get { return m_walkSpeed; }
				set { m_walkSpeed = value; } 
			}

			public float StrafeSpeed
			{
				get { return m_strafeSpeed; }
				set { m_strafeSpeed = value; }
			}
			
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
				if(m_motor != null)
				{
					float jx = inp.JoystickX * m_moveSensitivity;
					float jy = inp.JoystickY * m_moveSensitivity;
				
					//calculate velocity
					Vector3 right = transform.right * jx * m_strafeSpeed;
					Vector3 front = transform.forward * jy * m_walkSpeed;
					Vector3 force = right + front;

					if(force.z < m_constantWalkSpeed)
					{
						force.z = m_constantWalkSpeed;
					}

					if(!m_motor.grounded)
					{
						force.x *= Mathf.Pow(m_airDrag, Time.deltaTime * Time.timeScale);
						force.z *= Mathf.Pow(m_airDrag, Time.deltaTime * Time.timeScale);
					}

					force *= Time.timeScale;

					//set velocity(except along y axis)
					m_motor.movement.velocity.x = force.x;
					m_motor.movement.velocity.z = force.z;
				}
			}


			private void JumpBehaviour()
			{
				HandInput input = HandInputController.GetController (m_jumpHand);
			
				if(input != null)
				{
					if(input.GetButtonDown(m_jumpButton) && m_motor.grounded)
					{
						m_motor.inputMoveDirection = new Vector3(0.0f, m_jumpImpulse, 0.0f) * Time.timeScale;
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
						m_controller.height = Mathf.Lerp(m_controller.height, targetHeight, Time.deltaTime * m_crouchSpeed * Time.timeScale);
					}

					else if(!input.GetButton(m_crouchButton) && !m_ceiling)
					{
						if(m_preCrouchHeight - m_controller.height > 0.01f)
						{
							//stand up while crouch button is up
							float lastHeight = m_controller.height;
							m_controller.height = Mathf.Lerp(m_controller.height, m_preCrouchHeight, Time.deltaTime * m_crouchSpeed * Time.timeScale);

							m_controller.Move(new Vector3(0.0f, (m_controller.height - lastHeight) * 0.5f, 0.0f));
						}
					}
				}
			}
			
			private void FixedUpdate()
			{
				HandInput movementHI = HandInputController.GetController (Hands.LEFT);

				if(m_motor != null && movementHI != null)
				{
					CheckCeiling ();

					CrouchBehaviour ();
					JumpBehaviour ();
					MoveBehaviour (movementHI);
				}
			}

		}
	}
}
