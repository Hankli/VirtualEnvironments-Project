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

			public TextAsset m_dbLevelAsset = null;
			public TextAsset m_dbDeviceAsset = null;
			public TextAsset m_dbServerAsset = null;

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

			public string Database
			{
				get { return m_sqlSource.Database; }
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
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(m_dbDeviceAsset.text);	
				
				if(doc != null)
				{
					XmlNode root = doc.SelectSingleNode("Devices");
					foreach(XmlNode nodes in root.SelectNodes("Device"))
					{
						string name = "", description = "";
						
						foreach(XmlNode node in nodes.ChildNodes)
						{
							if(node.Name == "Name") name = node.InnerXml;
							if(node.Name == "Description") description = node.InnerXml;
						}
						
						m_deviceDB.AddDevice(name, description);
					}
				}
			}

			private void WriteDefaultTasks()
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(m_dbLevelAsset.text);	
				
				if(doc != null)
				{
					XmlNode root = doc.SelectSingleNode("Tasks");
					foreach(XmlNode nodes in root.SelectNodes("Task"))
					{
						string name = "", description = "";

						foreach(XmlNode node in nodes.ChildNodes)
						{
							if(node.Name == "Name") name = node.InnerXml;
							if(node.Name == "Description") description = node.InnerXml;
						}

						m_taskDB.AddTask(name, description);
					}
				}
			}

			public string GetDefaultServer()
			{
				//whitespace sensitive
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(m_dbServerAsset.text);	
			
				if(doc != null)
				{
					string settings = "";

					foreach(XmlNode x in doc.SelectSingleNode("Settings").ChildNodes)
					{
						if(x.Name == "Server") settings += x.Name + "=" + x.InnerXml + ";";
						else if(x.Name == "Database") settings += x.Name + "=" + x.InnerXml + ";";
						else if(x.Name == "User") settings += x.Name + "=" + x.InnerXml + ";";
						else if(x.Name == "Password") settings += x.Name + "=" + x.InnerXml + ";";
						else if(x.Name == "Pooling") settings += x.Name + "=" + x.InnerXml + ";";
					}
					return settings;	
				}

				return "";
			}

			public bool Reconnect(string config)
			{		
				if(config == null || config.Length == 0) return false;

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

					m_patientDB.AddPatient ("Bryan", "password", "12/5/1209", "Male");
					m_reportDB.AddReport("Bryan", "Mouse and Keyboard", "Object Interaction", new System.DateTime(2011, 3, 12, 20, 34, 19), 120, 10.0f, 0.0f);
					m_reportDB.AddReport("Bryan", "Leap Motion", "Object Avoidance", new System.DateTime(1999, 10, 19, 3, 20, 30), 212, 2.05f, 2.0f);
					m_reportDB.AddReport("Bryan", "Kinect", "Way Finding", new System.DateTime(2001, 8, 24, 5, 54, 9), 444, 4.0f, 11.0f);
					m_reportDB.AddReport("Bryan", "Razer Hydra", "Object Interaction", new System.DateTime(1977, 4, 12, 16, 30, 55), 194, 12.0f, 14.0f);
				}

			}

			private void OnApplicationQuit()
			{
				Disconnect ();
			}

		}
	}
}
