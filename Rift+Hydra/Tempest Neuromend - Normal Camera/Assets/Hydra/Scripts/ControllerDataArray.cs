using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class ControllerDataArray
		{
			private int m_which;
			private Plugin.sixenseControllerData[] m_controllerData = null;
			
			public ControllerDataArray(int items, int which)
			{
				m_which = which;
				m_controllerData = new Plugin.sixenseControllerData[items];
			}
			
			public void Initialize()
			{
				for(int i=0; i<m_controllerData.Length; i++)
				{
					Plugin.sixenseGetNewestData(m_which, ref m_controllerData[i]);
				}
			}
			
			public void Update()
			{
				int i=0;
				
				for(; i<m_controllerData.Length - 1; i++)
				{
					m_controllerData[i] = m_controllerData[i + 1];
				}
				
				Plugin.sixenseGetNewestData(m_which, ref m_controllerData[i]);
			}
			
			public Plugin.sixenseControllerData[] ControllerData
			{
				get { return m_controllerData; }
			}
		}
	}
}
