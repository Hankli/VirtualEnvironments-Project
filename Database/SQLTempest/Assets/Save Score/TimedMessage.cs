using UnityEngine;

namespace Tempest
{
	namespace Menu
	{
		public class TimedMessage
		{
			private float m_displayTime;
			private float m_startTime;
			private GUIStyle m_style;
			private string m_log;

			public TimedMessage()
			{				
				m_log = "";
				m_startTime = Time.time;
				m_displayTime = 0f;
			}

			public void Begin(string msg, float lifetime, GUIStyle style)
			{
				m_style = style;
				m_log = msg;
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
					GUILayout.Label (m_log, m_style);	
				}
			}
		}
	}
}
