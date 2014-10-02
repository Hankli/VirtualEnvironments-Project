using UnityEngine;

namespace Tempest
{
	namespace MenuGUI
	{
		public class CalendarMenu 
		{
			private string[] m_monthField = new string[12];
			private string[] m_dayField;
			private string[] m_yearField;
			
			private int m_monthSelection;
			private int m_yearSelection;
			private int m_daySelection;
			private int m_yearRange;
			
			private Vector2 m_dayScrollPos; 
			private Vector2 m_monthScrollPos;
			private Vector2 m_yearScrollPos;
			
			private bool m_dropDayList;
			private bool m_dropMonthList;
			private bool m_dropYearList;
			
			public Rect m_dayPos;
			public Rect m_monthPos;
			public Rect m_yearPos;
			public int m_xCount;
			public int m_scrollPadding;
			public int m_dropPadding;
			
			public CalendarMenu(int yearRange)
			{
				m_dropDayList = false;
				m_dropYearList = false;
				m_dropMonthList = false;

				m_xCount = 5;
				m_scrollPadding = 12;
				m_dropPadding = 5;
				
				YearRange = yearRange;
				
				MakeToday ();
			}

			
			public void Render()
			{
				GUIStyle style = new GUIStyle (GUI.skin.button);
				style.fontSize = 15;
				style.alignment = TextAnchor.MiddleCenter;
				style.fontStyle = FontStyle.Italic;
			
				if(GUI.Button(m_dayPos, SelectedDay, style))
				{
					m_dropDayList = !m_dropDayList;
				}
				else if(GUI.Button(m_monthPos, SelectedMonth, style))
				{
					m_dropMonthList = !m_dropMonthList;
				}
				else if(GUI.Button(m_yearPos, SelectedYear, style))
				{
					m_dropYearList = !m_dropYearList;
				}
				
				DayDropList ();
				MonthDropList ();
				YearDropList ();
			}
			
			private void DayDropList()
			{
				GUIStyle style = new GUIStyle (GUI.skin.button);
				style.margin = new RectOffset (1, 1, 1, 1);
				style.alignment = TextAnchor.MiddleCenter;
				style.hover.textColor = Color.red;
				
				if(m_dropDayList)
				{
					const int RIGHT_PADDING = 15;
					const int BOTTOM_PADDING = 5;
					
					Rect pos = new Rect(m_dayPos.x, m_dayPos.y + m_dayPos.height + m_dropPadding, m_dayPos.width + m_scrollPadding, m_dayPos.height * m_xCount);
					Rect view = new Rect (0f, 0f, m_dayPos.width * 0.25f, m_dayPos.height * m_dayField.Length);
					
					m_dayScrollPos = GUI.BeginScrollView (pos, m_dayScrollPos, view);

					int lastSelect = m_daySelection;
					m_daySelection = GUI.SelectionGrid(new Rect(0f, 0f, m_dayPos.width, m_dayPos.height * m_dayField.Length), 
					                                   m_daySelection, m_dayField, 1, style);

					GUI.EndScrollView ();
				}
			}
			
			private void MonthDropList()
			{
				GUIStyle style = new GUIStyle (GUI.skin.button);
				style.margin = new RectOffset (1, 1, 1, 1);
				style.alignment = TextAnchor.MiddleCenter;
				style.hover.textColor = Color.red;
				
				if(m_dropMonthList)
				{
					Rect pos = new Rect(m_monthPos.x, m_monthPos.y + m_monthPos.height + m_dropPadding, m_monthPos.width + m_scrollPadding, m_monthPos.height * m_xCount);
					Rect view = new Rect (0f, 0f, m_monthPos.width * 0.25f, m_monthPos.height * m_monthField.Length);
					
					m_monthScrollPos = GUI.BeginScrollView (pos, m_monthScrollPos, view);
					
					int lastSelect = m_monthSelection;
					m_monthSelection = GUI.SelectionGrid(new Rect(0f, 0f, m_monthPos.width, m_monthPos.height * m_monthField.Length), 
					                                     m_monthSelection, m_monthField, 1, style);
					
					if(lastSelect != m_monthSelection) //month change 'event'(any other alternatives?)
					{
						UpdateDayField();
					}
					
					GUI.EndScrollView ();
				}
			}

			private void YearDropList()
			{
				GUIStyle style = new GUIStyle (GUI.skin.button);
				style.margin = new RectOffset (1, 1, 1, 1);
				style.alignment = TextAnchor.MiddleCenter;
				style.hover.textColor = Color.red;
				
				if(m_dropYearList)
				{
					Rect pos = new Rect(m_yearPos.x, m_yearPos.y + m_yearPos.height + m_dropPadding, m_yearPos.width + m_scrollPadding, m_yearPos.height * m_xCount);
					Rect view = new Rect (0f, 0f, m_yearPos.width * 0.25f, m_yearPos.height * m_yearField.Length);
					
					m_yearScrollPos = GUI.BeginScrollView (pos, m_yearScrollPos, view);
					
					int lastSelect = m_yearSelection;
					m_yearSelection = GUI.SelectionGrid(new Rect(0f, 0f, m_yearPos.width, m_yearPos.height * m_yearField.Length), 
					                                    m_yearSelection, m_yearField, 1, style);
					
					if(lastSelect != m_yearSelection) //month change 'event'(any other alternatives?)
					{
						UpdateDayField();
					}
					
					GUI.EndScrollView ();
				}
			}
			
			public void MakeToday()
			{
				m_daySelection = System.DateTime.Today.Day - 1; //index position
				m_monthSelection = System.DateTime.Today.Month - 1; //index position
				m_yearSelection = 0;

				m_dropDayList = false;
				m_dropMonthList = false;
				m_dropYearList = false;

				m_dayScrollPos = Vector2.zero;
				m_monthScrollPos = Vector2.zero;
				m_yearScrollPos = Vector2.zero;

				UpdateYearField ();
				UpdateMonthField ();
				UpdateDayField ();
			}
			
			public int YearRange
			{
				get { return m_yearRange; }
				set { m_yearRange = value > 0 ? value : 10; }
			}
			
			public string SelectedMonth
			{
				get { return System.DateTime.ParseExact (m_monthField [m_monthSelection], "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month.ToString(); }
			}
			
			public string SelectedYear
			{
				get { return m_yearField [m_yearSelection]; }
			}
			
			public string SelectedDay
			{
				get { return m_dayField [m_daySelection]; }
			}

			public string GetFormattedDate(char c, bool MDY = false)
			{
				if(!MDY)
				{
					return SelectedDay + c + SelectedMonth + c + SelectedYear;
				}
				return SelectedMonth + c + SelectedDay + SelectedYear;
			}
			
			public void UpdateYearField()
			{
				m_yearSelection = 0;
				m_yearField = new string[m_yearRange];
				
				for(int i=0; i<m_yearRange; i++)
				{
					m_yearField[i] = (System.DateTime.Today.Year - i).ToString();
				}
				
				m_yearSelection = Mathf.Clamp (m_yearSelection, 0, m_yearField.Length - 1);
			}
			
			private void UpdateDayField()
			{
				m_dayField = new string[System.DateTime.DaysInMonth (int.Parse(SelectedYear), int.Parse(SelectedMonth))];
				
				for(int i=0; i< m_dayField.Length; i++)
				{
					m_dayField[i] = (i + 1).ToString();
				}
				
				m_daySelection = Mathf.Clamp (m_daySelection, 0, m_dayField.Length - 1);
			}
			
			private void UpdateMonthField()
			{
				for(int i=0; i<12; i++)
				{
					m_monthField[i] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1);
				}
				
				m_monthSelection = Mathf.Clamp (m_monthSelection, 0, m_monthField.Length - 1);
			}
		}
	}
}

