using UnityEngine;
using System.Collections.Generic;

public class ProfileMenu : MonoBehaviour 
{
	private Rect m_createProfileRect;
	private Rect m_loadProfileRect;
	private Rect m_deleteProfileRect;
	private Rect m_goBackRect;
	private Rect m_headingRect;
	private Rect m_serverRect;
	
	private int m_buttonWidth;
	private int m_buttonHeight;

	private int m_textFieldWidth;
	private int m_textFieldHeight;

	private int m_labelWidth;
	private int m_labelHeight;

	private string m_usernameField;
	private string m_passwordField;

	private string m_dbServerField;
	private string m_dbPasswordField;
	private string m_dbDatabaseField;
	private string m_dbUserIDField;

	private bool m_bProfileLoaded;
	private bool m_bProfileDeleted;
	private bool m_bProfileCreated;

	public Tempest.Database.PatientDB.Patient m_account;
	private Tempest.Database.TempestCoreDB m_database;

	private delegate void MenuFunction();
	private MenuFunction Callback;

	// Use this for initialization
	private void Start () 
	{
		m_database = GameObject.Find ("Database").GetComponent<Tempest.Database.TempestCoreDB> ();
		m_account = new Tempest.Database.PatientDB.Patient ();

		m_bProfileLoaded = false;
		m_bProfileDeleted = false;
		m_bProfileCreated = false;
		
		m_usernameField = "";
		m_passwordField = "";

		m_dbServerField = "";
		m_dbUserIDField = "";
		m_dbDatabaseField = "";
		m_dbPasswordField = "";

		m_buttonWidth = 120;
		m_buttonHeight = 60;

		m_textFieldWidth = 150;
		m_textFieldHeight = 40;

		m_labelWidth = 60;
		m_labelHeight = 40;

		Callback = OptionsMenu;
	}

	private void ProfileCreate()
	{
		//enter details, submit 'form'.. if username does not exist

	}

	private void ProfileDelete()
	{
		//enter username
		//confirmation
		//delete.. or not

	}

	private void ProfileLoad()
	{
		//enter username
		//load.. or not
	}

	private void GoBack()
	{
		m_goBackRect = new Rect (Screen.width * 0.1f, Screen.height * 0.66f,
		                         (Screen.width - m_buttonWidth) * 0.12f,
		                         (Screen.height - m_buttonHeight) * 0.07f);

		if(GUI.Button (m_goBackRect, "BACK"))
		{
			Callback = null; //back to main menu
		}
	}

	private void Heading()
	{
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontSize = 20;
		style.normal = new GUIStyleState();
		style.normal.textColor = Color.red;
		m_database = GameObject.Find ("Database").GetComponent<Tempest.Database.TempestCoreDB>();
		m_headingRect = new Rect (0.0f, 0.0f, Screen.width, Screen.height * 0.05f);
		GUI.Label (m_headingRect, "Profile Menu", style);
	}

	private void ServerSettings()
	{
		GUI.skin.textField.wordWrap = true;
		GUI.skin.textField.clipping = TextClipping.Clip;

		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.2f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.1f), "Server");
		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.25f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.1f), "Database");
		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.3f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.1f), "User ID");
		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.35f, (Screen.width - m_labelWidth) * 0.1f, (Screen.height - m_labelHeight) * 0.1f), "Password");

		m_dbServerField =   GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.2f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbServerField);
		m_dbDatabaseField = GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.25f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbDatabaseField);
		m_dbUserIDField =   GUI.TextField(new Rect(Screen.width * 0.85f, Screen.height * 0.3f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbUserIDField);

		GUIStyle style = new GUIStyle ();
		style = GUI.skin.textField;
			
		m_dbPasswordField = GUI.PasswordField(new Rect(Screen.width * 0.85f, Screen.height * 0.35f, (Screen.width - m_textFieldWidth) * 0.13f, (Screen.height - m_textFieldHeight) * 0.05f), m_dbPasswordField, '*', style);
		
		if(GUI.Button (new Rect(Screen.width * 0.85f, Screen.height * 0.4f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), "Default"))
		{
			string[] tokens = Tempest.Database.TempestCoreDB.DefaultConfigIni.Split(new string[] {";"}, System.StringSplitOptions.RemoveEmptyEntries);

			m_dbServerField = tokens[0].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
			m_dbDatabaseField = tokens[1].Split (new string[]{"="} , System.StringSplitOptions.RemoveEmptyEntries)[1];
			m_dbUserIDField = tokens[2].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];
			m_dbPasswordField = tokens[3].Split (new string[]{"="}, System.StringSplitOptions.RemoveEmptyEntries)[1];

		}

		else if(GUI.Button (new Rect(Screen.width * 0.85f, Screen.height * 0.45f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), "Connect"))
		{
			string config = "Server=" + m_dbServerField + ";" +
				            "Database=" + m_dbDatabaseField + ";" +
					        "User ID=" + m_dbUserIDField + ";" +
					        "Password=" + m_dbPasswordField + ";" + 
					        "Pooling=false";

			m_database.Reconnect(config);

		}

		GUI.Label (new Rect(Screen.width * 0.75f, Screen.height * 0.15f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), "Connection Status: ");
		
		string status = m_database.IsConnected ? "Online" : "Offline";
		Color color = GUI.color;
		GUI.color = m_database.IsConnected ? Color.green : Color.red;
		GUI.Label (new Rect(Screen.width * 0.85f, Screen.height * 0.15f, (Screen.width - m_buttonWidth) * 0.1f, (Screen.height - m_buttonHeight) * 0.05f), status);
		GUI.color = color;

	}

	private void OptionsMenu()
	{
		m_createProfileRect = new Rect(Screen.width * 0.1f, Screen.height * 0.3f, 
		                               (Screen.width - m_buttonWidth) * 0.12f, 
		                               (Screen.height - m_buttonHeight) * 0.07f);
		
		m_loadProfileRect = new Rect (Screen.width * 0.1f, Screen.height * 0.42f,
		                              (Screen.width - m_buttonWidth) * 0.12f,
		                              (Screen.height - m_buttonHeight) * 0.07f);
		
		m_deleteProfileRect = new Rect (Screen.width * 0.1f, Screen.height * 0.54f,
		                                (Screen.width - m_buttonWidth) * 0.12f,
		                                (Screen.height - m_buttonHeight) * 0.07f);

		if(GUI.Button (m_createProfileRect, "CREATE PROFILE"))
		{
			Callback = ProfileCreate;
		}

		else if(GUI.Button (m_loadProfileRect, "LOAD PROFILE"))
		{
			Callback = ProfileLoad;
		}

		else if(GUI.Button (m_deleteProfileRect, "DELETE PROFILE"))
		{
			Callback = ProfileDelete;
		}
	}

	private void OnGUI()
	{
		Heading();
		ServerSettings();
		GoBack ();
		Callback ();

	}

	// Update is called once per frame
	private void Update ()
	{
	
	}
}
