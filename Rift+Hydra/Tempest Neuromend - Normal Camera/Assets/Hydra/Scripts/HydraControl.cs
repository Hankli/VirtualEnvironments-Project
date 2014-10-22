using System;
using UnityEngine;
namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraControl : MonoBehaviour
		{
			public float m_leftJoystickSens = 1.0f;
			public float m_rightJoystickSens = 1.0f;

			public float m_linearHandSens = 1.0f;
			public float m_angularHandSens = 1.0f;

			public float m_throwSens = 1.0f;
			public float m_triggerSens = 1.0f;

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

					character.MoveSensitivity = m_leftJoystickSens; 
					cam.CameraSensitivity = m_rightJoystickSens;
					motion.MoveSensitivity = m_linearHandSens;
					motion.RotateSensitivity = m_angularHandSens;

					Component[] comp = o.GetComponents<Hand> ();
					foreach(Component c in comp)
					{
						if(c is Hand)
						{
							Hand hand = c as Hand;
							hand.TriggerSensitivity = m_triggerSens;

							HandsObjectPicker picker = hand.GetComponent<HandsObjectPicker>();
							picker.ThrowSensitivity = m_throwSens;
						}
					}
				}
			}
		}
	}
}