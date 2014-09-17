
//
// Copyright (C) 2013 Sixense Entertainment Inc.
// All Rights Reserved
//
// Sixense Driver Unity Plugin
// Version 1.0
//

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


namespace Tempest
{
	namespace RazorHydra
	{
		/// <summary>
		/// SixenseInput provides an interface for accessing Sixense controllers.
		/// </summary>
		/// <remarks>
		/// This script should be bound to a GameObject in the scene so that its Start(), Update() and OnApplicationQuit() methods are called.  This can be done by adding the SixenseInput prefab to a scene.  The public static interface to the Controller objects provides a user friendly way to integrate Sixense controllers into your application.
		/// </remarks>
		public class HandInputController : MonoBehaviour
		{
			/// <summary>
			/// Max number of controllers allowed by driver.
			/// </summary>
			public const uint MAX_CONTROLLERS = 4;

			/// <summary>
			/// Enable or disable the controller manager.
			/// </summary>
			public static bool m_controllerManagerEnabled = true;
			
			private static HandInput[] m_controllers = new HandInput[MAX_CONTROLLERS];

			private static ControllerManagerState m_controllerManagerState = ControllerManagerState.NONE;

			/// <summary>
			/// Access to Controller objects.
			/// </summary>
			public static HandInput[] Controllers { get { return m_controllers; } }

			/// <summary>
			/// Gets the Controller object bound to the specified hand.
			/// </summary>
			public static HandInput GetController( Hands hand )
			{
				for ( int i = 0; i < MAX_CONTROLLERS; i++ )
				{
					if ( ( m_controllers[i] != null ) && ( m_controllers[i].BoundedHand == hand ) )
					{
						return m_controllers[i];
					}
				}
				
				return null;
			}

			/// <summary>
			/// Returns true if the base for zero-based index i is connected.
			/// </summary>
			public static bool IsBaseConnected( int i )
			{
				return ( Plugin.sixenseIsBaseConnected( i ) != 0 );
			}
			
			/// <summary>
			/// Initialize the sixense driver and allocate the controllers.
			/// </summary>
			private void Start()
			{
				Plugin.sixenseInit();
				for ( int i = 0; i < MAX_CONTROLLERS; i++ )
				{
					m_controllers[i] = new HandInput();
				}
			}

			
			/// <summary>
			/// Update the static controller data once per frame.
			/// </summary>
			private void Update()
			{
				// update controller data
				uint numControllersBound = 0;
				uint numControllersEnabled = 0;

				Plugin.sixenseControllerData cd = new Plugin.sixenseControllerData(); //capture all data in loop

				for ( int i = 0; i < MAX_CONTROLLERS; i++ )
				{
					if ( m_controllers[i] != null )
					{
						if ( Plugin.sixenseIsControllerEnabled( i ) == 1 )
						{
							Plugin.sixenseGetNewestData( i, ref cd );
							m_controllers[i].Update( ref cd );
							m_controllers[i].SetEnabled( true );

							numControllersEnabled++; //controller is enabled and updated

							//check if controller manager is enabled and its bounded to left or right hand
							if ( m_controllerManagerEnabled && ( m_controllers[i].BoundedHand != Hands.UNKNOWN ) )
							{
								numControllersBound++;
							}
						}
						else
						{
							m_controllers[i].SetEnabled( false );
						}
					}
				}
				
				// update controller manager
				if ( m_controllerManagerEnabled )
				{
					if ( numControllersEnabled < 2 )
					{
						//if less than 2 controllers enabled, then signal the need to 
						m_controllerManagerState = ControllerManagerState.NONE;
					}
					
					switch( m_controllerManagerState )
					{
						case ControllerManagerState.NONE:
						{
							if ( IsBaseConnected( 0 ) && ( numControllersEnabled > 1 ) )
							{
								if ( numControllersBound == 0 ) //if no controllers have been bounded to either hand
								{
									m_controllerManagerState = ControllerManagerState.BIND_CONTROLLER_ONE; 
								}
								else if ( numControllersBound == 1 ) //if one controller bounded to a hand
								{
									m_controllerManagerState = ControllerManagerState.BIND_CONTROLLER_TWO;
								}
							}
						}
						break;

						case ControllerManagerState.BIND_CONTROLLER_ONE:
						{
							if ( numControllersBound > 0 ) //if only one hand bounded, bind the second next
							{
								m_controllerManagerState = ControllerManagerState.BIND_CONTROLLER_TWO;
							}
							else
							{
							//find which hand to bind controller to
								for ( int i = 0; i < MAX_CONTROLLERS; i++ )
								{
									if ( ( m_controllers[i] != null ) &&
								    	m_controllers[i].GetButtonDown( Buttons.TRIGGER ) &&
								    ( 	m_controllers[i].BoundedHand == Hands.UNKNOWN ) )
									{
										m_controllers[i].HandBind = Hands.LEFT;
									 	Plugin.sixenseAutoEnableHemisphereTracking( i );
										m_controllerManagerState = ControllerManagerState.BIND_CONTROLLER_TWO;
										break;
									}
								}
							}
						}
						break;

						case ControllerManagerState.BIND_CONTROLLER_TWO:
						{
							if ( numControllersBound > 1 ) //if more than one hand has been bounded, then we are done
							{
								m_controllerManagerState = ControllerManagerState.NONE;
							}
							else
							{
								for ( int i = 0; i < MAX_CONTROLLERS; i++ )
								{
									if ( ( m_controllers[i] != null ) && 
								    	m_controllers[i].GetButtonDown( Buttons.TRIGGER ) &&
								    ( 	m_controllers[i].BoundedHand == Hands.UNKNOWN ) )
									{
										m_controllers[i].HandBind = Hands.RIGHT;
										Plugin.sixenseAutoEnableHemisphereTracking( i );
										m_controllerManagerState = ControllerManagerState.NONE;
										break;
									}
								}
							}
						}
						break;

						default: 
							break;
					}//end switch
				}//end if
			}
			
			/// <summary>
			/// Updates the controller manager GUI.
			/// </summary>
			private void OnGUI()
			{
				if ( m_controllerManagerEnabled && ( m_controllerManagerState != ControllerManagerState.NONE ) )
				{
					GUIStyle style = new GUIStyle();
					style.alignment = TextAnchor.MiddleCenter;
					style.fontStyle = FontStyle.Normal;
					style.fontSize = 15;

					uint boxWidth = 300;
					uint boxHeight = 24;
					string boxText = ( m_controllerManagerState == ControllerManagerState.BIND_CONTROLLER_ONE ) ?
						"Point left controller at base and pull trigger." :
						"Point right controller at base and pull trigger.";

					GUI.Box( new Rect(( ( Screen.width / 2 ) - ( boxWidth / 2 ) ), 
					                  ( ( Screen.height / 2 ) - ( boxHeight / 2 ) ),
					                      boxWidth, boxHeight), boxText, style);
				}
			}
			
			/// <summary>
			/// Exit sixense when the application quits.
			/// </summary>
			private void OnApplicationQuit()
			{
				Plugin.sixenseExit();
			}
		}	   	
	}
}
