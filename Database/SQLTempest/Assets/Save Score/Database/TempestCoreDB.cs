using UnityEngine;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Collections.Generic;

namespace Tempest
{
	namespace Database
	{
		public class Profile
		{
			public bool bLoaded;
			public Tempest.Database.PatientDB.Patient Account;

			public Profile()
			{
				bLoaded = false;
				Account = new PatientDB.Patient ();
			}
		}

		public class TempestCoreDB : MonoBehaviour
		{
			public const string DefaultConfigIni = "Server=au-cdbr-azure-east-a.cloudapp.net;" +
												   "Database=cdb_de02ef7d99;" +
												   "User ID=bdf6053979c316;" +
												   "Password=eaadd23f;" +
												   "Pooling=false";

			private SQLView m_sqlSource = new SQLView();
			private ActivityDB m_activityDB = null;
			private DeviceDB m_deviceDB = null;
			private PatientDB m_patientDB = null;
			private ReportDB m_reportDB = null;

			private Profile m_profile;


			public Profile ProfileData
			{
				get { return m_profile; }
			}

			public ReportDB ReportDatabase
			{
				get { return m_reportDB; }
			}

			public PatientDB AccountDatabase
			{
				get { return m_patientDB; }
			}

			public DeviceDB DeviceDatabase
			{
				get { return m_deviceDB; }
			}

			public ActivityDB LevelDatabase
			{
				get { return m_activityDB; }
			}

			private void InitialiseDeviceData()
			{
				m_deviceDB.AddDevice ("Razor Hydra", "Motion and orientation detection game controller by Sixense Entertainment");
				m_deviceDB.AddDevice ("Microsoft Kinect", "Motion sensing input device by Microsoft");
				m_deviceDB.AddDevice ("Leap Motion", "Computer hardware sensor device that supports hand and finger motions as input");
			}

			private void InitialiseActivityData()
			{
				m_activityDB.AddActivity ("Object Avoidance", "Tasks that involve navigating around, dodging and avoding obstacles in general");
				m_activityDB.AddActivity ("Object Interaction", "Tasks that involve manipulating of objects such as picking, pushing and throwing"); 
				m_activityDB.AddActivity ("Wayfinding", "Tasks that involve pathfinding related puzzles");
			}

			public void Reconnect(string config)
			{
				if(config.Length == 0) return;

				m_sqlSource.OpenConnection(config);
				
				if(m_sqlSource.OpenConnectionState)
				{
					//create each database relation manager
					m_patientDB = new PatientDB (m_sqlSource);
					m_deviceDB = new DeviceDB (m_sqlSource);
					m_activityDB = new ActivityDB (m_sqlSource);
					m_reportDB = new ReportDB (m_sqlSource);

					//create tables if does not exist
					m_patientDB.CreateRelation();
					m_deviceDB.CreateRelation();
					m_activityDB.CreateRelation();
					m_reportDB.CreateRelation();

					//initialise all known data if not already there
				    InitialiseDeviceData ();
				    InitialiseActivityData ();
				
					m_profile = new Profile();
				}
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
				m_profile = new Profile ();
			
				DontDestroyOnLoad (gameObject);
				Reconnect (DefaultConfigIni);


				//keep for testing purposes ONLY
				m_reportDB.DropRelation ();
				m_deviceDB.DropRelation ();
				m_activityDB.DropRelation ();
				m_patientDB.DropRelation ();

				m_deviceDB.CreateRelation ();
				m_patientDB.CreateRelation ();
				m_activityDB.CreateRelation ();
				m_reportDB.CreateRelation ();


				m_patientDB.AddPatient ("Hello", "password", "12/5/1209", "Male", "Bad");
				m_deviceDB.AddDevice ("xbox controller", "n X-BOX controller");
				m_activityDB.AddActivity ("swimming", "Swim N Drown");
				m_activityDB.AddActivity ("flying", "Fly");

				List<ReportDB.Report> list = new List<ReportDB.Report> ();
				m_reportDB.AddReport ("Hello", "xbox controller", "swimming", System.DateTime.Now, 120);
				m_reportDB.AddReport ("Hello", "xbox controller", "swimming", System.DateTime.Now, 156);
				m_reportDB.AddReport ("Hello", "xbox controller", "flying", System.DateTime.Now, 1999);
			}

			private void OnApplicationQuit()
			{
				Disconnect ();
			}

		}
	}
}
