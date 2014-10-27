
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

			private Vector3 m_correctedPosition;
			private Quaternion m_correctedRotation;

			private Vector3	m_modelPosition; 
			private Quaternion m_modelRotation; 

			public float m_triggerSensitivity;

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
				get { return m_controller != null ? m_controller.Trigger * m_triggerSensitivity : 0.0f; }
			}

			public float TriggerSensitivity
			{
				set { m_triggerSensitivity = value; }
				get { return m_triggerSensitivity; }
			}

			public Vector3 CorrectedPosition
			{
				get { return m_correctedPosition; }
				set { m_correctedPosition = value; }
			}

			public Quaternion CorrectedRotation
			{
				get { return m_correctedRotation; }
				set { m_correctedRotation = value; }
			}

			private void Start() 
			{
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


		}
	}
}

