using UnityEngine;
using System.Collections.Generic;

namespace Tempest
{
	namespace MenuGUI
	{
		public class TableContainer
		{
			public string m_heading;
			public List<object> m_data;
			public float m_width;
			public float m_height;

			public TableContainer(string heading, float width, float height)
			{
				m_heading = heading;
				m_data = new List<object> ();
				m_width = width;
				m_height = height;
			}

			public string[] Fields(int index)
			{
				if(index >= 0 && index < m_data.Count)
				{
					m_data[index].ToString().Split ("\n"[0]);
				}
				return null;
			}
		}

		public class TableView
		{
			public GUIStyle m_labelStyle;
			public Vector2 m_scrollView;
			private List<TableContainer> m_info;

			public void Update()
			{

			}
		}
	}

}
