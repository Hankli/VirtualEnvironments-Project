using UnityEngine;

using MySql.Data;
using MySql.Data.MySqlClient;

using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Tempest
{
	namespace Database
	{
		public class TempestDB : MonoBehaviour
		{
			Nullable<PatientDB.Patient> m_profile;

			private const string k_defaultServerFile = "db_server.xml";
			private const string k_defaultTasksFile = "db_tasks.xml";
			private const string k_defaultDevicesFile = "db_devices.xml";
			private const string k_loggedSessionFile = "session_log.xml";

			private SQLView m_sqlSource = new SQLView();
			private TaskDB m_taskDB = null;
			private DeviceDB m_deviceDB = null;
			private PatientDB m_patientDB = null;
			private ReportDB m_reportDB = null;

			public Nullable<PatientDB.Patient> Profile
			{
				get { return m_profile; }
				set { m_profile = value; }
			}

			public ReportDB ReportDatabase
			{
				get { return m_reportDB; }
			}

			public PatientDB ProfileDatabase
			{
				get { return m_patientDB; }
			}

			public DeviceDB DeviceDatabase
			{
				get { return m_deviceDB; }
			}
				
			public TaskDB TaskDatabase
			{
				get { return m_taskDB; }
			}

			private void WriteDefaultDevices()
			{
				XElement root = XElement.Load (@k_defaultDevicesFile);

				if(root != null)
				{
					foreach (XElement e in root.Elements())
					{
						m_deviceDB.AddDevice(e.Element("Name").Value, e.Element ("Description").Value);
					}
				}
			}

			private void WriteDefaultTasks()
			{
				XElement root = XElement.Load (@k_defaultTasksFile);

				if(root != null)
				{
					foreach (XElement e in root.Elements())
					{
						m_taskDB.AddTask(e.Element("Name").Value, e.Element ("Description").Value);
					}
				}
			}

			public string GetDefaultServer()
			{
				//whitespace sensitive
				XElement root = XElement.Load (@k_defaultServerFile);
	
				if(root != null)
				{
					return "Server=" + root.Element("Server").Value + ";" +
						   "Database=" + root.Element("Database").Value + ";" +
						   "User=" + root.Element("User").Value + ";" +
						   "Password=" + root.Element("Password").Value + ";" +
						   "Pooling=" + root.Element("Pooling").Value + ";";
				}
				return "";
			}

			public bool SaveLoggedSession(bool logout)
			{
				if(Profile.HasValue)
				{
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					XmlWriter writer =  XmlWriter.Create(@k_loggedSessionFile, settings);

					writer.WriteStartDocument();
					writer.WriteStartElement("Session");
					writer.WriteElementString("Logout", logout.ToString());
					writer.WriteElementString("Database", m_sqlSource.Database); 
					writer.WriteElementString("Username", Profile.Value.m_username);
					writer.WriteElementString("Password", Profile.Value.m_password);
					writer.WriteEndElement();
					writer.WriteEndDocument();

					writer.Flush();
					writer.Close();

					if(logout)
					{
						Profile = null;
					}

					return true;
				}
				return false;
			}

			public bool LoadLoggedSession()
			{
				if(IsConnected && System.IO.File.Exists(@k_loggedSessionFile))
				{
					XElement root =  XElement.Load(@k_loggedSessionFile);
					
					if(root != null)
					{
						if(root.Element("Database").Value == m_sqlSource.Database)
						{
							bool logout = false;
							bool.TryParse(root.Element("Logout").Value, out logout);

							if(!logout)
							{
								string username = root.Element("Username").Value;
								string password = root.Element("Password").Value;

								PatientDB.Patient patient = new PatientDB.Patient();

								if(m_patientDB.ExtractPatient(username, password, ref patient))
								{
									Profile = patient;
									return true;
								}
							}//end if
						}//end if
					}//end if
				}
				return false;
			}

			public bool Reconnect(string config)
			{		
				if(config.Length == null || config.Length == 0) return false;

				m_sqlSource.OpenConnection(config);

				if(m_sqlSource.OpenConnectionState)
				{
					//create each database relation manager
					m_patientDB = new PatientDB (m_sqlSource);
					m_deviceDB = new DeviceDB (m_sqlSource);
					m_taskDB = new TaskDB (m_sqlSource);
					m_reportDB = new ReportDB (m_sqlSource);

					//create tables if does not exist
					m_patientDB.CreateRelation();
					m_deviceDB.CreateRelation();
					m_taskDB.CreateRelation();
					m_reportDB.CreateRelation();

					//initialise all known data if not already there
					WriteDefaultDevices ();
					WriteDefaultTasks ();

					return true;
				}

				return false;
			}

			public bool IsConnected
			{
				get { return m_sqlSource != null ? m_sqlSource.OpenConnectionState : false; }
			}


			public void Disconnect()
			{
				m_sqlSource.CloseConnection ();
			}

			private void Start()
			{
				m_profile = null;

				DontDestroyOnLoad (gameObject);
		

				/////////keep for testing purposes ONLY EVERYTHING BELOW/////////////////////////
				/*
				if(Reconnect (GetDefaultServer()))
				{
					m_reportDB.DropRelation ();
					m_deviceDB.DropRelation ();
					m_taskDB.DropRelation ();
					m_patientDB.DropRelation ();

					m_deviceDB.CreateRelation ();
					m_patientDB.CreateRelation ();
					m_taskDB.CreateRelation ();
					m_reportDB.CreateRelation ();

					WriteDefaultDevices ();
					WriteDefaultTasks ();

					m_patientDB.AddPatient ("Bryan", "password", "12/5/1209", "Male", "Stroke");
					m_reportDB.AddReport("Bryan", "Leap Motion", "Object Avoidance", new System.DateTime(1999, 10, 19, 3, 20, 30), 212);
					m_reportDB.AddReport("Bryan", "Kinect", "Way Finding", new System.DateTime(2001, 8, 24, 5, 54, 9), 444);
					m_reportDB.AddReport("Bryan", "Razer Hydra", "Object Interaction", new System.DateTime(1989, 4, 10, 10, 30, 55), 194);
				}*/

			}

			private void OnApplicationQuit()
			{
				Disconnect ();
			}

		}
	}
}
