using UnityEngine;
using System.Collections.Generic;

namespace Tempest
{
	namespace Menu
	{
		public class ProfileMenu 
		{
			private string m_usernameField;
			private string m_passwordField;
			private int m_genderSelection;
			private string[] m_genderField;
			private string m_medicalField;
			private CalendarView m_dobView;
			private TableView<Database.ReportDB.Report> m_statView;
			
			private string m_dbServerField;
			private string m_dbPasswordField;
			private string m_dbDatabaseField;
			private string m_dbUserIDField;
			
			private Vector2 m_msgLogScrollView;
			
			private GUIStyle m_buttonStyle;
			private GUIStyle m_labelStyle;
			private GUIStyle m_textAreaStyle;
			private GUIStyle m_textFieldStyle;
			private GUIStyle m_msgLogStyle;
			private GUIStyle m_feedbackStyle;
			
			private int m_maxUsernameLength = 15;
			private int m_minUsernameLength = 2;
			private int m_maxPasswordLength = 15;
			private int m_minPasswordLength = 5;
			
			private TimedMessage m_feedback;
			
			private Database.TempestDB m_tempestDB = null;
			
			private int m_time;
			private string m_message;
			
			private delegate void MenuFunction();
			private MenuFunction Callback;
			
			// Use this for initialization

			public void BackToBeginning()
			{
				Callback = Options;
				ClearNonPersistantFields ();
			}

			public void Initialize () 
			{
				if(m_tempestDB == null)
				{
					m_tempestDB = GameObject.Find ("Database").GetComponent<Database.TempestDB> ();
					m_feedback = new TimedMessage ();
					
					m_usernameField = "";
					m_passwordField = "";
					m_genderField = new string[] {"Male", "Female", "Other"};
					m_genderSelection = 0;
					m_medicalField = "";
					m_dobView = new CalendarView (80);
					
					m_statView = new TableView<Database.ReportDB.Report> ();
					m_statView.AddColumn ("ID", 0.1f, (x, y) => (x.m_reportID.CompareTo(y.m_reportID)), (x, y) => (y.m_reportID.CompareTo(x.m_reportID)));
					m_statView.AddColumn ("Device", 0.2f, (x, y) => (x.m_device.CompareTo(y.m_device)), (x, y) => (y.m_device.CompareTo(x.m_device)));
					m_statView.AddColumn ("Task", 0.2f, (x, y) => (x.m_task.CompareTo(y.m_task)), (x, y) => (y.m_task.CompareTo(x.m_task)));
					m_statView.AddColumn ("Date", 0.3f, (x, y) => (x.m_timestamp.CompareTo(y.m_timestamp)), (x, y) => (y.m_timestamp.CompareTo(x.m_timestamp)));
					m_statView.AddColumn ("Score", 0.2f,(x, y) => (x.m_score.CompareTo(y.m_score)), (x, y) => (y.m_score.CompareTo(x.m_score)));
					
					m_dbServerField = "";
					m_dbUserIDField = "";
					m_dbDatabaseField = "";
					m_dbPasswordField = "";

					m_msgLogScrollView = Vector2.zero;

					Callback = Options;
				}
			}
			
			private void ClearNonPersistantFields()
			{
				m_usernameField = "";
				m_passwordField = "";
				m_medicalField = "";
				m_genderSelection = 0;
				m_dobView.MakeToday ();
				m_dobView.ResetScroll ();
				m_statView.ResetScroll ();
			}
			
			private bool VerifyAccountDetails(string username, string password)
			{
				if(username.Length < m_minUsernameLength)
				{
					m_feedback.Begin("Error, minimum of " + m_minUsernameLength.ToString() + " character allowable for the username", 5.0f, m_msgLogStyle);
					return false;
				}
				
				if(password.Length < m_minPasswordLength)
				{
					m_feedback.Begin("Error, minimum of  " + m_minPasswordLength.ToString() + " characters allowable for the password", 5.0f, m_msgLogStyle);
					return false;
				}
				
				return true;
			}
			
			private void CreateProfile()
			{
				Rect rect1 = new Rect (Screen.width * 0.3f, Screen.height * 0.2f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect2 = new Rect (Screen.width * 0.3f, Screen.height * 0.25f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect3 = new Rect (Screen.width * 0.3f, Screen.height * 0.3f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect4 = new Rect (Screen.width * 0.3f, Screen.height * 0.35f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect5 = new Rect (Screen.width * 0.3f, Screen.height * 0.5f, Screen.width * 0.07f, Screen.height * 0.04f);

				Rect rect6 = new Rect (Screen.width * 0.38f, Screen.height * 0.2f, Screen.width * 0.07f, Screen.height * 0.04f);		
				Rect rect7 = new Rect (Screen.width * 0.38f, Screen.height * 0.25f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect8 = new Rect (Screen.width * 0.38f, Screen.height * 0.3f, Screen.width * 0.12f, Screen.height * 0.04f);
				Rect rect9 = new Rect (Screen.width * 0.38f, Screen.height * 0.35f, Screen.width * 0.2f, Screen.height * 0.14f);
				Rect rect10 = new Rect (Screen.width * 0.38f, Screen.height * 0.5f, Screen.width * 0.04f, Screen.height * 0.04f);
				Rect rect11 = new Rect (Screen.width * 0.43f, Screen.height * 0.5f, Screen.width * 0.06f, Screen.height * 0.04f);
				Rect rect12 = new Rect (Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.04f, Screen.height * 0.04f);

				Rect rect13 = new Rect (Screen.width * 0.3f, Screen.height * 0.8f, Screen.width * 0.07f, Screen.height * 0.04f);
				Rect rect14 = new Rect (Screen.width * 0.38f, Screen.height * 0.8f, Screen.width * 0.07f, Screen.height * 0.04f);

				GUIStyle s1 = new GUIStyle (GUI.skin.label);
				s1.fontSize = 10;
				s1.alignment = TextAnchor.UpperCenter;
				
				GUIStyle s2 = new GUIStyle (GUI.skin.textField);
				s2.fontSize = 10;
				s2.alignment = TextAnchor.MiddleLeft;
				
				GUIStyle s3 = new GUIStyle (GUI.skin.button);
				s3.fontSize = 10;
				s3.alignment = TextAnchor.MiddleCenter;

				GUIStyle s4 = new GUIStyle (GUI.skin.textArea);
				s4.fontSize = 10;
				s4.alignment = TextAnchor.UpperLeft;

				//enter details, submit 'form'.. if username does not exist
				
				GUI.Label (rect1, "USERNAME", s1);
				GUI.Label (rect2, "PASSWORD", s1);
				GUI.Label (rect3, "GENDER", s1);
				GUI.Label (rect4, "MEDICAL DESCR", s1);
				GUI.Label (rect5, "DATE OF BIRTH", s1);
				
				//accept user details
				m_usernameField = GUI.TextField(rect6, m_usernameField, m_maxUsernameLength, s2).Trim();
				m_passwordField = GUI.PasswordField(rect7, m_passwordField, '*', m_maxPasswordLength, s2).Trim ();
				m_genderSelection = GUI.SelectionGrid (rect8, m_genderSelection, m_genderField, 3, s3);  
				m_medicalField = GUI.TextArea (rect9, m_medicalField, s4); 
				
				//render date GUI and accept user date of birth
				m_dobView.m_dayPos = rect10;
				m_dobView.m_monthPos = rect11;
				m_dobView.m_yearPos = rect12;
				m_dobView.Display ();
				
				//trim username and password fields

				if(GUI.Button(rect13, "CREATE", s3))
				{
					if(m_tempestDB.IsConnected)
					{
						if(VerifyAccountDetails(m_usernameField, m_passwordField))
						{
							if(m_tempestDB.ProfileDatabase.AddPatient(m_usernameField, m_passwordField, m_dobView.GetFormattedNumericDate ('/'), m_genderField[m_genderSelection], m_medicalField))
							{
								m_feedback.Begin("Profile created", 5.0f, m_msgLogStyle);
							}
							else
							{
								m_feedback.Begin("Profile with username already exists", 5.0f, m_msgLogStyle);
							}
						}
					}
					else
					{
						m_feedback.Begin("Not connected to server", 5.0f, m_msgLogStyle);
					}
				}

				GoBack (Options, rect14, s3);
			}
			
			private void ViewScores()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					//set styles for id and score column
					GUIStyle tupleStyle = new GUIStyle(GUI.skin.label);
					GUIStyle columnStyle = new GUIStyle(GUI.skin.label);
					
					tupleStyle.alignment = TextAnchor.MiddleCenter;
					tupleStyle.fontSize = 12;
					columnStyle.alignment = TextAnchor.MiddleCenter;
					columnStyle.fontSize = 10;
					
					m_statView.GetColumn("ID").SetStyles(columnStyle, tupleStyle);
					m_statView.GetColumn("Score").SetStyles(columnStyle, tupleStyle);
					
					//set styles for device, task and date columns
					tupleStyle = new GUIStyle(GUI.skin.label);
					columnStyle = new GUIStyle(GUI.skin.label);
					
					tupleStyle.alignment = TextAnchor.MiddleLeft;
					tupleStyle.fontSize = 12;
					columnStyle.alignment = TextAnchor.MiddleLeft;
					columnStyle.fontSize = 10;
					
					m_statView.GetColumn("Device").SetStyles(columnStyle, tupleStyle);
					m_statView.GetColumn("Task").SetStyles(columnStyle, tupleStyle);
					m_statView.GetColumn("Date").SetStyles(columnStyle, tupleStyle);
					
					//set style for entire table
					GUIStyle viewStyle = new GUIStyle(GUI.skin.box);
					viewStyle.alignment = TextAnchor.UpperCenter;
					m_statView.ViewStyle = viewStyle;
					
					m_statView.Width = Screen.width * 0.45f;
					m_statView.Height = Screen.height * 0.5f;
					m_statView.Position = new Vector2(Screen.width * 0.25f, Screen.height * 0.2f);
					
					m_statView.Display();
				}
				else
				{
					TextAnchor prevAnchor = m_labelStyle.alignment;
					m_labelStyle.alignment = TextAnchor.MiddleCenter;
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.35f, (Screen.width ) * 0.3f, (Screen.height ) * 0.3f), "No Profile Loaded", m_labelStyle);
					m_labelStyle.alignment = prevAnchor;
				}
				
				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                          (Screen.width ) * 0.12f,
				                          (Screen.height ) * 0.06f);
				
				//GoBack (ViewProfile, backRect);
			}
			
			
			private void ViewProfile()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					Rect rect1 = new Rect (Screen.width * 0.3f, Screen.height * 0.2f, Screen.width * 0.07f, Screen.height * 0.04f);		
					Rect rect2 = new Rect (Screen.width * 0.3f, Screen.height * 0.25f, Screen.width * 0.07f, Screen.height * 0.04f);
					Rect rect3 = new Rect (Screen.width * 0.3f, Screen.height * 0.3f, Screen.width * 0.07f, Screen.height * 0.04f);
					Rect rect4 = new Rect (Screen.width * 0.3f, Screen.height * 0.35f, Screen.width * 0.2f, Screen.height * 0.14f);

					Rect rect5 = new Rect (Screen.width * 0.38f, Screen.height * 0.2f, Screen.width * 0.06f, Screen.height * 0.04f);		
					Rect rect6 = new Rect (Screen.width * 0.38f, Screen.height * 0.25f, Screen.width * 0.06f, Screen.height * 0.04f);
					Rect rect7 = new Rect (Screen.width * 0.38f, Screen.height * 0.3f, Screen.width * 0.06f, Screen.height * 0.04f);
					Rect rect8 = new Rect (Screen.width * 0.38f, Screen.height * 0.35f, Screen.width * 0.2f, Screen.height * 0.14f);

					Rect rect9 = new Rect (Screen.width * 0.3f, Screen.height * 0.4f, Screen.width * 0.07f, Screen.height * 0.04f);
					Rect rect10 = new Rect (Screen.width * 0.38f, Screen.height * 0.4f, Screen.width * 0.07f, Screen.height * 0.04f);
	
					GUIStyle s1 = new GUIStyle (GUI.skin.label);
					s1.fontSize = 10;
					s1.alignment = TextAnchor.UpperCenter;
					
					GUIStyle s2 = new GUIStyle (GUI.skin.textField);
					s2.fontSize = 10;
					s2.alignment = TextAnchor.MiddleLeft;
					
					GUIStyle s3 = new GUIStyle (GUI.skin.button);
					s3.fontSize = 10;
					s3.alignment = TextAnchor.MiddleCenter;
					
					GUIStyle s4 = new GUIStyle (GUI.skin.textArea);
					s4.fontSize = 10;
					s4.alignment = TextAnchor.UpperLeft;
					
					//viewing of profile
					GUI.Label (rect1, "Username", s1);
					GUI.Label (rect2, "Date Of Birth", s1);
					GUI.Label (rect3, "Gender", s1);
					GUI.Label (rect4, "Medical Condition", s1);
					
					//get user details
					string gender = m_tempestDB.Profile.Value.m_gender;
					string dob = m_tempestDB.Profile.Value.m_birthDate;
					string medical = m_tempestDB.Profile.Value.m_medicalCondition;
					string username = m_tempestDB.Profile.Value.m_username;
					
					GUI.Label(rect5, username, s2);
					GUI.Label(rect6, dob, s2);
					GUI.Label(rect7, gender, s2);  
					GUI.Label (rect8, medical, s4); 
					
					//deletion of profile
					if(GUI.Button(rect9, "VIEW SCORES", s3))
					{
						//retrieve all scores from the database belonging to this user
						LoadScores();

						Callback = ViewScores;
					}

					else if(GUI.Button(rect10, "DELETE PROFILE", s3))
					{
						if(m_tempestDB.ProfileDatabase.DeletePatient (m_tempestDB.Profile.Value.m_username, m_tempestDB.Profile.Value.m_password))
						{
							m_tempestDB.Profile = null;
							
							m_feedback.Begin("Profile successfully deleted", 5.0f, m_msgLogStyle);
						}
						else
						{						
							m_feedback.Begin("Profile deletion failed", 5.0f, m_msgLogStyle);
						}
					}		
				}

				Rect rect11 = new Rect (Screen.width * 0.3f, Screen.height * 0.4f, Screen.width * 0.07f, Screen.height * 0.04f);

				GoBack (Options, rect11, GUI.skin.button);
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
				Rect rect1 = new Rect (Screen.width * 0.4f, Screen.height * 0.3f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect2 = new Rect (Screen.width * 0.46f, Screen.height * 0.3f, Screen.width * 0.1f, Screen.height * 0.04f);

				Rect rect3 = new Rect (Screen.width * 0.4f, Screen.height * 0.36f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect4 = new Rect (Screen.width * 0.46f, Screen.height * 0.36f, Screen.width * 0.1f, Screen.height * 0.04f);

				Rect rect5 = new Rect (Screen.width * 0.4f, Screen.height * 0.42f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect6 = new Rect (Screen.width * 0.46f, Screen.height * 0.42f, Screen.width * 0.1f, Screen.height * 0.04f);

				GUIStyle s1 = new GUIStyle (GUI.skin.label);
				s1.fontSize = 10;
				s1.alignment = TextAnchor.UpperCenter;

				GUIStyle s2 = new GUIStyle (GUI.skin.textField);
				s2.fontSize = 10;
				s2.alignment = TextAnchor.MiddleLeft;

				GUIStyle s3 = new GUIStyle (GUI.skin.button);
				s3.fontSize = 10;
				s3.alignment = TextAnchor.MiddleCenter;
				
				GUI.Label (rect1, "USERNAME", s1);
				GUI.Label (rect3, "PASSWORD", s1);
				
				m_usernameField = GUI.TextField(rect2, m_usernameField, m_maxUsernameLength, s2).Trim ();
				m_passwordField = GUI.PasswordField(rect4, m_passwordField, '*', m_maxPasswordLength, s2).Trim ();

				if(GUI.Button(rect6, "LOGIN", s3))
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

							m_feedback.Begin("Profile successfully loaded", 5.0f, m_msgLogStyle);
						}
						else 
						{
							m_feedback.Begin("Profile loading failed", 5.0f, m_msgLogStyle);
						}
					}
					else
					{
						m_feedback.Begin("Not connected to the server", 5.0f, m_msgLogStyle);
					}
				}
				
				GoBack (Options, rect5, s3);
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
				Rect rect1 = new Rect (Screen.width * 0.75f, Screen.height * 0.05f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect2 = new Rect (Screen.width * 0.81f, Screen.height * 0.05f, Screen.width * 0.05f, Screen.height * 0.04f);

				Rect rect3 = new Rect (Screen.width * 0.75f, Screen.height * 0.1f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect4 = new Rect (Screen.width * 0.81f, Screen.height * 0.1f, Screen.width * 0.06f, Screen.height * 0.04f);

				Rect rect5 = new Rect (Screen.width * 0.75f, Screen.height * 0.15f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect6 = new Rect (Screen.width * 0.81f, Screen.height * 0.15f, Screen.width * 0.06f, Screen.height * 0.04f);

				Rect rect7 = new Rect (Screen.width * 0.75f, Screen.height * 0.2f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect8 = new Rect (Screen.width * 0.81f, Screen.height * 0.2f, Screen.width * 0.06f, Screen.height * 0.04f);

				Rect rect9 = new Rect (Screen.width * 0.75f, Screen.height * 0.25f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect10 = new Rect (Screen.width * 0.81f, Screen.height * 0.25f, Screen.width * 0.06f, Screen.height * 0.04f);

				Rect rect11 = new Rect (Screen.width * 0.75f, Screen.height * 0.3f, Screen.width * 0.05f, Screen.height * 0.04f);
				Rect rect12 = new Rect (Screen.width * 0.81f, Screen.height * 0.3f, Screen.width * 0.06f, Screen.height * 0.04f);

				GUIStyle s1 = new GUIStyle(GUI.skin.label);
				s1.fontSize = 10;
				s1.alignment = TextAnchor.UpperCenter;

				GUIStyle s2 = new GUIStyle (GUI.skin.textField);
				s2.fontSize = 10;
				s2.alignment = TextAnchor.UpperLeft; 
				
				GUIStyle s3 = new GUIStyle (GUI.skin.button);
				s3.fontSize = 10;
				s3.alignment = TextAnchor.MiddleCenter; 
				
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

				if(GUI.Button (rect11, "DEFAULT", s3))
				{
					string[] tokens = m_tempestDB.GetDefaultServer().Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);
							
					m_dbServerField = tokens[0].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbDatabaseField = tokens[1].Split (new string[]{"="} , System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbUserIDField = tokens[2].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPasswordField = tokens[3].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
				}
						
				if(GUI.Button (rect12, "CONNECT", s3))
				{
					string config = "Server=" + m_dbServerField + ";" +
							"Database=" + m_dbDatabaseField + ";" +
							"User ID=" + m_dbUserIDField + ";" +
							"Password=" + m_dbPasswordField + ";" + 
							"Pooling=false;";
							
					m_tempestDB.Reconnect(config);
							
					if(m_tempestDB.IsConnected)
					{
						Callback = Options;
						ClearNonPersistantFields();
						m_tempestDB.LoadLoggedSession();
								
						m_feedback.Begin("Connected", 5.0f, m_msgLogStyle); 
					}
					else
					{
						m_feedback.Begin("Connection attempt failed", 5.0f, m_msgLogStyle); 
					}
				}
		
			}
			
			private void SetupStyles()
			{
				m_feedbackStyle = new GUIStyle(GUI.skin.label);
				m_feedbackStyle.alignment = TextAnchor.UpperLeft;
				m_feedbackStyle.fontSize = 10;
			}

			private void Options()
			{
				Rect rect1 = new Rect (Screen.width * 0.05f, Screen.height * 0.05f, Screen.width * 0.1f, Screen.height * 0.04f);
				Rect rect2 = new Rect (Screen.width * 0.16f, Screen.height * 0.05f, Screen.width * 0.12f, Screen.height * 0.04f);
				Rect rect3 = new Rect (Screen.width * 0.29f, Screen.height * 0.05f, Screen.width * 0.08f, Screen.height * 0.04f);
				Rect rect4 = new Rect (Screen.width * 0.38f, Screen.height * 0.05f, Screen.width * 0.08f, Screen.height * 0.04f);
				Rect rect5 = new Rect (Screen.width * 0.05f, Screen.height * 0.1f, Screen.width * 0.1f, Screen.height * 0.04f);
				Rect rect6 = new Rect (Screen.width * 0.16f, Screen.height * 0.1f, Screen.width * 0.12f, Screen.height * 0.04f);

				GUIStyle s1 = new GUIStyle(GUI.skin.label);
				s1.fontSize = 10;
					
				GUIStyle s2 = new GUIStyle(GUI.skin.textField);
				s2.fontStyle = FontStyle.Bold;
				s2.fontSize = 10;

				GUIStyle s3 = new GUIStyle (GUI.skin.button);
				s3.fontSize = 10;
				s3.alignment = TextAnchor.MiddleCenter;
				
				GUI.Label (rect1, "CURRENT PROFILE:", s1);

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

				if(GUI.Button (rect3, "LOGIN", s3)) Callback = LoginProfile;
				if(GUI.Button (rect4, "LOGOUT", s3)) LogoutProfile();

				if(GUI.Button (rect5, "CREATE NEW", s3)) Callback = CreateProfile;
				if(GUI.Button (rect6, "VIEW PROFILE", s3)) Callback = ViewProfile;
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
				Rect rect1 = new Rect (Screen.width * 0.75f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.15f);
				GUIStyle s1 = new GUIStyle (GUI.skin.box);
				s1.fontSize = 10;

				GUILayout.BeginArea (rect1, s1);
				m_msgLogScrollView = GUILayout.BeginScrollView (m_msgLogScrollView);

				GUILayout.BeginHorizontal ();
				GUILayout.Label ("MESSAGE LOG", m_feedbackStyle);
				GUILayout.EndHorizontal ();
				
				GUILayout.BeginHorizontal ();
				m_feedback.Display ();
				GUILayout.EndHorizontal ();
				
				GUILayout.EndScrollView ();
				GUILayout.EndArea ();
			}
			
			public void Draw()
			{
				SetupStyles ();
				UpdateFeedback ();
				ServerSettings();
				Callback ();
			}

		}
	}
}
