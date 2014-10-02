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

			private string m_dbServerField;
			private string m_dbPasswordField;
			private string m_dbDatabaseField;
			private string m_dbUserIDField;

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

				m_dobCalendar = new CalendarMenu (80);

				m_feedback = new TimedMessage ();

				m_usernameField = "";
				m_passwordField = "";
				m_genderField = new string[] {"Male", "Female"};
				m_genderSelection = 0;
				m_medicalField = "";

				m_dbServerField = "";
				m_dbUserIDField = "";
				m_dbDatabaseField = "";
				m_dbPasswordField = "";

				m_buttonWidth = 120;
				m_buttonHeight = 100;

				m_textFieldWidth = 150;
				m_textFieldHeight = 220;

				m_labelWidth = 60;
				m_labelHeight = 120;

				m_gridWidth = 40;
				m_gridHeight = 50;

				m_textAreaWidth = 300;
				m_textAreaHeight = 400;

				Callback = OptionsMenu;
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
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.2f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.28f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.36f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Gender", m_labelStyle);
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.44f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Medical Condition", m_labelStyle);
				GUI.Label(new Rect(Screen.width * 0.3f, Screen.height * 0.68f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Date Of Birth", m_labelStyle);

				//accept user details
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_textFieldStyle);
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.28f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_textFieldStyle);
				m_genderSelection = GUI.SelectionGrid (new Rect (Screen.width * 0.4f, Screen.height * 0.36f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), m_genderSelection, m_genderField, 2, m_buttonStyle);  
				m_medicalField = GUI.TextArea (new Rect (Screen.width * 0.4f, Screen.height * 0.44f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), m_medicalField, m_textAreaStyle); 

				//render date GUI and accept user date of birth
				m_dobCalendar.m_dayPos = new Rect (Screen.width * 0.4f, Screen.height * 0.68f, (Screen.width - 130.0f) * 0.055f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.m_monthPos = new Rect (Screen.width * 0.46f, Screen.height * 0.68f, (Screen.width - 130.0f) * 0.055f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.m_yearPos = new Rect (Screen.width * 0.52f, Screen.height * 0.68f, (Screen.width - 130.0f) * 0.055f, (Screen.height - 140.0f) * 0.05f);
				m_dobCalendar.Display ();

				//trim username and password fields
				bool fieldCheck = m_usernameField.Trim().Length > 0 && m_passwordField.Trim ().Length > 0;

				Rect createButtonRect = new Rect(Screen.width * 0.3f, Screen.height * 0.8f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.06f);
				if(GUI.Button(createButtonRect, "CREATE PROFILE", m_buttonStyle) && fieldCheck)
				{
					string msg = "";

					if(m_tempestDB.AccountDatabase.AddPatient(m_usernameField, m_passwordField, m_dobCalendar.GetFormattedDate ('/'), m_genderField[m_genderSelection], m_medicalField))
					{
						msg = "*Profile Successfully Created*";
					}
					else msg = "*Profile Creation Failed*";

					m_feedback.Begin(new Rect (Screen.width * 0.4f, Screen.height * 0.8f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), msg, 5.0f, m_labelStyle);
				}
			}

			private void ViewProfile()
			{
				if(m_tempestDB.m_bAccountLoaded)
				{
					//viewing of profile
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.20f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.28f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Date Of Birth", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.36f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Gender", m_labelStyle);
					GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.44f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Medical Condition", m_labelStyle);

					string gender = m_tempestDB.m_account.m_gender;
					string dob = m_tempestDB.m_account.m_birthDate;
					string medical = m_tempestDB.m_account.m_medicalCondition;
					string username = m_tempestDB.m_account.m_username;

					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), username, m_textFieldStyle);
					GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.28f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), dob, m_textFieldStyle);
					GUI.Label(new Rect (Screen.width * 0.4f, Screen.height * 0.36f, (Screen.width - m_gridWidth) * 0.1f, (Screen.height - m_gridHeight) * 0.04f), gender, m_textFieldStyle);  
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.44f, (Screen.width - m_textAreaWidth) * 0.35f, (Screen.height - m_textAreaHeight) * 0.5f), medical, m_textAreaStyle); 
				
					//deletion of profile
					Rect deleteButtonRect = new Rect(Screen.width * 0.3f, Screen.height * 0.65f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.06f);
					if(GUI.Button(deleteButtonRect, "DELETE PROFILE", m_buttonStyle))
					{
						string msg = "";

						if(m_tempestDB.AccountDatabase.DeletePatient (m_tempestDB.m_account.m_username, m_tempestDB.m_account.m_password))
						{
							m_tempestDB.m_bAccountLoaded = false;

							msg = "*Profile Successfully Deleted*";
						}
						else msg = "*Profile Deletion Failed";
					
						m_feedback.Begin(new Rect(Screen.width * 0.4f, Screen.height * 0.64f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), msg, 5.0f, m_labelStyle);
					}
				
				}
				else
				{
					GUI.Label (new Rect (Screen.width * 0.4f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.3f, (Screen.height - m_labelHeight) * 0.3f), "No Profile Loaded", m_labelStyle);
				}
			}

			private void LoadProfile()
			{
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.2f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Username");
				GUI.Label (new Rect (Screen.width * 0.3f, Screen.height * 0.28f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), "Password");
				
				m_usernameField = GUI.TextField(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_usernameField, m_textFieldStyle);
				m_passwordField = GUI.PasswordField(new Rect(Screen.width * 0.4f, Screen.height * 0.28f, (Screen.width - m_textFieldWidth) * 0.1f, (Screen.height - m_textFieldHeight) * 0.06f), m_passwordField, '*', m_textFieldStyle);

				Rect loadButtonRect = new Rect (Screen.width * 0.3f, Screen.height * 0.4f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.06f);
				if(GUI.Button(loadButtonRect, "LOAD PROFILE", m_buttonStyle))
				{
					string msg = "";

					if(m_tempestDB.AccountDatabase.ReadPatient (m_usernameField, m_passwordField, ref m_tempestDB.m_account))
					{
						m_tempestDB.m_bAccountLoaded = true;

						msg = "*Profile Successfully Loaded*";
					}
					else msg = "*Profile Loading Failed*";
				
					m_feedback.Begin(new Rect (Screen.width * 0.4f, Screen.height * 0.4f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.06f), msg, 5.0f, m_labelStyle);
				}
			}

			private void GoBack()
			{
				Rect goBackRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
				                         (Screen.width - m_buttonWidth) * 0.12f,
				                         (Screen.height - m_buttonHeight) * 0.06f);

				if(GUI.Button (goBackRect, "BACK", m_buttonStyle))
				{
					m_feedback.End (); //stop any feedback messages
					Callback = OptionsMenu; //back to main menu
				}
			}

			private void Heading()
			{
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.alignment = TextAnchor.MiddleCenter;
				style.fontSize = 25;
				style.normal = new GUIStyleState();
				style.normal.textColor = Color.black;

				Rect headingRect = new Rect (0.0f, 0.0f, Screen.width, Screen.height * 0.05f);
				GUI.Label (headingRect, "Profile Menu", style);
			}

			private void ServerSettings()
			{
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

				}

				GUI.Label (new Rect(Screen.width * 0.75f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), "Connection Status: ");
				
				string status = m_tempestDB.IsConnected ? "Online" : "Offline";
				Color color = GUI.color;
				GUI.color = m_tempestDB.IsConnected ? Color.green : Color.red;
				GUI.Label (new Rect(Screen.width * 0.85f, Screen.height * 0.15f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.05f), status, m_labelStyle);
				GUI.color = color;
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

			private void OptionsMenu()
			{
				Rect createProfileRect = new Rect(Screen.width * 0.1f, Screen.height * 0.3f, 
				                               (Screen.width - m_buttonWidth) * 0.12f, 
				                               (Screen.height - m_buttonHeight) * 0.06f);
				
				Rect loadProfileRect = new Rect (Screen.width * 0.1f, Screen.height * 0.42f,
				                              (Screen.width - m_buttonWidth) * 0.12f,
				                              (Screen.height - m_buttonHeight) * 0.06f);

				Rect viewProfileRect = new Rect (Screen.width * 0.1f, Screen.height * 0.54f,
				                                   (Screen.width - m_buttonWidth) * 0.12f,
				                                   (Screen.height - m_buttonHeight) * 0.06f);

				if(GUI.Button (createProfileRect, "CREATE PROFILE", m_buttonStyle))
				{
					ClearNonPersistantFields();
					Callback = CreateProfile;
				}

				else if(GUI.Button (loadProfileRect, "LOAD PROFILE", m_buttonStyle))
				{
					ClearNonPersistantFields();
					Callback = LoadProfile;
				}

				else if(GUI.Button(viewProfileRect, "VIEW PROFILE", m_buttonStyle))
				{
					ClearNonPersistantFields();
					Callback = ViewProfile;
				}
			}

			private void Feedback()
			{
				m_feedback.Display ();
			}

			private void OnGUI()
			{
				InitStyles ();
				Heading();
				ServerSettings();
				GoBack ();

				if(Callback != null)
				{
					Callback ();
				}

				Feedback ();
			}
		}
	}
}
