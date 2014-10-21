using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Tempest
{
	namespace Menu
	{
		public class Column<T>
		{
			public enum SortDirection
			{
				ASCEND = 0,
				DESCEND = 1
			}

			private string m_heading;
			private float m_widthScale;

			private GUIStyle m_columnStyle;
			private GUIStyle m_tupleStyle;

			private SortDirection m_sortDirection;
			private Comparison<T> m_ascendCompare;
			private Comparison<T> m_descendCompare;

			public GUIStyle TupleStyle
			{
				get { return m_tupleStyle; }
			}

			public GUIStyle ColumnStyle
			{
				get { return m_columnStyle; }
			}

			public void SetStyles(GUIStyle columnStyle, GUIStyle tupleStyle)
			{
				m_columnStyle = columnStyle;
				m_tupleStyle = tupleStyle;
			}

			public void Trigger(List<T> list)
			{
				switch(m_sortDirection)
				{
					case SortDirection.ASCEND: 
					{
						if(m_ascendCompare != null)
						{
							list.Sort(m_ascendCompare);
							m_sortDirection = SortDirection.DESCEND;
						}
					}
					break;

					case SortDirection.DESCEND: 
					{
						if(m_descendCompare != null)
						{
							list.Sort(m_descendCompare);
							m_sortDirection = SortDirection.ASCEND;
						}
					}
					break;
				}
			}

			public string Heading
			{
				get{ return m_heading; }
			}

			public float WidthScale
			{
				get{ return m_widthScale; }
			}

			public Column(string heading, float widthScale, Comparison<T> asc, Comparison<T> desc,
			              GUIStyle tupleStyle = null, GUIStyle columnStyle = null) 
			{
				m_heading = heading;
				m_widthScale = widthScale;
				m_ascendCompare = asc;
				m_descendCompare = desc;
				m_tupleStyle = tupleStyle;
				m_columnStyle = columnStyle;

			}

			public override bool Equals (object obj)
			{
				if(obj != null && 
				   this != obj && 
				   obj is Column<T>)
				{
					Column<T> t = obj as Column<T>;
					return Heading.Equals(t.Heading);
				}
			
				return false;
			}

			public override int GetHashCode ()
			{
				return m_heading.GetHashCode ();
			}
		}

		public class TableView<T>
		{
			private Vector2 m_pos;
			private float m_height;
			private float m_width;

			private GUIStyle m_viewStyle;
			private List<Column<T>> m_columns;
			private List<T> m_items;

			private Vector2 m_tableScroll = Vector2.zero;

			public TableView()
			{
				m_columns = new List<Column<T>> ();
				m_items = new List<T> ();
			}

			public void ResetScroll()
			{
				m_tableScroll = Vector2.zero;
			}

			public GUIStyle ViewStyle
			{
				get { return m_viewStyle; }
				set { m_viewStyle = value; }
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

			public bool AddColumn(string heading, float widthScale, Comparison<T> asc, Comparison<T> desc,
			                      GUIStyle tupleStyle = null, GUIStyle columnStyle = null)
			{
				if(!m_columns.Any(x => x.Heading.Equals(heading)))
				{
					m_columns.Add (new Column<T>(heading, widthScale, asc, desc, tupleStyle, columnStyle));
					return true;
				}
				return false;
			}


			public bool RemoveColumn(string heading)
			{
				return m_columns.RemoveAll (x => x.Heading.Equals (heading)) > 0;
			}

			public bool AddItem(T item) 
			{
				if(!m_items.Any(x => x.Equals(item)))
				{
					m_items.Add (item);
					return true;
				}
				return false;
			}

			public bool RemoveItem(T item)
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

			public Column<T> GetColumn(string heading)
			{
				return m_columns.Find (x => x.Heading.Equals (heading));
			}

			public void Display()
			{
				Rect rect = new Rect (m_pos.x, m_pos.y, m_width, m_height); 			
				Column<T>[] cols = m_columns.ToArray();

				GUILayout.BeginArea(rect, "", m_viewStyle);
				m_tableScroll = GUILayout.BeginScrollView (m_tableScroll);

				/////DRAW HEADING FOR TABLE COLUMNS/////
				GUILayout.BeginHorizontal ();
				foreach(Column<T> c in cols)
				{
					float width = m_width * c.WidthScale;
					GUIStyle style = c.ColumnStyle != null ? c.ColumnStyle : GUI.skin.label;

					if(GUILayout.Button(c.Heading, style, GUILayout.Width(width)))
					{	
						c.Trigger(m_items);
					}
				}
				GUILayout.EndHorizontal ();

				////DRAW TABLE TUPLES/////
				foreach(T o in m_items)
				{
					int i=0;
					string[] fields = o.ToString().Split (new string[] {"\n"}, System.StringSplitOptions.None);

					//draw tuples
					GUILayout.BeginHorizontal();
					foreach(Column<T> c in cols)
					{
						GUIStyle style = c != null ? c.TupleStyle : GUI.skin.label;
						float width = m_width * c.WidthScale;

						if(i < fields.Length)
						{
							GUILayout.Label(fields[i], style, GUILayout.Width(width));
						}
						else
						{
							GUILayout.Label("", style, GUILayout.Width(width));
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
