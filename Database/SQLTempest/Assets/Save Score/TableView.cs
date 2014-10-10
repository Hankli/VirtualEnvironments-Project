using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Tempest
{
	namespace MenuGUI
	{
		public class Column
		{
			private string m_heading;
			private float m_widthScale;
		
			public string Heading
			{
				get{ return m_heading; }
			}

			public float WidthScale
			{
				get{ return m_widthScale; }
			}

			public Column(string heading, float width) 
			{
				m_heading = heading;
				m_widthScale = width;
			}

			public override bool Equals (object obj)
			{
				if(obj != null && 
				   this != obj && 
				   obj is Column)
				{
					Column t = obj as Column;
					return Heading.Equals(t.Heading);
				}
			
				return false;
			}

			public override int GetHashCode ()
			{
				return m_heading.GetHashCode ();
			}
		}

		public class TableView
		{
			private Vector2 m_pos;
			private float m_height;
			private float m_width;

			private GUIStyle m_viewStyle;
			private GUIStyle m_tupleStyle;
			private GUIStyle m_columnStyle;

			private HashSet<Column> m_columns;
			private HashSet<object> m_items;

			private Vector2 m_tableScroll = Vector2.zero;

			public TableView()
			{
				m_columns = new HashSet<Column> ();
				m_items = new HashSet<object> ();
			}

			public GUIStyle ViewStyle
			{
				get { return m_viewStyle; }
				set { m_viewStyle = value; }
			}

			public GUIStyle ColumnStyle
			{
				get { return m_columnStyle; }
				set { m_columnStyle = value; }
			}

			public GUIStyle TupleStyle
			{
				get { return m_tupleStyle; }
				set { m_tupleStyle = value; }
			}

			public float Height
			{
				get { return m_height; }
				set { m_height = value; }
			}

			public float Width
			{
				get { return m_width; }
				set { m_width = value; }
			}

			public Vector2 Position
			{
				get { return m_pos; }
				set { m_pos = value; }
			}

			public bool AddColumn(string heading, float width)
			{
				return m_columns.Add (new Column(heading, width));
			}

			/*
			public bool RemoveColumn(string heading)
			{
				return m_columns.RemoveWhere
				(
					delegate (Column x)
	            	{
						return x.Heading.CompareTo(heading);
					}
				);
			}
*/
			public bool AddItem(object item)
			{
				return m_items.Add (item);
			}

			public bool RemoveItem(object item)
			{
				return m_items.Remove (item);
			}

			public void DropItems()
			{
				m_items.Clear ();
			}

			public void DropColumns()
			{
				m_items.Clear ();
				m_columns.Clear ();
			}

			public void Display()
			{
				Rect rect = new Rect (m_pos.x, m_pos.y, m_width, m_height); 			
				Column[] cols = m_columns.ToArray();

				GUILayout.BeginArea(rect, "", m_viewStyle);
				m_tableScroll = GUILayout.BeginScrollView (m_tableScroll);

				/////DRAW HEADING FOR TABLE COLUMNS/////
				GUILayout.BeginHorizontal ();
				foreach(Column c in cols)
				{
					float width = m_width * c.WidthScale;
					GUILayout.Label(c.Heading, m_columnStyle, GUILayout.Width(width));
				}
				GUILayout.EndHorizontal ();

				////DRAW TABLE TUPLES/////
				foreach(object o in m_items)
				{
					int i=0;
					string[] fields = o.ToString().Split (new string[] {"\n"}, System.StringSplitOptions.None);

					//draw tuples
					GUILayout.BeginHorizontal();
					foreach(Column c in cols)
					{
						float width = m_width * c.WidthScale;

						if(i < fields.Length)
						{
							GUILayout.Label(fields[i], m_tupleStyle, GUILayout.Width(width));
						}
						else
						{
							GUILayout.Label("-", m_tupleStyle, GUILayout.Width(width));
						}
						++i;
					}
					GUILayout.EndHorizontal();
				}

				GUILayout.EndScrollView ();
				GUILayout.EndArea();
			}
		}
	}

}
