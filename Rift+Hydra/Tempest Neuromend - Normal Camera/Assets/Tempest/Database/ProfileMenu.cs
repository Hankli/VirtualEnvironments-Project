using UnityEngine;
using System.Collections.Generic;

namespace Tempest
{
	namespace Menu
	{
		public class ProfileMenu 
		{
			private int m_buttonWidth;
			private int m_buttonHeight;
			
			private int m_textFieldWidth;
			private int m_textFieldHeight;
			
			private int m_labelWidth;
			private int m_labelHeight;
			
			private int m_gridWidth;
			private int m_gridHeight;
			
			private int m_textAreaWidth;
			private int m_textAreaHeight;
			
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
			
			private Database.TempestDB m_tempestDB;
			
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
					m_genderField = new string[] {"Male", "Female"};
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
					
					m_buttonWidth = 140;
					m_buttonHeight = 100;
					
					m_textFieldWidth = 150;
					m_textFieldHeight = 220;
					
					m_labelWidth = 60;
					m_labelHeight = 120;
					
					m_gridWidth = 40;
					m_gridHeight = 50;
					
					m_textAreaWidth = 300;
					m_textAreaHeight = 400;
					
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
				//enter details, submit 'form'.. if username does not exist
				
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.18f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.26f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.34f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Gender", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.42f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Medical Condition", m_labelStyle);
				GUI.Label(new Rect(Screen.width * 0.3f, Screen.height * 0.64f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Date Of Birth", m_labelStyle);
				
				//accept user details
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.18f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_maxUsernameLength, m_textFieldStyle).Trim();
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.26f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_maxPasswordLength, m_textFieldStyle).Trim ();
				m_genderSelection = GUI.SelectionGrid (new Rect (Screen.width * 0.4f, Screen.height * 0.34f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), m_genderSelection, m_genderField, 2, m_buttonStyle);  
				m_medicalField = GUI.TextArea (new Rect (Screen.width * 0.4f, Screen.height * 0.42f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), m_medicalField, m_textAreaStyle); 
				
				//render date GUI and accept user date of birth
				m_dobView.m_dayPos = new Rect (Screen.width * 0.4f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.05f, (Screen.height - 140.0f) * 0.05f);
				m_dobView.m_monthPos = new Rect (Screen.width * 0.45f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.07f, (Screen.height - 140.0f) * 0.05f);
				m_dobView.m_yearPos = new Rect (Screen.width * 0.52f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.05f, (Screen.height - 140.0f) * 0.05f);
				m_dobView.Display ();
				
				//trim username and password fields
				Rect createButtonRect = new Rect(Screen.width * 0.45f, Screen.height * 0.75f, (Screen.width - m_buttonWidth) * 0.05f, (Screen.height - m_buttonHeight) * 0.06f);
				
				if(GUI.Button(createButtonRect, "CREATE", m_buttonStyle))
				{
					if(m_tempestDB.IsConnected)
					{
						if(VerifyAccountDetails(m_usernameField, m_passwordField))
						{
							if(m_tempestDB.AccountDatabase.AddPatient(m_usernameField, m_passwordField, m_dobView.GetFormattedNumericDate ('/'), m_genderField[m_genderSelection], m_medicalField))
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

				Rect backRect = new Rect (Screen.width * 0.4f, Screen.height * 0.75f,
				                          (Screen.width - m_buttonWidth) * 0.05f,
				                          (Screen.height - m_buttonHeight) * 0.06f);
				
				GoBack (Options, backRect);
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
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.3f, (Screen.height - m_labelHeight) * 0.3f), "No Profile Loaded", m_labelStyle);
					m_labelStyle.alignment = prevAnchor;
				}
				
				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                          (Screen.width - m_buttonWidth) * 0.12f,
				                          (Screen.height - m_buttonHeight) * 0.06f);
				
				GoBack (ViewProfile, backRect);
			}
			
			
			private void ViewProfile()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					//viewing of profile
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.20f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.28f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Date Of Birth", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.36f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Gender", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.44f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Medical Condition", m_labelStyle);
					
					//get user details
					string gender = m_tempestDB.Profile.Value.m_gender;
					string dob = m_tempestDB.Profile.Value.m_birthDate;
					string medical = m_tempestDB.Profile.Value.m_medicalCondition;
					string username = m_tempestDB.Profile.Value.m_username;
					
					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), username, m_textFieldStyle);
					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.28f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), dob, m_textFieldStyle);
					GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.36f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), gender, m_textFieldStyle);  
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.44f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), medical, m_textAreaStyle); 
					
					//deletion of profile
					Rect showScoreRect = new Rect (Screen.width * 0.1f, Screen.height * 0.42f,(Screen.width - m_buttonWidth) * 0.12f,(Screen.height - m_buttonHeight) * 0.06f);
					Rect deleteButtonRect = new Rect(Screen.width * 0.1f, Screen.height * 0.54f, (Screen.width - m_buttonWidth) * 0.12f, (Screen.height - m_buttonHeight) * 0.06f);

					if(GUI.Button(showScoreRect, "VIEW SCORES", m_buttonStyle))
					{
						//retrieve all scores from the database belonging to this user
						LoadScores();

						Callback = ViewScores;
					}

					else if(GUI.Button(deleteButtonRect, "DELETE PROFILE", m_buttonStyle))
					{
						if(m_tempestDB.AccountDatabase.DeletePatient (m_tempestDB.Profile.Value.m_username, m_tempestDB.Profile.Value.m_password))
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
				else
				{
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.3f, (Screen.height - m_labelHeight) * 0.3f), "No Profile Loaded", m_labelStyle);
				}
						
				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                          (Screen.width - m_buttonWidth) * 0.12f,
				                          (Screen.height - m_buttonHeight) * 0.06f);
				
				GoBack (Options, backRect);
			}

			private void LoadScores()
			{
				if(m_tempestDB.Profile.HasValue)
				{
					m_statView.DropItems ();
					m_tempestDB.ReportDatabase.ExtractReport(m_tempestDB.Profile.Value.m_username, m_statView.Items);
				}
			}
			
			private void LoadProfile()
			{
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.3f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.38f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password", m_labelStyle);
				
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.3f, (Screen.width - m_textFieldWidth) * 0.12f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_maxUsernameLength, m_textFieldStyle).Trim ();
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.38f, (Screen.width - m_textFieldWidth) * 0.12f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_maxPasswordLength, m_textFieldStyle).Trim ();
				
				Rect loadButtonRect = new Rect (Screen.width * 0.45f, Screen.height * 0.46f, (Screen.width - m_buttonWidth) * 0.05f, (Screen.height - m_buttonHeight) * 0.06f);
				
				if(GUI.Button(loadButtonRect, "LOAD", m_buttonStyle))
				{
					if(m_tempestDB.IsConnected)
					{
						//clear whatever was left from before
						m_statView.DropItems();
					
						//display feedback
						Database.PatientDB.Patient profile = new Database.PatientDB.Patient();	

						if(m_tempestDB.AccountDatabase.ExtractPatient (m_usernameField, m_passwordField, ref profile))
						{
							m_tempestDB.Profile = profile;
							
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
				
				//draw back rectangle
				Rect backRect = new Rect (Screen.width * 0.4f, Screen.height * 0.46f,
				                          (Screen.width - m_buttonWidth) * 0.05f,
				                          (Screen.height - m_buttonHeight) * 0.06f);
				
				GoBack (Options, backRect);
			}
			
			private void GoBack(MenuFunction func, Rect rect)
			{
				//convinient go back function 
				if(GUI.Button (rect, "BACK", m_buttonStyle))
				{
					ClearNonPersistantFields(); 
					Callback = func; //back to main menu
				}
			}
			
			private void Heading()
			{
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.MiddleCenter;
				style.fontSize = 25;
				style.normal = new GUIStyleState();
				style.normal.textColor = Color.black;
				
				Rect headingRect = new Rect (Screen.width * 0.22f, 0.0f, Screen.width *0.5f, Screen.height * 0.05f);
				GUI.Label (headingRect, "Profile Menu", style);
			}
			
			private void ServerSettings()
			{
				GUI.Label (new Rect(Screen.width * 0.75f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), "Connection Status", m_labelStyle);
				
				string status = m_tempestDB.IsConnected ? "Online" : "Offline";
				
				Color color = GUI.color;
				GUI.color = m_tempestDB.IsConnected ? Color.green : Color.red;
				GUI.Label (new Rect(Screen.width * 0.85f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), status, m_msgLogStyle);
				GUI.color = color;
				
				GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.2f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Server", m_labelStyle);
				GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.25f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Database", m_labelStyle);
				GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.3f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "User ID", m_labelStyle);
				GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password", m_labelStyle);
				
				m_dbServerField =   GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbServerField, m_textFieldStyle);
				m_dbDatabaseField = GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.25f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbDatabaseField, m_textFieldStyle);
				m_dbUserIDField =   GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.3f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbUserIDField, m_textFieldStyle);
				m_dbPasswordField = GUI.PasswordField(new Rect(Screen.width * 0.85f, Screen.height * 0.35f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbPasswordField, '*', m_textFieldStyle);
				
				if(GUI.Button (new Rect(Screen.width * 0.85f, Screen.height * 0.42f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), "Default", m_buttonStyle))
				{
					string[] tokens = m_tempestDB.GetDefaultServer().Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);
					
					m_dbServerField = tokens[0].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbDatabaseField = tokens[1].Split (new string[]{"="} , System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbUserIDField = tokens[2].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
					m_dbPasswordField = tokens[3].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
				}
				
				else if(GUI.Button (new Rect(Screen.width * 0.85f, Screen.height * 0.48f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), "Connect", m_buttonStyle))
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
				m_labelStyle = new GUIStyle (GUI.skin.label);
				m_labelStyle.fontSize = 11;
				m_labelStyle.alignment = TextAnchor.UpperRight;

				m_msgLogStyle = new GUIStyle (GUI.skin.label);
				m_msgLogStyle.fontSize = 10;
				m_msgLogStyle.alignment = TextAnchor.UpperLeft;
				
				m_buttonStyle = new GUIStyle (GUI.skin.button);
				m_buttonStyle.alignment = TextAnchor.MiddleCenter;
				m_buttonStyle.clipping = TextClipping.Clip;
				m_buttonStyle.fontSize = 11;
				
				m_textFieldStyle = new GUIStyle (GUI.skin.textField);
				m_textFieldStyle.alignment = TextAnchor.UpperLeft;
				m_textFieldStyle.fontSize = 11;
				m_textFieldStyle.clipping = TextClipping.Clip;
				m_textFieldStyle.wordWrap = true;
				
				m_textAreaStyle = new GUIStyle (GUI.skin.textArea);
				m_textAreaStyle.alignment = TextAnchor.UpperLeft;
				m_textAreaStyle.fontSize = 11;
				m_textAreaStyle.clipping = TextClipping.Clip;
				m_textAreaStyle.wordWrap = true;

				m_feedbackStyle = new GUIStyle(GUI.skin.label);
				m_feedbackStyle.alignment = TextAnchor.UpperLeft;
				m_feedbackStyle.fontSize = 8;
			}
			
			private void Options()
			{
				Rect createProfileRect = new Rect(Screen.width * 0.42f, Screen.height * 0.2f, 
				                                 (Screen.width - m_buttonWidth) * 0.12f, 
				                                 (Screen.height - m_buttonHeight) * 0.06f);
				
				Rect loadProfileRect = new Rect(Screen.width * 0.42f, Screen.height * 0.32f,
				                               (Screen.width - m_buttonWidth) * 0.12f,
				                               (Screen.height - m_buttonHeight) * 0.06f);
				
				Rect viewProfileRect = new Rect(Screen.width * 0.42f, Screen.height * 0.44f,
				                               (Screen.width - m_buttonWidth) * 0.12f,
				                               (Screen.height - m_buttonHeight) * 0.06f);
				
				Rect backRect = new Rect(Screen.width * 0.42f, Screen.height * 0.56f,
				                        (Screen.width - m_buttonWidth) * 0.12f,
				                        (Screen.height - m_buttonHeight) * 0.06f);
				
				
				if(GUI.Button (createProfileRect, "CREATE PROFILE", m_buttonStyle))
				{
					Callback = CreateProfile;
				}
				
				else if(GUI.Button (loadProfileRect, "LOAD PROFILE", m_buttonStyle))
				{
					Callback = LoadProfile;
				}
				
				else if(GUI.Button(viewProfileRect, "VIEW PROFILE", m_buttonStyle))
				{
					Callback = ViewProfile;
				}
			}
			
			
			private void UpdateFeedback()
			{
				GUILayout.BeginArea (new Rect(Screen.width * 0.4f, Screen.height * 0.85f, (Screen.width - 150.0f) * 0.2f, (Screen.height - 200.0f) * 0.2f), "", GUI.skin.box);
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
				Heading();
				UpdateFeedback ();
				ServerSettings();
				Callback ();
			}
		}
	}
}
