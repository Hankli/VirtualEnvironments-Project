using UnityEngine;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Collections.Generic;

namespace Tempest
{
	namespace Database
	{
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

			//specifically for loading and keeping track of current profiles
			public bool m_bAccountLoaded;
			public PatientDB.Patient m_account;


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

			public void Reconnect(string config = DefaultConfigIni)
			{
				m_sqlSource.OpenConnection(config);
				
				if(m_sqlSource.OpenConnectionState)
				{
					m_patientDB = new PatientDB (m_sqlSource);
					m_deviceDB = new DeviceDB (m_sqlSource);
					m_activityDB = new ActivityDB (m_sqlSource);
					m_reportDB = new ReportDB (m_sqlSource);

				    InitialiseDeviceData ();
				    InitialiseActivityData ();
				
					m_bAccountLoaded = false;
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
				m_bAccountLoaded = false;
				m_account = new PatientDB.Patient ();

				DontDestroyOnLoad (gameObject);
				Reconnect (DefaultConfigIni);

				/*
				m_deviceDB.DropRelation ();
				m_activityDB.DropRelation ();
				m_patientDB.DropRelation ();
				m_reportDB.DropRelation ();

				m_deviceDB.CreateRelation ();
				m_patientDB.CreateRelation ();
				m_activityDB.CreateRelation ();
				m_reportDB.CreateRelation ();

				m_patientDB.AddPatient ("tpv", "password", "12/5/1209", "Male", "Bad");
				m_deviceDB.AddDevice ("xbox controller", "n X-BOX controller");
				m_activityDB.AddActivity ("swimming", "Swim N Drown");

				List<ReportDB.Report> list = new List<ReportDB.Report> ();
				m_reportDB.AddReport ("tpv", "xbox controller", "swimming", System.DateTime.Now, 120);
				m_reportDB.AddReport ("tpv", "xbox controller", "swimming", System.DateTime.Now, 156);
				m_reportDB.ExtractReport ("tpv", list);

				foreach(ReportDB.Report rep in list)
				{
					Debug.Log (rep.ToString() + '\n');
				}
				*/

			}

			private void OnApplicationQuit()
			{
				Disconnect ();
			}

		}
	}
}
