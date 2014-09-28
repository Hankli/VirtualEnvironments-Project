using UnityEngine;

using MySql.Data;
using MySql.Data.MySqlClient;


namespace Tempest
{
	namespace Database
	{
		public class TempestDBHUB : MonoBehaviour
		{
			public string m_configIni = "Server=au-cdbr-azure-east-a.cloudapp.net;" +
				"Database=cdb_de02ef7d99;User ID=bdf6053979c316;Password=eaadd23f;Pooling=false";

			private SQLView m_sqlView = new SQLView();

			private ActivityDB m_activityDB = null;
			private DeviceDB m_deviceDB = null;
			private PatientDB m_patientDB = null;
			private ReportDB m_reportDB = null;

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

			public void Reconnect()
			{
				m_sqlView.OpenConnection(m_configIni);
				
				if(m_sqlView.OpenConnectionState)
				{
				   m_patientDB = new PatientDB (m_sqlView);
				   m_deviceDB = new DeviceDB (m_sqlView);
				   m_activityDB = new ActivityDB (m_sqlView);
				   m_reportDB = new ReportDB (m_sqlView);

				   InitialiseDeviceData ();
				   InitialiseActivityData ();
				}
			}

			private void Start()
			{
				Reconnect ();
				/*
				m_reportDB.DropRelation ();
				m_deviceDB.DropRelation ();
				m_activityDB.DropRelation ();
				m_patientDB.DropRelation ();
				*/

				m_patientDB.AddPatient ("tpv", "password", "12/5/1209", "Male", "Bad");
				m_deviceDB.AddDevice ("xbox controller", "An X-BOX controller");
				m_activityDB.AddActivity ("swimming", "Swim N Drown");
				m_reportDB.AddReport ("tpv", "xbox controller", "swimming", System.DateTime.Now, 120);

				m_sqlView.BeginQuery ("SELECT * FROM patient");
				m_sqlView.CommitQuery ();
				Debug.Log (m_sqlView.GetQueryResult ());
				m_sqlView.EndQuery ();
			}

			private void OnApplicationQuit()
			{
				m_sqlView.CloseConnection ();
			}
		}
	}
}
