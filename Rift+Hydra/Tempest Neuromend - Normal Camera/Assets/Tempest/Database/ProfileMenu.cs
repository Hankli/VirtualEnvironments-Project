using UnityEngine;
using System.Collections.Generic;

namespace Tempest
{
	namespace Menu
	{
		public class ProfileMenu : MonoBehaviour
		{
			private string m_usernameField;
			private string m_passwordField;
			private int m_genderSelection;
			private string[] m_genderField;
			private CalendarView m_dobView;
			private TableView<Database.ReportDB.Report> m_statView;
			
			private string m_dbServerField;
			private string m_dbPasswordField;
			private string m_dbDatabaseField;
			private string m_dbUserIDField;
			private string m_dbPoolingField;
			
			private Vector2 m_msgLogScrollView;
			byte[] m_saltedPassword;
			
			private int m_maxUsernameLength = 15;
			private int m_minUsernameLength = 2;
			private int m_maxPasswordLength = 15;
			private int m_minPasswordLength = 5;
			
			private TimedMessage m_feedback;
			
			private Database.TempestDB m_tempestDB = null;
			
			private int m_time;
			private string m_message;

			private GUIStyle m_feedbackStyle;
			private GUIStyle s1; //label
			private GUIStyle s2; //field
			private GUIStyle s3;  //area
			private GUIStyle s4; //button

			private delegate void MenuFunction();
			private MenuFunction Callback;
			
			// Use this for initialization

			public void BackToBeginning()
			{
				Callback = Options;
				ClearNonPersistantFields ();
			}

			public void Start()
			{
				Initialize ();
			}

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

				Rect rect11 = new Rect (Screen.width * 0.15f, Screen.height * 0.4f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect12 = new Rect (Screen.width * 0.3f, Screen.height * 0.4f, Screen.width * 0.12f, Screen.height * 0.05f);

				//enter details, submit 'form'.. if username does not exist
				
				GUI.Label (rect1, "USERNAME", s1);
				GUI.Label (rect2, "PASSWORD", s1);
				GUI.Label (rect3, "GENDER", s1);
				GUI.Label (rect4, "DATE OF BIRTH", s1);
				
				//accept user details
				m_usernameField = GUI.TextField(rect5, m_usernameField, m_maxUsernameLength, s2).Trim();
				m_passwordField = GUI.PasswordField(rect6, m_passwordField, '*', m_maxPasswordLength, s2).Trim ();
				m_genderSelection = GUI.SelectionGrid (rect7, m_genderSelection, m_genderField, 3, s4);  
				
				//render date GUI and accept user date of birth
				m_dobView.m_dayPos = rect8;
				m_dobView.m_monthPos = rect9;
				m_dobView.m_yearPos = rect10;
				m_dobView.Display (s4);
				
				//trim username and password fields

				if(GUI.Button(rect11, "CREATE", s4))
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

				GoBack (Options, rect12, s4);

				ServerSettings ();
			}
			
			private void ViewStats()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					//set styles for id and score column
					GUIStyle tupleStyle = new GUIStyle(GUI.skin.label);
					GUIStyle columnStyle = new GUIStyle(GUI.skin.label);

					m_statView.GetColumn("Score").SetStyles(columnStyle, s1);
					m_statView.GetColumn("Sensitivity").SetStyles(columnStyle, s1);
					m_statView.GetColumn("Speed").SetStyles(columnStyle, s1);

					//set styles for device, task and date columns
					tupleStyle = s1;
					columnStyle = s1;

					m_statView.GetColumn("Device").SetStyles(columnStyle, s1);
					m_statView.GetColumn("Task").SetStyles(columnStyle, s1);
					m_statView.GetColumn("Date").SetStyles(columnStyle, s1);
					
					//set style for entire table
					GUIStyle viewStyle = new GUIStyle(GUI.skin.box);
					viewStyle.alignment = TextAnchor.UpperCenter;
					m_statView.ViewStyle = viewStyle;
					
					m_statView.Width = Screen.width * 0.9f;
					m_statView.Height = Screen.height * 0.5f;
					m_statView.Position = new Vector2(Screen.width * 0.05f, Screen.height * 0.05f);
					
					m_statView.Display();
				}

				Rect rect1 = new Rect (Screen.width * 0.05f, Screen.height * 0.56f, Screen.width * 0.44f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.5f, Screen.height * 0.56f, Screen.width * 0.44f, Screen.height * 0.05f);
				
				if(GUI.Button(rect1, "EXPORT CSV", s4))
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

				GoBack (ViewProfile, rect2, s4);
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
					GUI.Label (rect1, "Username", s1);
					GUI.Label (rect2, "Date Of Birth", s1);
					GUI.Label (rect3, "Gender", s1);
					
					//get user details
					string gender = m_tempestDB.Profile.Value.m_gender;
					string dob = m_tempestDB.Profile.Value.m_birthDate;
					string username = m_tempestDB.Profile.Value.m_username;
					
					GUI.Label(rect5, username, s2);
					GUI.Label(rect6, dob, s2);
					GUI.Label(rect7, gender, s2);  
					
					//deletion of profile
					if(GUI.Button(rect8, "STATS", s4))
					{
						//retrieve all scores from the database belonging to this user
						LoadScores();

						Callback = ViewStats;
					}

					else if(GUI.Button(rect9, "DELETE", s4))
					{
						if(m_tempestDB.ProfileDatabase.DeletePatient (m_tempestDB.Profile.Value.m_username, m_tempestDB.Profile.Value.m_password))
						{
							m_tempestDB.Profile = null;
							
							m_feedback.Begin("Profile successfully deleted", 5.0f, m_feedbackStyle);
						}
						else
						{						
							m_feedback.Begin("Profile deletion failed", 5.0f, m_feedbackStyle);
						}
					}	

					GoBack(Options, rect11, s4);
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
				
				GUI.Label (rect1, "USERNAME", s1);
				GUI.Label (rect3, "PASSWORD", s1);
				
				m_usernameField = GUI.TextField(rect2, m_usernameField, m_maxUsernameLength, s2).Trim ();
				m_passwordField = GUI.PasswordField(rect4, m_passwordField, '*', m_maxPasswordLength, s2).Trim ();

				if(GUI.Button(rect6, "LOGIN", s4))
				{
					if(m_tempestDB.IsConnected)
					{
						//clear whatever was left from before
						m_statView.DropItems();
				
						//display feedback
						Database.PatientDB.Patient profile = new Database.PatientDB.Patient();	

						if(m_tempestDB.ProfileDatabase.ExtractPatient (m_usernameField, m_passwordField, ref profile))
						{
							m_tempestDB.Profile = profile;

							//create new session so user can restore to this later on(if they don't log out)
							m_tempestDB.SaveLoggedSession(false);

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
				
				GoBack (Options, rect5, s4);

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
				
				GUI.Label(rect1, "STATUS", s1);

				Color prevColor = s1.normal.textColor;

				if(m_tempestDB.IsConnected)
				{
					s1.normal.textColor = Color.green;
					GUI.Label(rect2, "ONLINE", s1);
				}
				else
				{
					s1.normal.textColor = Color.red;
					GUI.Label(rect2, "OFFLINE", s1);
				}

				s1.normal.textColor = prevColor;

				GUI.Label(rect3, "SERVER", s1);
				m_dbServerField = GUI.TextField(rect4, m_dbServerField, s2);

				GUI.Label(rect5, "DATABASE", s1);
				m_dbDatabaseField = GUI.TextField(rect6, m_dbDatabaseField, s2);
		
				GUI.Label(rect7, "USER ID", s1);
				m_dbUserIDField = GUI.TextField(rect8, m_dbUserIDField, s2);
		
				GUI.Label(rect9, "PASSWORD", s1);
				m_dbPasswordField = GUI.PasswordField(rect10, m_dbPasswordField, '*', s2);

				if(GUI.Button (rect11, "DEFAULT", s4))
				{
					string[] tokens = m_tempestDB.GetDefaultServer().Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);
							
					m_dbServerField = tokens[0].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbDatabaseField = tokens[1].Split (new string[]{"="} , System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbUserIDField = tokens[2].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPasswordField = tokens[3].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPoolingField = tokens[4].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];     
				}
						
				if(GUI.Button (rect12, "CONNECT", s4))
				{
					string config = "Server=" + m_dbServerField + ";" +
							"Database=" + m_dbDatabaseField + ";" +
							"User ID=" + m_dbUserIDField + ";" +
							"Password=" + m_dbPasswordField + ";" + 
							"Pooling=" + m_dbPoolingField + ";";
							
					m_tempestDB.Reconnect(config);
							
					if(m_tempestDB.IsConnected)
					{
						Callback = Options;
						ClearNonPersistantFields();
						m_tempestDB.LoadLoggedSession();
								
						m_feedback.Begin("Connected", 5.0f, m_feedbackStyle); 
					}
					else
					{
						m_feedback.Begin("Connection attempt failed", 5.0f, m_feedbackStyle); 
					}
				}

				UpdateFeedback ();
			}
			
			private void SetupStyles()
			{
				int fontSize = (int)Mathf.Min (Screen.width, Screen.height) / 48;

				s1 = new GUIStyle (GUI.skin.label);
				s1.fontSize = fontSize;
				s1.alignment = TextAnchor.MiddleCenter;
				
				s2 = new GUIStyle (GUI.skin.textField);
				s2.fontSize = fontSize;
				s2.alignment = TextAnchor.MiddleLeft;
				
				s3 = new GUIStyle (GUI.skin.textArea);
				s3.fontSize = fontSize;
				s3.alignment = TextAnchor.UpperLeft;
				
				s4 = new GUIStyle (GUI.skin.button);
				s4.fontSize = fontSize;
				s4.alignment = TextAnchor.MiddleCenter;

				m_feedbackStyle = new GUIStyle(GUI.skin.label);
				m_feedbackStyle.alignment = TextAnchor.UpperLeft;
				m_feedbackStyle.wordWrap = true;
				m_feedbackStyle.fontSize = fontSize;
			}

			private void Options()
			{
				Rect rect1 = new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect2 = new Rect (Screen.width * 0.2f, Screen.height * 0.05f, Screen.width * 0.16f, Screen.height * 0.05f);
				Rect rect3 = new Rect (Screen.width * 0.37f, Screen.height * 0.05f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect4 = new Rect (Screen.width * 0.5f, Screen.height * 0.05f, Screen.width * 0.12f, Screen.height * 0.05f);
				Rect rect5 = new Rect (Screen.width * 0.05f, Screen.height * 0.11f, Screen.width * 0.14f, Screen.height * 0.05f);
				Rect rect6 = new Rect (Screen.width * 0.2f, Screen.height * 0.11f, Screen.width * 0.16f, Screen.height * 0.05f);
				
				GUI.Label (rect1, "LOGGED IN:", s1);

				Color prevColor = s2.normal.textColor;

				if(!m_tempestDB.Profile.HasValue)
				{
					s2.normal.textColor = Color.red;
					GUI.Label (rect2, "NO PROFILE", s2);
				}
				else
				{
					s2.normal.textColor = Color.green;
					GUI.Label (rect2, m_tempestDB.Profile.Value.m_username, s2);
				}

				s2.normal.textColor = prevColor;

				if(GUI.Button (rect3, "LOGIN", s4)) 
				{
					Callback = LoginProfile;
				}

				else if(GUI.Button (rect4, "LOGOUT", s4)) 
				{
					LogoutProfile();
				}

				else if(GUI.Button (rect5, "CREATE NEW", s4))
				{
					Callback = CreateProfile;
				}

				else if(GUI.Button (rect6, "VIEW PROFILE", s4)) 
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
				if(m_tempestDB.SaveLoggedSession(true))
				{
					m_feedback.Begin("Logged out of session", 5.0f, m_feedbackStyle);
				}
				else
				{
					m_feedback.Begin("No session currently logged into", 5.0f, m_feedbackStyle);
				}
			}
			
			private void UpdateFeedback()
			{
				Rect pos = new Rect (Screen.width * 0.7f, Screen.height * 0.41f, Screen.width * 0.29f, Screen.height * 0.15f);
				Rect view = new Rect (0.0f, 0.0f, pos.width, pos.height);

				m_msgLogScrollView = GUI.BeginScrollView (pos, m_msgLogScrollView, view, false, false);
						
				GUI.Box (view, "", GUI.skin.textArea);

				GUI.Label (new Rect (Screen.width * 0.01f, Screen.height * 0.01f, Screen.width * 0.2f, Screen.height * 0.05f), "<Message Log>", m_feedbackStyle);
			    m_feedback.Display (new Rect (Screen.width * 0.01f, Screen.height * 0.045f, Screen.width * 0.2f, Screen.height * 0.1f));
				
				GUI.EndScrollView ();
			}

			public void OnGUI()
			{
				Draw ();
			}
			
			public void Draw()
			{
				SetupStyles ();
				Callback ();
			}

		}
	}
}
