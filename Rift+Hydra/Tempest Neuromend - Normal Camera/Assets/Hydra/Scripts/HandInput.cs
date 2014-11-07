using UnityEngine;

//under input

namespace Tempest
{
	namespace RazorHydra
	{

		/** used by Hand class to keep track of input to razor hydra device**/
		public class HandInput
		{
			private bool m_enabled;
			private bool m_docked;

			private Hands m_hand;
			private Hands m_handBind;

			private Buttons m_buttons;
			private Buttons m_buttonsPrevious;

			private float m_trigger;
			private float m_lastTrigger;
			private float m_triggerButtonThreshold = DefaultTriggerButtonThreshold;

			private float m_joystickX;
			private float m_joystickY;

			private Vector3 m_rawPosition;
			private Vector3 m_position;
			private Vector3 m_lastPosition;

			private Quaternion m_rawRotation;
			private Quaternion m_rotation;
			private Quaternion m_lastRotation;

			/// <summary>
			/// The default trigger button threshold constant.
			/// </summary>
			public const float DefaultTriggerButtonThreshold = 0.9f;

			/// <summary>
			/// The controller enabled state.
			/// </summary>
			public bool Enabled { get { return m_enabled; } }
			
			/// <summary>
			/// The controller docked state.
			/// </summary>
			public bool Docked { get { return m_docked; } }

			
			/// <summary>
			/// Hand the controller is bound to, which could be UNKNOWN.
			/// </summary>
			public Hands BoundedHand { get { return ( ( m_hand == Hands.UNKNOWN ) ? m_handBind : m_hand ); } }

			internal Hands HandBind { get { return m_handBind; } set { m_handBind = value; } }

			/// <summary>
			/// Value of trigger from released (0.0) to pressed (1.0).
			/// </summary>
			public float Trigger { get { return m_trigger; } }

			public float LastTrigger { get { return m_lastTrigger; } set { m_lastTrigger = value; } }
			
			/// <summary>
			/// Value of joystick X axis from left (-1.0) to right (1.0).
			/// </summary>
			public float JoystickX { get { return m_joystickX ; } }
			
			/// <summary>
			/// Value of joystick Y axis from bottom (-1.0) to top (1.0).
			/// </summary>
			public float JoystickY { get { return m_joystickY; } }
			
			/// <summary>
			/// The controller position in Unity coordinates.
			/// </summary>
			public Vector3 Position { get { return m_position; } }

			/// <summary>
			/// The previous controller position in Unity coordinates.
			/// </summary>
			public Vector3 LastPosition { get { return m_lastPosition; } }

			/// <summary>
			/// The raw controller position value.
			/// </summary>
			public Vector3 PositionRaw { get { return m_rawPosition	; } }
			
			/// <summary>
			/// The controller rotation in Unity coordinates.
			/// </summary>
			public Quaternion Rotation { get { return m_rotation; } }

			/// <summary>
			/// The previous controller position in Unity coordinates.
			/// </summary>
			public Quaternion LastRotation { get { return m_lastRotation; } }

			/// <summary>
			/// The raw controller rotation value.
			/// </summary>
			public Quaternion RotationRaw { get { return m_rawRotation; } }
			
			/// <summary>
			/// The value which the Trigger value must pass to register a TRIGGER button press.  This value can be set.
			/// </summary>
			public float TriggerButtonThreshold { get { return m_triggerButtonThreshold; } set { m_triggerButtonThreshold = value; } }

			/// <summary>
			/// Returns true if the button parameter is being pressed.
			/// </summary>
			public bool GetButton( Buttons button )
			{
				return ( ( button & m_buttons ) != 0 );
			}
			
			/// <summary>
			/// Returns true if the button parameter was pressed this frame.
			/// </summary>
			public bool GetButtonDown( Buttons button )
			{
				return ( ( button & m_buttons ) != 0 ) && ( ( button & m_buttonsPrevious ) == 0 );
			}
			
			/// <summary>
			/// Returns true if the button parameter was released this frame.
			/// </summary>
			public bool GetButtonUp( Buttons button )
			{
				return ( ( button & m_buttons ) == 0 ) && ( ( button & m_buttonsPrevious ) != 0 );
			}
			
			internal HandInput()
			{
				m_enabled = false;
				m_docked = false;
				m_hand = Hands.UNKNOWN;
				m_handBind = Hands.UNKNOWN;
				m_buttons = 0;
				m_buttonsPrevious = 0;
				m_trigger = 0.0f;
				m_lastTrigger = 0.0f;
				m_joystickX = 0.0f;
				m_joystickY = 0.0f;

				m_position.Set( 0.0f, 0.0f, 0.0f );
				m_lastPosition = m_position;

				m_rotation.Set( 0.0f, 0.0f, 0.0f, 1.0f );
				m_lastRotation = m_rotation;
			}
			
			internal void SetEnabled( bool enabled )
			{
				m_enabled = enabled;
			}

			internal void Update(int which, Plugin.sixenseControllerData[] cd )
			{
				int latest = cd.Length - 1;

				m_docked = ( cd[latest].is_docked != 0 );
				m_hand = ( Hands )cd[latest].which_hand;
		
				m_buttonsPrevious = m_buttons;
				m_buttons = ( Buttons )cd[latest].buttons;

				m_lastTrigger = m_trigger;
				m_trigger = cd[latest].trigger;

				m_joystickX = cd[latest].joystick_x;
				m_joystickY = cd[latest].joystick_y;

				m_rawPosition = new Vector3 (cd[latest].pos [0], cd[latest].pos [1], -cd[latest].pos [2]);
				m_lastPosition = m_position;
				m_position = MotionSmoother.CalculatePosition (0.2f, cd);

				m_rawRotation = new Quaternion (-cd[latest].rot_quat [0], -cd[latest].rot_quat [1], cd[latest].rot_quat [2], cd[latest].rot_quat [3]);
				m_lastRotation = m_rotation;
				m_rotation = MotionSmoother.CalculateOrientation(cd);


				if ( m_trigger > TriggerButtonThreshold )
				{
					m_buttons |= Buttons.TRIGGER;
				}
			}
		
		}///END CLASS
	}
}
