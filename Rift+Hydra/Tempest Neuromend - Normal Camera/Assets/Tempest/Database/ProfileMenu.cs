using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Tempest
{
	namespace Menu
	{
		public class ProfileMenu
		{
			private const string k_loggedSessionFile = "session_log.xml";
			
			private string m_usernameField;
			private string m_passwordField;
			private int m_genderSelection;
			private string[] m_genderField;
			private CalendarView m_dobView;
			private TableView<Database.ReportDB.Report> m_statView;
			
			private TimedMessage m_feedback;
			
			private string m_dbServerField;
			private string m_dbPasswordField;
			private string m_dbDatabaseField;
			private string m_dbUserIDField;
			private string m_dbPoolingField;
			
			private Vector2 m_msgLogScrollView;
			
			private int m_maxUsernameLength = 15;
			private int m_minUsernameLength = 2;
			private int m_maxPasswordLength = 15;
			private int m_minPasswordLength = 5;
			
			private Database.TempestDB m_tempestDB = null;
			
			private GUIStyle m_feedbackStyle;
			private GUIStyle m_labelStyle; //label
			private GUIStyle m_textFieldStyle; //field
			private GUIStyle m_textAreaStyle;  //area
			private GUIStyle m_buttonStyle; //button
			private GUIStyle m_boxStyle; //box
			private GUIStyle m_statColumnStyle; //score table column
			private GUIStyle m_statRowStyle; //score table row
			
			private delegate void MenuFunction();
			private MenuFunction Callback;
			
			
			public void Initialize () 
			{
				if(m_tempestDB == null)
				{
					GameObject dbObj = GameObject.Find ("Database");
					
					if(dbObj != null)
					{
						m_tempestDB = dbObj.GetComponent<Database.TempestDB> ();
						m_feedback = new TimedMessage ();
						
						m_usernameField = "";
						m_passwordField = "";
						m_genderField = new string[] {"Male", "Female", "Other"};
						m_genderSelection = 0;
						m_dobView = new CalendarView (80);
						
						m_statView = new TableView<Database.ReportDB.Report> ();
						m_statView.AddColumn ("Device", 0.16f, (x, y) => (x.m_device.CompareTo(y.m_device)), (x, y) => (y.m_device.CompareTo(x.m_device)));
						m_statView.AddColumn ("Task", 0.16f, (x, y) => (x.m_task.CompareTo(y.m_task)), (x, y) => (y.m_task.CompareTo(x.m_task)));
						m_statView.AddColumn ("Date", 0.2f, (x, y) => (x.m_timestamp.CompareTo(y.m_timestamp)), (x, y) => (y.m_timestamp.CompareTo(x.m_timestamp)));
						m_statView.AddColumn ("Score", 0.16f,(x, y) => (x.m_score.CompareTo(y.m_score)), (x, y) => (y.m_score.CompareTo(x.m_score)));
						m_statView.AddColumn ("Sensitivity", 0.16f, (x, y) => (x.m_sens.CompareTo(y.m_sens)), (x, y) => (y.m_sens.CompareTo(x.m_sens)));
						m_statView.AddColumn ("Speed", 0.16f, (x, y) => (x.m_speed.CompareTo(y.m_speed)), (x, y) => (y.m_speed.CompareTo(x.m_speed)));
						
						m_dbServerField = "";
						m_dbUserIDField = "";
						m_dbDatabaseField = "";
						m_dbPasswordField = "";
						m_dbPoolingField = "";
						
						m_msgLogScrollView = Vector2.zero;
						
						Callback = Options;
					}
				}
			}
			
			private void ClearNonPersistantFields()
			{
				m_usernameField = "";
				m_passwordField = "";
				m_genderSelection = 0;
				m_dobView.MakeToday ();
				m_dobView.ResetScroll ();
				m_statView.ResetScroll ();
			}
			
			private bool VerifyAccountPolicy(string username, string password)
			{
				if(username.Length < m_minUsernameLength)
				{
					m_feedback.Begin("Error, minimum of " + m_minUsernameLength.ToString() + " character allowable for the username", 5.0f, m_feedbackStyle);
					return false;
				}
				
				if(password.Length < m_minPasswordLength)
				{
					m_feedback.Begin("Error, minimum of  " + m_minPasswordLength.ToString() + " characters allowable for the password", 5.0f, m_feedbackStyle);
					return false;
				}
				
				return true;
			}
			
			private void CreateProfile()
			{
				Rect rect1 = new Rect (Screen.width * 0.15f, Screen.height * 0.1f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.15f, Screen.height * 0.16f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect3 = new Rect (Screen.width * 0.15f, Screen.height * 0.22f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect4 = new Rect (Screen.width * 0.15f, Screen.height * 0.28f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect5 = new Rect (Screen.width * 0.3f, Screen.height * 0.1f, Screen.width * 0.14f, Screen.height * 0.05f);		
				Rect rect6 = new Rect (Screen.width * 0.3f, Screen.height * 0.16f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect7 = new Rect (Screen.width * 0.3f, Screen.height * 0.22f, Screen.width * 0.26f, Screen.height * 0.05f);
				Rect rect8 = new Rect (Screen.width * 0.3f, Screen.height * 0.28f, Screen.width * 0.11f, Screen.height * 0.05f);
				Rect rect9 = new Rect (Screen.width * 0.42f, Screen.height * 0.28f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect10 = new Rect (Screen.width * 0.55f, Screen.height * 0.28f, Screen.width * 0.11f, Screen.height * 0.05f);
				
				Rect rect11 = new Rect (Screen.width * 0.15f, Screen.height * 0.58f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect12 = new Rect (Screen.width * 0.3f, Screen.height * 0.58f, Screen.width * 0.37f, Screen.height * 0.05f);
				
				//enter details, submit 'form'.. if username does not exist
				
				GUI.Label (rect1, "USERNAME", m_labelStyle);
				GUI.Label (rect2, "PASSWORD", m_labelStyle);
				GUI.Label (rect3, "GENDER", m_labelStyle);
				GUI.Label (rect4, "D.O.B", m_labelStyle);
				
				//accept user details
				m_usernameField = GUI.TextField(rect5, m_usernameField, m_maxUsernameLength, m_textFieldStyle).Trim();
				m_passwordField = GUI.PasswordField(rect6, m_passwordField, '*', m_maxPasswordLength, m_textFieldStyle).Trim ();
				m_genderSelection = GUI.SelectionGrid (rect7, m_genderSelection, m_genderField, 3, m_buttonStyle);  
				
				//render date GUI and accept user date of birth
				m_dobView.m_dayPos = rect8;
				m_dobView.m_monthPos = rect9;
				m_dobView.m_yearPos = rect10;
				m_dobView.Display (m_buttonStyle);
				
				//trim username and password fields
				
				if(GUI.Button(rect11, "CREATE", m_buttonStyle))
				{
					if(m_tempestDB.IsConnected)
					{
						if(VerifyAccountPolicy(m_usernameField, m_passwordField))
						{
							if(m_tempestDB.ProfileDatabase.AddPatient(m_usernameField, m_passwordField, m_dobView.GetFormattedNumericDate ('/'), m_genderField[m_genderSelection]))
							{
								m_feedback.Begin("Profile created", 5.0f, m_feedbackStyle);
							}
							else
							{
								m_feedback.Begin("Profile with username already exists", 5.0f, m_feedbackStyle);
							}
						}
					}
					else
					{
						m_feedback.Begin("Not connected to server", 5.0f, m_feedbackStyle);
					}
				}
				
				GoBack (Options, rect12, m_buttonStyle);
				
				ServerSettings ();
			}
			
			private void ViewStats()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					//set styles for id and score column
					
					m_statView.GetColumn("Score").SetStyles(m_statColumnStyle, m_statRowStyle);
					m_statView.GetColumn("Sensitivity").SetStyles(m_statColumnStyle, m_statRowStyle);
					m_statView.GetColumn("Speed").SetStyles(m_statColumnStyle, m_statRowStyle);
					m_statView.GetColumn("Device").SetStyles(m_statColumnStyle, m_statRowStyle);
					m_statView.GetColumn("Task").SetStyles(m_statColumnStyle, m_statRowStyle);
					m_statView.GetColumn("Date").SetStyles(m_statColumnStyle, m_statRowStyle);
					
					//set style for entire table
					m_statView.ViewStyle = m_boxStyle;
					
					m_statView.Width = Screen.width * 0.9f;
					m_statView.Height = Screen.height * 0.5f;
					m_statView.Position = new Vector2(Screen.width * 0.05f, Screen.height * 0.05f);
					
					m_statView.Display();
				}
				
				Rect rect1 = new Rect (Screen.width * 0.05f, Screen.height * 0.56f, Screen.width * 0.44f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.5f, Screen.height * 0.56f, Screen.width * 0.44f, Screen.height * 0.05f);
				
				if(GUI.Button(rect1, "EXPORT CSV", m_buttonStyle))
				{
					string path = "ProfileData";
					string file = m_tempestDB.Profile.Value.m_username + ".csv";
					
					if(m_tempestDB.ReportDatabase.ExtractReport(m_tempestDB.Profile.Value.m_username, path, file))
					{
						m_feedback.Begin("Profile data has been exported to .csv file", 5.0f, m_feedbackStyle);
					}
					else
					{
						m_feedback.Begin("Error exporting profile data to .csv file, check if file is currently open", 5.0f, m_feedbackStyle);
					}
				}
				
				GoBack (ViewProfile, rect2, m_buttonStyle);
			}
			
			
			private void ViewProfile()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					Rect rect1 = new Rect (Screen.width * 0.3f, Screen.height * 0.1f, Screen.width * 0.14f, Screen.height * 0.05f);
					Rect rect2 = new Rect (Screen.width * 0.3f, Screen.height * 0.16f, Screen.width * 0.14f, Screen.height * 0.05f);
					Rect rect3 = new Rect (Screen.width * 0.3f, Screen.height * 0.22f, Screen.width * 0.14f, Screen.height * 0.05f);
					
					Rect rect5 = new Rect (Screen.width * 0.45f, Screen.height * 0.1f, Screen.width * 0.14f, Screen.height * 0.05f);		
					Rect rect6 = new Rect (Screen.width * 0.45f, Screen.height * 0.16f, Screen.width * 0.14f, Screen.height * 0.05f);
					Rect rect7 = new Rect (Screen.width * 0.45f, Screen.height * 0.22f, Screen.width * 0.14f, Screen.height * 0.05f);
					
					Rect rect8 = new Rect (Screen.width * 0.3f, Screen.height * 0.28f, Screen.width * 0.14f, Screen.height * 0.05f);
					Rect rect9 = new Rect (Screen.width * 0.45f, Screen.height * 0.28f, Screen.width * 0.14f, Screen.height * 0.05f);
					Rect rect11 = new Rect (Screen.width * 0.3f, Screen.height * 0.34f, Screen.width * 0.29f, Screen.height * 0.05f);
					
					//viewing of profile
					GUI.Label (rect1, "Username", m_labelStyle);
					GUI.Label (rect2, "Date Of Birth", m_labelStyle);
					GUI.Label (rect3, "Gender", m_labelStyle);
					
					//get user details
					string gender = m_tempestDB.Profile.Value.m_gender;
					string dob = m_tempestDB.Profile.Value.m_birthDate;
					string username = m_tempestDB.Profile.Value.m_username;
					
					GUI.Label(rect5, username, m_textFieldStyle);
					GUI.Label(rect6, dob, m_textFieldStyle);
					GUI.Label(rect7, gender, m_textFieldStyle);  
					
					//deletion of profile
					if(GUI.Button(rect8, "STATS", m_buttonStyle))
					{
						//retrieve all scores from the database belonging to this user
						LoadScores();
						
						Callback = ViewStats;
					}
					
					else if(GUI.Button(rect9, "DELETE", m_buttonStyle))
					{
						Callback = DeleteProfile;
					}	
					
					GoBack(Options, rect11, m_buttonStyle);
				}		
				
				ServerSettings ();
			}
			
			private void LoadScores()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					m_statView.DropItems ();
					m_tempestDB.ReportDatabase.ExtractReport(m_tempestDB.Profile.Value.m_username, m_statView.Items);
				}
			}
			
			private void LoginProfile()
			{
				Rect rect1 = new Rect (Screen.width * 0.3f, Screen.height * 0.2f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.45f, Screen.height * 0.2f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect3 = new Rect (Screen.width * 0.3f, Screen.height * 0.26f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect4 = new Rect (Screen.width * 0.45f, Screen.height * 0.26f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect5 = new Rect (Screen.width * 0.3f, Screen.height * 0.32f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect6 = new Rect (Screen.width * 0.45f, Screen.height * 0.32f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				GUI.Label (rect1, "USERNAME", m_labelStyle);
				GUI.Label (rect3, "PASSWORD", m_labelStyle);
				
				m_usernameField = GUI.TextField(rect2, m_usernameField, m_maxUsernameLength, m_textFieldStyle).Trim ();
				m_passwordField = GUI.PasswordField(rect4, m_passwordField, '*', m_maxPasswordLength, m_textFieldStyle).Trim ();
				
				if(GUI.Button(rect6, "LOGIN", m_buttonStyle))
				{
					if(m_tempestDB.IsConnected)
					{
						//clear whatever was left from before
						
						//display feedback
						Database.PatientDB.Patient profile = new Database.PatientDB.Patient();	
						
						if(m_tempestDB.ProfileDatabase.ExtractPatient (m_usernameField, m_passwordField, ref profile))
						{
							m_tempestDB.Profile = profile;
							
							//create new session so user can restore to this later on(if they don't log out)
							SaveLoggedSession(false);
							m_statView.DropItems();
							ClearNonPersistantFields();
							Callback = Options; 
							
							m_feedback.Begin("Profile successfully loaded", 5.0f, m_feedbackStyle);
						}
						else 
						{
							m_feedback.Begin("Profile loading failed", 5.0f, m_feedbackStyle);
						}
					}
					else
					{
						m_feedback.Begin("Not connected to the server", 5.0f, m_feedbackStyle);
					}
				}
				
				GoBack (Options, rect5, m_buttonStyle);
				ServerSettings ();
			}
			
			private void GoBack(MenuFunction func, Rect rect, GUIStyle style)
			{
				//convinient go back function 
				if(GUI.Button (rect, "BACK", style))
				{
					ClearNonPersistantFields(); 
					Callback = func; //back to main menu
				}
			}
			
			private void ServerSettings()
			{
				Rect rect1 = new Rect (Screen.width * 0.7f, Screen.height * 0.05f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.85f, Screen.height * 0.05f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect3 = new Rect (Screen.width * 0.7f, Screen.height * 0.11f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect4 = new Rect (Screen.width * 0.85f, Screen.height * 0.11f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect5 = new Rect (Screen.width * 0.7f, Screen.height * 0.17f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect6 = new Rect (Screen.width * 0.85f, Screen.height * 0.17f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect7 = new Rect (Screen.width * 0.7f, Screen.height * 0.23f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect8 = new Rect (Screen.width * 0.85f, Screen.height * 0.23f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect9 = new Rect (Screen.width * 0.7f, Screen.height * 0.29f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect10 = new Rect (Screen.width * 0.85f, Screen.height * 0.29f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				Rect rect11 = new Rect (Screen.width * 0.7f, Screen.height * 0.35f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect12 = new Rect (Screen.width * 0.85f, Screen.height * 0.35f, Screen.width * 0.14f, Screen.height * 0.05f);
				
				GUI.Label(rect1, "STATUS", m_labelStyle);
				
				Color prevColor = m_labelStyle.normal.textColor;
				
				if(m_tempestDB.IsConnected)
				{
					m_labelStyle.normal.textColor = Color.green;
					GUI.Label(rect2, "ONLINE", m_labelStyle);
				}
				else
				{
					m_labelStyle.normal.textColor = Color.red;
					GUI.Label(rect2, "OFFLINE", m_labelStyle);
				}
				
				m_labelStyle.normal.textColor = prevColor;
				
				GUI.Label(rect3, "SERVER", m_labelStyle);
				m_dbServerField = GUI.TextField(rect4, m_dbServerField, m_textFieldStyle);
				
				GUI.Label(rect5, "DATABASE", m_labelStyle);
				m_dbDatabaseField = GUI.TextField(rect6, m_dbDatabaseField, m_textFieldStyle);
				
				GUI.Label(rect7, "USER ID", m_labelStyle);
				m_dbUserIDField = GUI.TextField(rect8, m_dbUserIDField, m_textFieldStyle);
				
				GUI.Label(rect9, "PASSWORD", m_labelStyle);
				m_dbPasswordField = GUI.PasswordField(rect10, m_dbPasswordField, '*', m_textFieldStyle);
				
				if(GUI.Button (rect11, "DEFAULT", m_buttonStyle))
				{
					string[] tokens = m_tempestDB.GetDefaultServer().Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);
					
					m_dbServerField = tokens[0].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbDatabaseField = tokens[1].Split (new string[]{"="} , System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbUserIDField = tokens[2].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPasswordField = tokens[3].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPoolingField = tokens[4].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];     
				}
				
				if(GUI.Button (rect12, "CONNECT", m_buttonStyle))
				{
					string config = "Server=" + m_dbServerField + ";" +
						"Database=" + m_dbDatabaseField + ";" +
							"User ID=" + m_dbUserIDField + ";" +
							"Password=" + m_dbPasswordField + ";" + 
							"Pooling=" + m_dbPoolingField + ";";
					
					m_tempestDB.Reconnect(config);
					
					if(m_tempestDB.IsConnected)
					{
						//if could not load a logged session, then set to not logged in(also a sign that the database is not the same)
						if(!LoadLoggedSession())
						{
							m_tempestDB.Profile = null;
						}
						m_feedback.Begin("Connected", 5.0f, m_feedbackStyle); 
					}
					else
					{
						m_tempestDB.Profile = null;
						m_feedback.Begin("Connection attempt failed", 5.0f, m_feedbackStyle); 
					}
					
					Callback = Options;
					ClearNonPersistantFields();
				}
				
				UpdateFeedback ();
			}
			
			private void SetupStyles()
			{
				//int fontSize = (int)Mathf.Min (Screen.width, Screen.height) / 50;
				int fontSize = (int)(Screen.width/50.0f);
				m_boxStyle = new GUIStyle (GUI.skin.box);
				m_boxStyle.fontSize = fontSize;
				m_boxStyle.fontStyle = FontStyle.Bold;
				m_boxStyle.normal.textColor = Color.black;
				m_boxStyle.alignment = TextAnchor.UpperLeft;
				
				m_statColumnStyle = new GUIStyle(GUI.skin.label); //score table column
				m_statColumnStyle.fontSize = fontSize;
				m_statColumnStyle.alignment = TextAnchor.MiddleCenter;
				m_statColumnStyle.normal.textColor = Color.black;
				m_statColumnStyle.fontStyle = FontStyle.Bold;
				
				m_statRowStyle = new GUIStyle(GUI.skin.label); //score table row
				m_statRowStyle.fontSize = fontSize;
				m_statRowStyle.alignment = TextAnchor.MiddleCenter;
				m_statRowStyle.normal.textColor = Color.black;
				m_statRowStyle.fontStyle = FontStyle.Bold;
				
				
				m_labelStyle = new GUIStyle (GUI.skin.label);
				m_labelStyle.fontSize = fontSize;
				m_labelStyle.alignment = TextAnchor.MiddleCenter;
				m_labelStyle.normal.textColor = Color.black;
				m_labelStyle.fontStyle = FontStyle.Bold;
				
				m_textFieldStyle = new GUIStyle (GUI.skin.textField);
				m_textFieldStyle.fontSize = fontSize;
				m_textFieldStyle.alignment = TextAnchor.MiddleLeft;
				
				m_textAreaStyle = new GUIStyle (GUI.skin.textArea);
				m_textAreaStyle.fontSize = fontSize;
				m_textAreaStyle.alignment = TextAnchor.UpperLeft;
				
				m_buttonStyle = new GUIStyle (GUI.skin.button);
				m_buttonStyle.fontSize = fontSize;
				m_buttonStyle.alignment = TextAnchor.MiddleCenter;
				m_buttonStyle.normal.textColor = Color.black;
				m_buttonStyle.fontStyle = FontStyle.Bold;
				
				m_feedbackStyle = new GUIStyle(GUI.skin.label);
				m_feedbackStyle.alignment = TextAnchor.UpperLeft;
				m_feedbackStyle.wordWrap = true;
				m_feedbackStyle.fontSize = fontSize;
				m_feedbackStyle.normal.textColor = Color.black;
			}
			
			private void Options()
			{
				Rect rect1 = new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.2f, Screen.height * 0.05f, Screen.width * 0.16f, Screen.height * 0.05f);
				Rect rect3 = new Rect (Screen.width * 0.37f, Screen.height * 0.05f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect4 = new Rect (Screen.width * 0.5f, Screen.height * 0.05f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect5 = new Rect (Screen.width * 0.05f, Screen.height * 0.11f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect6 = new Rect (Screen.width * 0.2f, Screen.height * 0.11f, Screen.width * 0.16f, Screen.height * 0.05f);
				
				GUI.Label (rect1, "LOGGED IN:", m_labelStyle);
				
				Color prevColor = m_textFieldStyle.normal.textColor;
				
				if(!m_tempestDB.Profile.HasValue)
				{
					m_textFieldStyle.normal.textColor = Color.red;
					GUI.Label (rect2, "NO PROFILE", m_textFieldStyle);
				}
				else
				{
					m_textFieldStyle.normal.textColor = Color.green;
					GUI.Label (rect2, m_tempestDB.Profile.Value.m_username, m_textFieldStyle);
				}
				
				m_textFieldStyle.normal.textColor = prevColor;
				
				if(GUI.Button (rect3, "LOGIN", m_buttonStyle)) 
				{
					Callback = LoginProfile;
				}
				
				else if(GUI.Button (rect4, "LOGOUT", m_buttonStyle)) 
				{
					LogoutProfile();
				}
				
				else if(GUI.Button (rect5, "CREATE NEW", m_buttonStyle))
				{
					Callback = CreateProfile;
				}
				
				else if(GUI.Button (rect6, "VIEW PROFILE", m_buttonStyle)) 
				{
					if(!m_tempestDB.Profile.HasValue)
					{
						m_feedback.Begin("No profile loaded for viewing", 5.0f, m_feedbackStyle);
					}
					else
					{
						Callback = ViewProfile;
					}
				}
				
				ServerSettings ();
			}
			
			private void LogoutProfile()
			{
				if(SaveLoggedSession(true))
				{
					m_feedback.Begin("Logged out of session", 5.0f, m_feedbackStyle);
				}
				else
				{
					m_feedback.Begin("No session currently logged into", 5.0f, m_feedbackStyle);
				}
			}
			
			private void DeleteProfile()
			{
				Rect rect1 = new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.35f, Screen.height * 0.1f);
				Rect rect2 = new Rect (Screen.width * 0.2f, Screen.height * 0.31f, Screen.width * 0.17f, Screen.height * 0.05f);
				Rect rect3 = new Rect (Screen.width * 0.38f, Screen.height * 0.31f, Screen.width * 0.17f, Screen.height * 0.05f);
				
				GUI.Label (rect1, "ARE YOU SURE ?", m_labelStyle);
				
				if(GUI.Button(rect2, "YES", m_buttonStyle))
				{
					if(m_tempestDB.IsConnected)
					{

						string password = Utils.Encryptor.Decrypt(m_tempestDB.Profile.Value.m_encryptedPassword, m_tempestDB.Profile.Value.m_salt);

						if(m_tempestDB.ProfileDatabase.DeletePatient(m_tempestDB.Profile.Value.m_username, password))
						{
							m_tempestDB.Profile = null;
							DeleteLoggedSession();
							ClearNonPersistantFields();
		
							m_feedback.Begin("Your profile has been successfully deleted", 5.0f, m_feedbackStyle);
							Callback = Options;
							
						}
						else
						{
							m_feedback.Begin("Error has occured attempting to delete this profile", 5.0f, m_feedbackStyle);
						}
					}
					else
					{
						m_feedback.Begin("Not connected to the server", 5.0f, m_feedbackStyle);
					}
				}
				else if(GUI.Button(rect3, "NO", m_buttonStyle))
				{
					Callback = ViewProfile;
				} 
				
				ServerSettings ();
			}
			
			private void UpdateFeedback()
			{
				Rect pos = new Rect (Screen.width * 0.7f, Screen.height * 0.43f, Screen.width * 0.29f, Screen.height * 0.2f);
				Rect view = new Rect (0.0f, 0.0f, pos.width * 2.0f, pos.height * 2.0f);
				
				m_msgLogScrollView = GUI.BeginScrollView (pos, m_msgLogScrollView, view, false, false);
				
				GUI.Box (view, "Message Log", m_boxStyle);
				
				//GUI.Label (new Rect (Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.2f, Screen.height * 0.05f), "<Message Log>", m_feedbackStyle);
				m_feedback.Display (new Rect (Screen.width * 0.01f, Screen.height * 0.045f, Screen.width * 0.2f, Screen.height * 0.1f));
				
				GUI.EndScrollView ();
			}
			
			public void Draw()
			{
				SetupStyles ();
				Callback ();
			}

			public void DeleteLoggedSession()
			{
				if(System.IO.File.Exists(k_loggedSessionFile))
				{
					System.IO.File.Delete (k_loggedSessionFile);
				}
			}
			
			public bool SaveLoggedSession(bool logout)
			{
				if(m_tempestDB.Profile.HasValue)
				{
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					XmlWriter writer =  XmlWriter.Create(@k_loggedSessionFile, settings);
					
					writer.WriteStartDocument();
					writer.WriteStartElement("Session");
					writer.WriteElementString("Logout", logout.ToString());
					writer.WriteElementString("Database", m_tempestDB.Database); 
					writer.WriteElementString("Access-Time", System.DateTime.Now.ToLongDateString());

					writer.WriteStartElement("Information");
					writer.WriteAttributeString("Username", m_tempestDB.Profile.Value.m_username);
					writer.WriteAttributeString("Password", m_tempestDB.Profile.Value.m_encryptedPassword);
					writer.WriteAttributeString("Salt", m_tempestDB.Profile.Value.m_salt);
					writer.WriteEndElement();

					writer.WriteEndElement();
					writer.WriteEndDocument();
					
					writer.Flush();
					writer.Close();
					
					if(logout)
					{
						m_tempestDB.Profile = null;
					}
					
					return true;
				}
				return false;
			}
			
			public bool LoadLoggedSession()
			{
				if(m_tempestDB.IsConnected && System.IO.File.Exists(@k_loggedSessionFile))
				{
					XElement root =  XElement.Load(@k_loggedSessionFile);
					
					if(root != null)
					{
						if(root.Element("Database").Value == m_tempestDB.Database)
						{
							bool logout = false;
							bool.TryParse(root.Element("Logout").Value, out logout);
							
							if(!logout)
							{
								XElement info = root.Element("Information");
								string username = info.Attribute("Username").Value;
								string password = Utils.Encryptor.Decrypt(info.Attribute("Password").Value,
								                                          info.Attribute("Salt").Value);
								
								Database.PatientDB.Patient patient = new Database.PatientDB.Patient();
								
								if(m_tempestDB.ProfileDatabase.ExtractPatient(username, password, ref patient))
								{
									m_tempestDB.Profile = patient;
									return true;
								}
							}//end if
						}//end if
					}//end if
				}
				return false;
			}
			
			
		}
	}
}
