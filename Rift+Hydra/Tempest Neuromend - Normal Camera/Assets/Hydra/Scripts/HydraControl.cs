using System;
using UnityEngine;
namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraControl
		{
			public float m_characterMoveSensitivity = 1.0f;
			public float m_cameraRotateSensitivity = 1.0f;

			public float m_handMoveSensitivity = 1.0f;
			public float m_handRotateSensitivity = 1.0f;

			public float m_throwingSensitivity = 2.0f;
			public float m_grippingSensitivity = 2.0f;
		
		    public void ApplyChanges(GameObject o)
			{
				HandsMotionController motion = o.GetComponent<HandsMotionController> ();
				motion.m_linearSensitivity = m_handMoveSensitivity;
				motion.m_rotationSensitivity = m_handRotateSensitivity;

				HydraCameraController cam = o.GetComponent<HydraCameraController> ();
				cam.m_cameraSensitivity = m_cameraRotateSensitivity;

				HydraCharacterController character = o.GetComponent<HydraCharacterController> ();
				character.m_moveSensitivity = m_characterMoveSensitivity; 
			}
		}

	}
}