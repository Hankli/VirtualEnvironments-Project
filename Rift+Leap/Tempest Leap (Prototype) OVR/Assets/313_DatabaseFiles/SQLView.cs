using UnityEngine;
using System.Collections;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Tempest
{
	namespace SQL
	{
		public class SQLView : MonoBehaviour
		{
			public static string m_defaultConfig = "Server=au-cdbr-azure-east-a.cloudapp.net;Database=cdb_de02ef7d99;User ID=bdf6053979c316;Password=eaadd23f;Pooling=false";
			private string m_currentConfig = null;

			private MySqlConnection m_connection = null;
			private MySqlCommand m_command = null;
			private MySqlDataReader m_reader = null;

			private void Start()
			{
				OpenConnection (m_defaultConfig);


				//SetQuery ("drop table patient");
				//CommitQuery ();

				//CREATING TABLES  NO TRIGGERS :( )

				/*
				BeginQuery ();

					SetQuery("CREATE TABLE device(" +
	                         "Name VARCHAR(50)," +
							 "Description VARCHAR(100), " +
							 "CONSTRAINT PRIMARY KEY(Name))");
					CommitQuery ();
	

					SetQuery("CREATE TABLE activity(" +
					         "Name VARCHAR(50)," +
					         "Description VARCHAR(100)," +
					         "CONSTRAINT PRIMARY KEY(Name))");
					CommitQuery ();


					SetQuery ("CREATE TABLE patient(" +
						"PatientID INT NOT NULL AUTO_INCREMENT, " +
					    "Gender VARCHAR(10), " +
						"DateOfBirth DATE, " +
						"MedicalCondition VARCHAR(100), " +
						"CONSTRAINT PRIMARY KEY(PatientID))");	   
					CommitQuery ();


					SetQuery("CREATE TABLE activityreport(" +
					         "ReportID INT NOT NULL AUTO_INCREMENT," +
					         "PatientID INT NOT NULL," +
					         "DeviceName VARCHAR(50) NOT NULL," +
					         "ActivityName VARCHAR(50) NOT NULL," +
					         "CompletionDate DATE NOT NULL," +
					         "CompletionScore INT NOT NULL," +
					         "PRIMARY KEY(ReportID)," +
					         "FOREIGN KEY(DeviceName) REFERENCES device(Name) ON UPDATE CASCADE ON DELETE RESTRICT, " +
					         "FOREIGN KEY(ActivityName) REFERENCES activity(Name) ON UPDATE CASCADE ON DELETE RESTRICT, " +
					         "FOREIGN KEY(PatientID) REFERENCES patient(PatientID) ON UPDATE CASCADE ON DELETE RESTRICT)" );
					CommitQuery();


					SetQuery ("CREATE TABLE patientaccount(" +
							  "Username VARCHAR(20) UNIQUE NOT NULL, " +
							  "Password VARCHAR(20) NOT NULL, "+
				          	  "PatientID INT NOT NULL, " +
				              "PRIMARY KEY(Username))," +
				              "FOREIGN KEY REFERENCES patient(PatientID) ON UPDATE CASCADE ON DELETE RESTRICT"); 
				    CommitQuery();


				    EndQuery();

					//*/

				/* ADDING DATA
		        BeginQuery();
				SetQuery("insert into patient(DateOfBirth, Gender, Description, Height, Weight) values (?DOB, ?GEND, ?DESC, ?HGT, ?WGT)");
				Write ("?DOB", MySqlDbType.Date, System.Convert.ToDateTime("5/3/1991"));
				Write ("?GEND", MySqlDbType.VarChar, "Female");
				Write ("?DESC", MySqlDbType.VarChar, "Severe stroke.");
				Write ("?HGT", MySqlDbType.Int32, 166);
				Write ("?WGT", MySqlDbType.Int32, 67);
				CommitQuery ();

				SetQuery("insert into patient(DateOfBirth, Gender, Description, Height, Weight) values (?DOB, ?GEND, ?DESC, ?HGT, ?WGT)");
				Write ("?DOB", MySqlDbType.Date, System.Convert.ToDateTime("12/7/1961"));
				Write ("?GEND", MySqlDbType.VarChar, "Male");
				Write ("?DESC", MySqlDbType.VarChar, "Loss of motor functionality in hands.");
				Write ("?HGT", MySqlDbType.Int32, 189);
				Write ("?WGT", MySqlDbType.Int32, 79);
				CommitQuery ();
				EndQuery();

**/
				BeginQuery ("select * from patient");
				CommitQuery ();

				MySqlDataReader rdr = null;
				while( (rdr = Read ()) != null)
				{
					Debug.Log (rdr["PatientID"].ToString());
					Debug.Log (rdr["Gender"].ToString());
					Debug.Log (rdr["DateOfBirth"].ToString());
					Debug.Log (rdr["Description"].ToString());
				}
				EndQuery ();



				CloseConnection ();
			}

			
			public void OpenConnection(string config)
			{
				m_currentConfig = config;
				m_connection = new MySqlConnection (m_currentConfig);
				m_connection.Open ();
			}
			
			public void CloseConnection()
			{
				//MySqlConnection.ClearPool (m_connection);
				m_connection.Dispose();
				m_connection.Close ();					 
			}

			public bool OpenConnectionState
			{
				get { return m_connection.State == System.Data.ConnectionState.Open; }
			}

			public bool ClosedConnectionState
			{
				get { return m_connection.State == System.Data.ConnectionState.Closed; }
			}

			public void BeginQuery(string text = null)
			{
				if(m_command == null)
				{
					m_command = new MySqlCommand (text, m_connection);
				}
			}
			
			public void CommitQuery()
			{
				if(m_command != null)
				{
					try
					{
						m_command.ExecuteNonQuery ();
					}
					catch(MySqlException ex)
					{
						Debug.Log (ex.Message);
					}
				}
			}
			
			public void EndQuery()
			{
				if(m_command != null)
				{
					m_command.Dispose ();
					m_command = null;
				}
			}

			public void SetQuery (string query)
			{
				if(m_command != null)
				{
					m_command.CommandText = query;
				}
			}

			public string GetQueryResult()
			{
				string txt = null;
				MySqlDataReader rdr;

				while((rdr = Read ()) != null)
				{
					for(int i=0; i<rdr.FieldCount; i++)
					{
						txt += rdr.GetValue(i).ToString();
						txt += " ";
					}
					txt += "\n";
				}
				return txt;
			}

			public void Write(string param, MySqlDbType sqlType, object value)
			{
				if(m_command != null)
				{
					MySqlParameter sqlParam = m_command.Parameters.Add (param, sqlType);
					sqlParam.Value = value;
				}
			}

			public MySqlDataReader Read()
			{
				if(m_command == null)
				{
					return null;
				}

				if(m_reader == null)
				{
					m_reader = m_command.ExecuteReader();
				}

				if(m_reader.HasRows && m_reader.Read ())
				{
					return m_reader;
				}

				m_reader.Close();
				m_reader = null;

				return m_reader;
			}
		}
	}
}

