
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
			private Animator m_animator; //animation stuff
			private float m_fLastTriggerVal;
			private Vector3	m_modelPosition; 
			private Quaternion m_modelRotation; 

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

			public float TriggerValue
			{
				get { return m_controller != null ? m_controller.Trigger : 0.0f; }
			}
			
			protected void Start() 
			{
				//use interpolation and continous detection mode
				rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

				// get the Animator and set initial editor rotation + position of hand
				m_animator = gameObject.GetComponent<Animator>();

				m_modelRotation = transform.localRotation;
				m_modelPosition = transform.localPosition;
			}

			protected void Update()
			{
				if ( m_controller == null )
				{
					m_controller = HandInputController.GetController( m_hands );
				}
				
				else if ( m_animator != null )
				{
					UpdateHandAnimation();
				}
			}

			// Updates the animated object from controller input.
			protected void UpdateHandAnimation()
			{
				//UPDATE ANIMATION HERE!!!!!!
				// Point right(one) or left(two)
				if ( m_hands == Hands.RIGHT ? m_controller.GetButton(Buttons.ONE) : m_controller.GetButton(Buttons.TWO) )
				{
					m_animator.SetBool( "Point", true );
				}
				else
				{
					m_animator.SetBool( "Point", false );
				}
				
				// Grip Ball with right(2) or left(1)
				if ( m_hands == Hands.RIGHT ? m_controller.GetButton(Buttons.TWO) : m_controller.GetButton(Buttons.ONE)  )
				{
					m_animator.SetBool( "GripBall", true );
				}
				else
				{
					m_animator.SetBool( "GripBall", false );
				}
				
				// Hold Book with right(3) ot left(4)
				if ( m_hands == Hands.RIGHT ? m_controller.GetButton(Buttons.THREE) : m_controller.GetButton(Buttons.FOUR) )
				{
					m_animator.SetBool( "HoldBook", true );
				}
				else
				{
					m_animator.SetBool( "HoldBook", false );
				}

				// Fist with right or left
				float fTriggerVal = Mathf.Lerp( m_fLastTriggerVal, m_controller.Trigger, 0.1f );
				m_fLastTriggerVal = fTriggerVal;
				
				if ( fTriggerVal > 0.01f )
				{
					m_animator.SetBool( "Fist", true );
				}
				else
				{
					m_animator.SetBool( "Fist", false );
				}
				
				m_animator.SetFloat("FistAmount", fTriggerVal);
				
				// Idle
				if ( m_animator.GetBool("Fist") == false &&  
				    m_animator.GetBool("HoldBook") == false && 
				    m_animator.GetBool("GripBall") == false && 
				    m_animator.GetBool("Point") == false )
				{
					m_animator.SetBool("Idle", true);
				}
				else
				{
					m_animator.SetBool("Idle", false);
				}
			}

			private void OnCollisionEnter(Collision cl)
			{
				rigidbody.velocity = Vector3.zero;
			}

			private void OnCollisionStay(Collision cl)
			{
				rigidbody.velocity = Vector3.zero;
			}

			private void OnCollisionExit(Collision cl)
			{
			}

		}
	}
}

