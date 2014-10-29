using UnityEngine;

//under input

namespace Tempest
{
	namespace RazorHydra
	{
		public class HandMotionSmoother
		{
			private Vector3[] m_samples = null;
			private float m_smoothFactor;

			private int m_index;
			private int m_total;

			public HandMotionSmoother(int readings, float smoothFactor)
			{
				m_samples = new Vector3[readings];
				m_smoothFactor = smoothFactor;
				m_index = 0;
			}


			public void AddSample(Vector3 sample)
			{
				m_samples [++m_index % m_samples.Length] = sample;

				if(m_total < m_samples.Length)
				{
					++m_total;
				}
			}

			public void ClearSamples()
			{
				m_total = 0;
				m_index = 0;
			}

			public Vector3 GetSmoothedSample()
			{
				//use simple exponential smoothing for positional input data
				//x(0) = sample[0]
				//s(i) = a * x(i) + (1.0f - a) * s(i-1)

				Vector3 s_0 = m_samples[0];
				Vector3 s_t = s_0;

				for(int i = 1; i < m_total; i++)
				{
					s_t = m_smoothFactor * s_0 + (1.0f - m_smoothFactor) * s_t;
					s_0 = m_samples[i];
				}

				return s_t;
			}
		}

		/** used by Hand class to keep track of input to razor hydra device**/
		public class HandInput
		{
			private HandMotionSmoother m_smoother;

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

			public HandMotionSmoother Smoother { get { return m_smoother; } }

			/// <summary>
			/// Value of trigger from released (0.0) to pressed (1.0).
			/// </summary>
			public float Trigger { get { return m_trigger; } }

			public float LastTrigger { get { return m_lastTrigger; } set { m_lastTrigger = value; } }
			
			/// <summary>
			/// Value of joystick X axis from left (-1.0) to right (1.0).
			/// </summary>
			public float JoystickX { get { return m_joystickX; } }
			
			/// <summary>
			/// Value of joystick Y axis from bottom (-1.0) to top (1.0).
			/// </summary>
			public float JoystickY { get { return m_joystickY; } }
			
			/// <summary>
			/// The controller position in Unity coordinates.
			/// </summary>
			public Vector3 Position { get { return m_position; } set { m_position = value; }}
			public Vector3 LastPosition { get { return m_lastPosition; } set { m_lastPosition = value; }}


			/// <summary>
			/// The raw controller position value.
			/// </summary>
			public Vector3 PositionRaw { get { return m_rawPosition	; } }
			
			/// <summary>
			/// The controller rotation in Unity coordinates.
			/// </summary>
			public Quaternion Rotation { get { return m_rotation; } }
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

				m_smoother = new HandMotionSmoother (3, 0.3f);
			}
			
			internal void SetEnabled( bool enabled )
			{
				m_enabled = enabled;
			}

			internal void Update( ref Plugin.sixenseControllerData cd )
			{
				m_docked = ( cd.is_docked != 0 );
				m_hand = ( Hands )cd.which_hand;
		
				m_buttonsPrevious = m_buttons;
				m_buttons = ( Buttons )cd.buttons;

				m_lastTrigger = m_trigger;
				m_trigger = cd.trigger;

				m_joystickX = cd.joystick_x;
				m_joystickY = cd.joystick_y;

				m_rawPosition = new Vector3 (cd.pos [0], cd.pos [1], -cd.pos [2]);
				m_smoother.AddSample (m_rawPosition);

				m_lastPosition = m_position;
				m_position = m_smoother.GetSmoothedSample();

				m_rawRotation = new Quaternion (-cd.rot_quat [0], -cd.rot_quat [1], cd.rot_quat [2], cd.rot_quat [3]);
			
				m_lastRotation = m_rotation;
				m_rotation = m_rawRotation;
		

				if ( m_trigger > TriggerButtonThreshold )
				{
					m_buttons |= Buttons.TRIGGER;
				}
			}
		
		}///END CLASS
	}
}
