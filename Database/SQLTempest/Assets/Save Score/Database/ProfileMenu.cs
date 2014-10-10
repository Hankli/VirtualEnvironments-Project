using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Tempest
{
	namespace MenuGUI
	{
		public class ProfileMenu : MonoBehaviour
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
			private CalendarMenu m_dobCalendar;
			private TableView m_scoreTable;

			private string m_dbServerField;
			private string m_dbPasswordField;
			private string m_dbDatabaseField;
			private string m_dbUserIDField;

			private Vector2 m_msgLogScrollView;

			private GUIStyle m_buttonStyle;
			private GUIStyle m_labelStyle;
			private GUIStyle m_textAreaStyle;
			private GUIStyle m_textFieldStyle;

			private TimedMessage m_feedback;

			private Tempest.Database.TempestCoreDB m_tempestDB;

			private int m_time;
			private string m_message;

			private delegate void MenuFunction();
			private MenuFunction Callback;

			// Use this for initialization


			private void Start () 
			{
				m_tempestDB = GameObject.Find ("Database").GetComponent<Tempest.Database.TempestCoreDB> ();

				m_feedback = new TimedMessage ();

				m_usernameField = "";
				m_passwordField = "";
				m_genderField = new string[] {"Male", "Female"};
				m_genderSelection = 0;
				m_medicalField = "";
				m_dobCalendar = new CalendarMenu (80);

				m_scoreTable = new TableView ();
				m_scoreTable.AddColumn ("ID", 0.2f);
				m_scoreTable.AddColumn ("Device", 0.2f);
				m_scoreTable.AddColumn ("Level", 0.2f);
				m_scoreTable.AddColumn ("Timestamp", 0.2f);
				m_scoreTable.AddColumn ("Score", 0.2f);

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

			private void ClearNonPersistantFields()
			{
				m_usernameField = "";
				m_passwordField = "";
				m_medicalField = "";
				m_genderSelection = 0;
				m_dobCalendar.MakeToday ();
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
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.18f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_textFieldStyle);
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.26f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_textFieldStyle);
				m_genderSelection = GUI.SelectionGrid (new Rect (Screen.width * 0.4f, Screen.height * 0.34f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), m_genderSelection, m_genderField, 2, m_buttonStyle);  
				m_medicalField = GUI.TextArea (new Rect (Screen.width * 0.4f, Screen.height * 0.42f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), m_medicalField, m_textAreaStyle); 

				//render date GUI and accept user date of birth
				m_dobCalendar.m_dayPos = new Rect (Screen.width * 0.4f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.05f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.m_monthPos = new Rect (Screen.width * 0.45f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.07f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.m_yearPos = new Rect (Screen.width * 0.52f, Screen.height * 0.64f, (Screen.width - 130.0f) * 0.05f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.Display ();

				//trim username and password fields
				bool fieldCheck = m_usernameField.Trim().Length > 0 && m_passwordField.Trim ().Length > 0;

				Rect createButtonRect = new Rect(Screen.width * 0.1f, Screen.height * 0.52f, (Screen.width - m_buttonWidth) * 0.12f, (Screen.height - m_buttonHeight) * 0.06f);
				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.64f,
				                          (Screen.width - m_buttonWidth) * 0.12f,
				                          (Screen.height - m_buttonHeight) * 0.06f);

				if(GUI.Button(createButtonRect, "CREATE", m_buttonStyle))
				{
					string msg = "";

					if(fieldCheck)
					{
						if(m_tempestDB.AccountDatabase.AddPatient(m_usernameField, m_passwordField, m_dobCalendar.GetFormattedNumericDate ('/'), m_genderField[m_genderSelection], m_medicalField))
						{
							msg = "*Profile successfully created*";
						}
						else
						{
							msg = "*Profile creation failed*";
						}
					}
					else 
					{
						msg = "*Cannot create profile with empty username or password*";
					}
					
					m_feedback.Begin(msg, 5.0f, m_labelStyle);
				}

				GoBack (Options, backRect);
			}
	
			private void ViewScores()
			{
				if(m_tempestDB.ProfileData.bLoaded)
				{
					GUIStyle tupleStyle = new GUIStyle(GUI.skin.label);
					GUIStyle columnStyle = new GUIStyle(GUI.skin.label);
					GUIStyle viewStyle = new GUIStyle(GUI.skin.box);

					tupleStyle.alignment = TextAnchor.MiddleCenter;
					tupleStyle.fontSize = 12;

					columnStyle.alignment = TextAnchor.MiddleCenter;
					columnStyle.fontSize = 10;

					viewStyle.alignment = TextAnchor.UpperCenter;

					m_scoreTable.Width = Screen.width * 0.4f;
					m_scoreTable.Height = Screen.height * 0.5f;
					m_scoreTable.Position = new Vector2(Screen.width * 0.25f, Screen.height * 0.2f);
					m_scoreTable.TupleStyle = tupleStyle;
					m_scoreTable.ColumnStyle = columnStyle;
					m_scoreTable.ViewStyle = viewStyle;

					m_scoreTable.Display();
				}
				else
				{
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.3f, (Screen.height - m_labelHeight) * 0.3f), "No Profile Loaded", m_labelStyle);
				}

				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                          (Screen.width - m_buttonWidth) * 0.12f,
				                          (Screen.height - m_buttonHeight) * 0.06f);

				GoBack (ViewProfile, backRect);
			}


			private void ViewProfile()
			{
				if(m_tempestDB.ProfileData.bLoaded)
				{
					//viewing of profile
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.20f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.28f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Date Of Birth", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.36f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Gender", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.44f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Medical Condition", m_labelStyle);

					//get user details
					string gender = m_tempestDB.ProfileData.Account.m_gender;
					string dob = m_tempestDB.ProfileData.Account.m_birthDate;
					string medical = m_tempestDB.ProfileData.Account.m_medicalCondition;
					string username = m_tempestDB.ProfileData.Account.m_username;

					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), username, m_textFieldStyle);
					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.28f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), dob, m_textFieldStyle);
					GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.36f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), gender, m_textFieldStyle);  
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.44f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), medical, m_textAreaStyle); 

					//deletion of profile
					Rect showScoreRect = new Rect (Screen.width * 0.1f, Screen.height * 0.42f,(Screen.width - m_buttonWidth) * 0.12f,(Screen.height - m_buttonHeight) * 0.06f);
					Rect deleteButtonRect = new Rect(Screen.width * 0.1f, Screen.height * 0.54f, (Screen.width - m_buttonWidth) * 0.12f, (Screen.height - m_buttonHeight) * 0.06f);

					if(GUI.Button(showScoreRect, "VIEW SCORES", m_buttonStyle))
					{
						Callback = ViewScores;
					}

					else if(GUI.Button(deleteButtonRect, "DELETE PROFILE", m_buttonStyle))
					{
						string msg = "";

						if(m_tempestDB.AccountDatabase.DeletePatient (m_tempestDB.ProfileData.Account.m_username, m_tempestDB.ProfileData.Account.m_password))
						{
							m_tempestDB.ProfileData.bLoaded = false;

							msg = "*Profile successfully deleted*";
						}
						else
						{
							msg = "*Profile deletion failed";
						}

						m_feedback.Begin(msg, 5.0f, m_labelStyle);
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

			private void LoadProfile()
			{
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.3f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username");
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.38f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password");
				
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.3f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_textFieldStyle);
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.38f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_textFieldStyle);

				Rect loadButtonRect = new Rect (Screen.width * 0.1f, Screen.height * 0.54f, (Screen.width - m_buttonWidth) * 0.12f, (Screen.height - m_buttonHeight) * 0.06f);
				if(GUI.Button(loadButtonRect, "LOAD", m_buttonStyle))
				{
					//clear whatever was left from before
					m_scoreTable.DropItems();

					//extract all reports
					List<Tempest.Database.ReportDB.Report> list = new List<Tempest.Database.ReportDB.Report>();
					m_tempestDB.ReportDatabase.ExtractReport(m_usernameField, list);
					foreach(Tempest.Database.ReportDB.Report report in list)
					{
						m_scoreTable.AddItem(report);
					}

					//display feedback
					string msg = "";
					if(m_tempestDB.AccountDatabase.ExtractPatient (m_usernameField, m_passwordField, ref m_tempestDB.ProfileData.Account))
					{
						m_tempestDB.ProfileData.bLoaded = true;
						msg = "*Profile successfully loaded*";
					}
					else 
					{
						msg = "*Profile loading failed*";
					}

					m_feedback.Begin(msg, 5.0f, m_labelStyle);
				}


				//draw back rectangle
				Rect backRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                          (Screen.width - m_buttonWidth) * 0.12f,
				                          (Screen.height - m_buttonHeight) * 0.06f);

				GoBack (Options, backRect);
			}
	

			private void GoBack(MenuFunction func, Rect rect)
			{
				//convinient go back function 
				if(GUI.Button (rect, "BACK", m_buttonStyle))
				{
					m_feedback.End (); //stop any feedback messages 

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
				GUI.Label (new Rect(Screen.width * 0.75f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), "Connection Status: ");
				
				string status = m_tempestDB.IsConnected ? "Online" : "Offline";
				Color color = GUI.color;
				GUI.color = m_tempestDB.IsConnected ? Color.green : Color.red;
				GUI.Label (new Rect(Screen.width * 0.85f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), status, m_labelStyle);
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
					string[] tokens = Tempest.Database.TempestCoreDB.DefaultConfigIni.Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);

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
							        "Pooling=false";

					m_tempestDB.Reconnect(config);

					if(m_tempestDB.IsConnected)
					{
						m_feedback.Begin("*Connected*", 5.0f, m_labelStyle); 
					}
					else
					{
						m_feedback.Begin("*Connection attempt failed*", 5.0f, m_labelStyle); 
					}
				}
			}

			private void InitStyles()
			{
				m_labelStyle = new GUIStyle (GUI.skin.label);
				m_labelStyle.fontSize = 11;
				m_labelStyle.alignment = TextAnchor.UpperLeft;
				
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
			}

			private void Options()
			{
				Rect createProfileRect = new Rect(Screen.width * 0.42f, Screen.height * 0.2f, 
				                               (Screen.width - m_buttonWidth) * 0.12f, 
				                               (Screen.height - m_buttonHeight) * 0.06f);
				
				Rect loadProfileRect = new Rect (Screen.width * 0.42f, Screen.height * 0.32f,
				                              (Screen.width - m_buttonWidth) * 0.12f,
				                              (Screen.height - m_buttonHeight) * 0.06f);

				Rect viewProfileRect = new Rect (Screen.width * 0.42f, Screen.height * 0.44f,
				                                   (Screen.width - m_buttonWidth) * 0.12f,
				                                   (Screen.height - m_buttonHeight) * 0.06f);

				Rect backRect = new Rect (Screen.width * 0.42f, Screen.height * 0.56f,
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

				GoBack (Options, backRect);
			}


			private void ShowFeedback()
			{
				GUILayout.BeginArea (new Rect(Screen.width * 0.4f, Screen.height * 0.85f, (Screen.width - 150.0f) * 0.2f, (Screen.height - 200.0f) * 0.2f), "", GUI.skin.box);
				m_msgLogScrollView = GUILayout.BeginScrollView (m_msgLogScrollView);

				GUILayout.BeginHorizontal ();
				GUILayout.Label ("MESSAGE LOG:", m_labelStyle);
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ();
				m_feedback.Display ();
				GUILayout.EndHorizontal ();

				GUILayout.EndScrollView ();
				GUILayout.EndArea ();
			}

			private void OnGUI()
			{
				InitStyles ();
				ShowFeedback ();
				Heading();
				ServerSettings();

				if(Callback != null)
				{
					Callback ();
				}
			}
		}
	}
}
