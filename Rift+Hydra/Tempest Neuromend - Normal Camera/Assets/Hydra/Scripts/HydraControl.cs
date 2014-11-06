using System;
using UnityEngine;
namespace Tempest
{
	namespace RazorHydra
	{
		public class HydraControl : MonoBehaviour
		{
			private void Awake()
			{
				//DontDestroyOnLoad (gameObject);
			}

			private void OnLevelWasLoaded(int level)
			{
				GameObject objControl = GameObject.Find ("Game Control");
				GameObject objHand = GameObject.Find ("Hand Prefab");

				if(objControl != null && objHand != null)
				{
					GameControl gameControl = objControl.GetComponent<GameControl>();

					if(gameControl != null)
					{
						HandsMotionController motion = objHand.GetComponent<HandsMotionController> ();				
						HydraCharacterController character = objHand.GetComponent<HydraCharacterController> ();
						HydraCameraController camera = objHand.GetComponent<HydraCameraController> ();
					
						//speed slider setting
						character.MoveSensitivity = gameControl.inputSensitivity; 
						camera.CameraSensitivity = gameControl.inputSensitivity;
					
						//sensitivity slider setting
						motion.LinearSensitivity = gameControl.inputSensitivity;
						motion.AngularSensitivity = gameControl.inputSensitivity;
						
						foreach(Component c in objHand.GetComponents<Hand> ())
						{
							if(c is Hand)
							{
								Hand hand = c as Hand;
								hand.TriggerSensitivity = gameControl.inputSensitivity;
							}
						}
					}
				}
			}
		}
	}
}