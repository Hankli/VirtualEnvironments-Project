using System;
using UnityEngine;
namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraControl : MonoBehaviour
		{
			public float m_moveJoystickSensitivity = 1.0f;
			public float m_rotateJoystickSensitivity = 1.0f;

			public float m_handMoveSensitivity = 1.0f;
			public float m_handRotateSensitivity = 1.0f;

			public float m_throwingSensitivity = 1.0f;
			public float m_triggerSensitivity = 1.0f;

			public float m_walkSpeed = 3.0f;
			public float m_strafeSpeed = 3.0f;

			private void Awake()
			{
				DontDestroyOnLoad (gameObject);
			}

			private void OnLevelWasLoaded(int level)
			{
				GameObject hand = GameObject.Find ("Hand Prefab");
				if(hand != null)
				{
					ApplyChanges (hand);
				}
			}

		    private void ApplyChanges(GameObject o)
			{
				if(o != null)
				{
					HandsMotionController motion = o.GetComponent<HandsMotionController> ();				
					HydraCharacterController character = o.GetComponent<HydraCharacterController> ();
					HydraCameraController cam = o.GetComponent<HydraCameraController> ();

					cam.CameraSensitivity = m_rotateJoystickSensitivity;
					motion.MoveSensitivity = m_handMoveSensitivity;
					motion.RotateSensitivity = m_handRotateSensitivity;
					character.MoveSensitivity = m_moveJoystickSensitivity; 
					character.WalkSpeed = m_walkSpeed;  
					character.StrafeSpeed = m_strafeSpeed; 

					Component[] comp = o.GetComponents<Hand> ();
					foreach(Component c in comp)
					{
						if(c is Hand)
						{
							Hand hand = c as Hand;
							hand.TriggerSensitivity = m_triggerSensitivity;

							HandsObjectPicker picker = hand.GetComponent<HandsObjectPicker>();
							picker.ThrowSensitivity = m_throwingSensitivity;
						}
					}
				}
			}
		}
	}
}