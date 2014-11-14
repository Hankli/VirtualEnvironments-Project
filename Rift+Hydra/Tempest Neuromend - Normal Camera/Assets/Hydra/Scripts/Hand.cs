
//
// Copyright (C) 2013 Sixense Entertainment Inc.
// All Rights Reserved
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Basic class to represent a hand 
namespace Tempest
{
	namespace RazorHydra
	{
		public class Hand : MonoBehaviour
		{
			public Hands m_hands;
			private HandInput m_controller = null;

			private FSM<Hand> m_animationFSM;
			private Animator m_animator; 

			private Vector3 m_position;
			private Quaternion m_rotation;

			private Vector3	m_modelPosition; 
			private Quaternion m_modelRotation; 

			private float m_triggerSensitivity = 1.0f;
			private Vector3 m_contactNormal;

			public Vector3 ContactNormal
			{
				get { return m_contactNormal; }
			}

			public Quaternion ModelRotation
			{
				set { m_modelRotation = value; }
				get { return m_modelRotation; }
			}
		
			public Vector3 ModelPosition
			{
				set { m_modelPosition = value; }
				get { return m_modelPosition; }
			}

			public HandInput Controller
			{
				set { m_controller = value; }
				get { return m_controller; }
			}

			public Animator AnimationInfo
			{
				set { m_animator = value; }
				get { return m_animator; }
			}

			public float TriggerValue
			{
				get 
				{ 
					if(m_controller == null)
					{
						return 0.0f;
					}

					float triggerVal = m_controller.Trigger;
					if(triggerVal > 0.0f && m_triggerSensitivity > 0.0f)
					{
						triggerVal += (1.0f - triggerVal) * m_triggerSensitivity;
					}
					return triggerVal;
				}
			}

			public float TriggerSensitivity
			{
				set { m_triggerSensitivity = value; }

				get { return m_triggerSensitivity; }
			}

			public Vector3 Position
			{
				get { return m_position; }
				set { m_position = value; }
			}

			public Quaternion RotationalReference
			{
				get { return m_rotation; }
				set { m_rotation = value; }
			}

			public float SensitizedTriggerValue
			{
				get 
				{
					return Mathf.Clamp(TriggerValue * (1.0f + TriggerSensitivity), 0.0f, 1.0f);
				}
			}

			private void Start() 
			{
				m_rotation = Quaternion.identity;
				m_position = Vector3.zero;

				m_contactNormal = Vector3.zero;

				//use interpolation and continous detection mode
				rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

				// get the Animator and set initial editor rotation + position of hand
				m_animator = gameObject.GetComponent<Animator>();

				m_modelRotation = transform.localRotation;
				m_modelPosition = transform.localPosition;

				//set initial animations
				m_animationFSM = new FSM<Hand> (this);
				m_animationFSM.SwitchBackground (new HandIdleAnimationState ());
				m_animationFSM.SwitchLocal (new HandClenchAnimationState ());
			}

			private void Update()
			{
				if ( m_controller == null )
				{
					m_controller = HandInputController.GetController( m_hands );
				}
				
				else if (m_animator != null )
				{
					UpdateHandAnimation();
				}
			}

			// Updates the animated object from controller input.
			private void UpdateHandAnimation()
			{
				m_animationFSM.Update (this);
			}

			private void OnCollisionEnter(Collision col)
			{
				m_contactNormal = -col.contacts [0].normal * 0.98f; 
			}
			
			private void OnCollisionStay(Collision col)
			{
				m_contactNormal = -col.contacts [0].normal * 0.98f; 
			}
			
			private void OnCollisionExit(Collision col)
			{
				m_contactNormal = Vector3.zero;
			}
		}
	}
}

