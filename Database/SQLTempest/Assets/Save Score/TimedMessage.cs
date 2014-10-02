using UnityEngine;

namespace Tempest
{
	namespace MenuGUI
	{
		public class TimedMessage
		{
			private float m_displayTime;
			private float m_startTime;
			private Rect m_rect;
			private GUIStyle m_style;
			private string m_message;

			public TimedMessage()
			{				
				m_message = "";
				m_startTime = Time.time;
				m_displayTime = 0f;
			}

			public void Begin(Rect rect, string msg, float lifetime, GUIStyle style)
			{
				m_rect = rect;
				m_style = style;
				m_message = msg;
				m_displayTime = lifetime;
				m_startTime = Time.time;
			}

			public void End()
			{
				m_startTime = 0f;
			}

			public void Display()
			{
				float t = Time.time;
				if(t - m_startTime <= m_displayTime)
				{
					GUI.Label (m_rect, m_message, m_style);	
				}
			}
		}
	}
}
