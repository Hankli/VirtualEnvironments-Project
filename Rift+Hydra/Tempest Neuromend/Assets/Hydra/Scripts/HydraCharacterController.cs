using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		[RequireComponent(typeof(CharacterController))]
		public class HydraCharacterController : MonoBehaviour
		{
			private CharacterController m_controller;

			public Vector3 m_constantVelocity;
			private Vector3 m_acceleration;
			private Vector3 m_velocity;

			public Vector3 m_walkForce;
			public float m_inputSensitivity = 1.0f;
			
			public Hands m_crouchHand;
			public Buttons m_crouchButton;
			public float m_crouchHeightChange;
			public float m_crouchSpeed;
			private float m_preCrouchHeight;

			public Hands m_jumpHand;
			public Buttons m_jumpButton;
			public float m_jumpForce;
			public float m_airDrag = 0.0f;

			RaycastHit m_ceilingHit;
			RaycastHit m_groundHit;
			private bool m_grounded;
			private bool m_ceiling;

			public float m_gravity = 9.8f;

			private void Start()
			{
				m_controller = GetComponentInParent<CharacterController> ();
				m_preCrouchHeight = m_controller.height;
				m_acceleration = Vector3.zero;
			}

			private void CheckGrounded()
			{
				//check if ground below
				m_grounded = Physics.Raycast(m_controller.transform.position, 
				                               Vector3.down, out m_groundHit, m_controller.collider.bounds.extents.y + 0.1f,
				                               ~(Physics.DefaultRaycastLayers << m_controller.gameObject.layer));
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

			private void WalkBehaviour(HandInput inp)
			{
				//get scale factor for movement
				float jx = inp.JoystickX * m_inputSensitivity;
				float jy = inp.JoystickY * m_inputSensitivity;
				float dt = Time.deltaTime;
				
				//calculate velocity
				Vector3 right = transform.right * jx * m_walkForce.x * dt;
				Vector3 front = transform.forward * jy * m_walkForce.z * dt;		
				Vector3 force = right + front;

				if(!m_grounded)
				{
					force.x *= m_airDrag;
					force.z *= m_airDrag;
				}

				//set velocity(except along y axis)
				m_velocity.x = force.x;
				m_velocity.z = force.z;
			}

			private void JumpBehaviour()
			{
				HandInput input = HandInputController.GetController (m_jumpHand);
			
				if(input != null)
				{
					if(input.GetButtonDown(m_jumpButton) && m_grounded)
					{
						m_acceleration.y = m_jumpForce;
					}
				}
			}

			private void CrouchBehaviour()
			{
				HandInput input = HandInputController.GetController (m_crouchHand);

				if(input != null)
				{
					if(input.GetButton(m_crouchButton) && m_grounded)
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

			private void Integrate()
			{
				float dt = Time.deltaTime;
				m_velocity += m_acceleration * dt;
				m_controller.Move (m_velocity + m_constantVelocity * dt);
			}
			
			private void ApplyGravity()
			{
				if(!m_grounded)
				{
					m_acceleration.y -= m_gravity * Time.deltaTime;
				}

				else if(m_acceleration.y < 0.0f) //if grounded, acceleration that goes down gets clamped to zero
				{
					m_acceleration.y = 0.0f;
				}
			}
			
			private void FixedUpdate()
			{
				HandInput movementHI = HandInputController.GetController (Hands.LEFT);

				CheckGrounded ();
				CheckCeiling ();

				if(movementHI != null)
				{
					CrouchBehaviour ();
					JumpBehaviour ();
					WalkBehaviour (movementHI);

					ApplyGravity ();
					Integrate();
				}
			}

		}
	}
}
