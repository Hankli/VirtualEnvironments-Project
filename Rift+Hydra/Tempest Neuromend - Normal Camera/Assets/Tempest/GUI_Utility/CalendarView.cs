using UnityEngine;

namespace Tempest
{
	namespace Menu
	{
		public class CalendarView 
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

			private bool m_dayScrollEvent;
			private bool m_monthScrollEvent;
			private bool m_yearScrollEvent;

			public Rect m_dayPos;
			public Rect m_monthPos;
			public Rect m_yearPos;

			public int m_xCount;
			public int m_scrollPadding;
			public int m_dropPadding;
			
			public CalendarView(int yearRange)
			{
				m_dropDayList = false;
				m_dropYearList = false;
				m_dropMonthList = false;

				m_xCount = 4;
				m_scrollPadding = 15;
				m_dropPadding = 3;
				
				YearRange = yearRange;
				
				MakeToday ();
			}

			public void ResetScroll()
			{
				m_dayScrollPos = Vector2.zero;
				m_monthScrollPos = Vector2.zero;
				m_yearScrollPos = Vector2.zero;
			}

			
			public void Display(GUIStyle style)
			{
				if(GUI.Button(m_dayPos, SelectedDay, style))
				{
					m_dropDayList = !m_dropDayList;
					m_dropMonthList = false;
					m_dropYearList = false;
				}
				else if(GUI.Button(m_monthPos, SelectedMonth, style))
				{
					m_dropDayList = false;
					m_dropMonthList = !m_dropMonthList;
					m_dropYearList = false;
				}
				else if(GUI.Button(m_yearPos, SelectedYear, style))
				{
					m_dropDayList = false;
					m_dropMonthList = false;
					m_dropYearList = !m_dropYearList;
				}

				DayDropList (style);
				MonthDropList (style);
				YearDropList (style);
			}
			
			private void DayDropList(GUIStyle style)
			{
				if(m_dropDayList)
				{	
					Rect pos = new Rect(m_dayPos.x, m_dayPos.y + m_dayPos.height + m_dropPadding, m_dayPos.width + m_scrollPadding, m_dayPos.height * m_xCount);
					Rect view = new Rect (0f, 0f, m_dayPos.width * 0.25f, m_dayPos.height * m_dayField.Length);
					
					m_dayScrollPos = GUI.BeginScrollView (pos, m_dayScrollPos, view);

					int lastSelect = m_daySelection;
					m_daySelection = GUI.SelectionGrid(new Rect(0f, 0f, m_dayPos.width, m_dayPos.height * m_dayField.Length), 
					                                   m_daySelection, m_dayField, 1, style);

					if(lastSelect != m_daySelection)
					{
						m_dropDayList = false;
					}

					GUI.EndScrollView ();
				}
			}
			
			private void MonthDropList(GUIStyle style)
			{
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
						m_dropMonthList = false;
					}
					
					GUI.EndScrollView ();
				}
			}

			private void YearDropList(GUIStyle style)
			{
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
						m_dropYearList = false;
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

			public string SelectedMonthNumeric
			{
				get { return System.DateTime.ParseExact (m_monthField [m_monthSelection], "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month.ToString(); }
			}

			public int YearRange
			{
				get { return m_yearRange; }
				set { m_yearRange = value > 0 ? value : 10; }
			}

			public string SelectedMonth
			{
				get { return m_monthField [m_monthSelection]; } 
			}
			
			public string SelectedYear
			{
				get { return m_yearField [m_yearSelection]; }
			}
			
			public string SelectedDay
			{
				get { return m_dayField [m_daySelection]; }
			}

			public string GetFormattedNumericDate(char c, bool MDY = false)
			{
				if(!MDY)
				{
					return SelectedDay + c + SelectedMonthNumeric + c + SelectedYear;
				}
				return SelectedMonthNumeric + c + SelectedDay + SelectedYear;
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

				m_dayField = new string[System.DateTime.DaysInMonth (int.Parse(SelectedYear), int.Parse(SelectedMonthNumeric))];
				
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

