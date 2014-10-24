using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Tempest
{
	namespace Database
	{
		public class PatientDB
		{
			private SQLView m_sqlView = null;
			
			public struct Patient
			{
				public string m_birthDate;
				public string m_gender;
				public string m_medicalCondition;
				public string m_username;
				public string m_password;

				public override string ToString ()
				{
					return m_username + '\n' +
						   m_gender + '\n' +
						   m_birthDate + '\n' +
						   m_medicalCondition;
				}
			}
			
			public PatientDB(SQLView view)
			{
				m_sqlView = view;
			}

			public void CreateRelation()
			{
				m_sqlView.BeginQuery ("CREATE TABLE IF NOT EXISTS patient(" +
										"Username VARCHAR(20) NOT NULL, " +
										"Password VARCHAR(20) NOT NULL, " +
										"Gender VARCHAR(10) DEFAULT NULL, " +
										"MedicalCondition VARCHAR(200) DEFAULT NULL, " +
										"BirthDate DATE DEFAULT NULL, " +
										"CONSTRAINT PRIMARY KEY (Username))"); 
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}


			public void DropRelation()
			{
				m_sqlView.BeginQuery ("DROP TABLE patient");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropRows()
			{
				m_sqlView.BeginQuery ("TRUNCATE TABLE patient");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public bool AddPatient(string username, string password, string birthDate, string gender, string medicalCondition)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO patient(Username, Password, Gender, MedicalCondition, BirthDate) " +
					"VALUES (?USER, ?PASS, ?GND, ?COND, STR_TO_DATE(?BIRTH,'%d/%m/%Y'))");

				m_sqlView.Write ("?USER", username);
				m_sqlView.Write ("?PASS", password);
			    m_sqlView.Write ("?BIRTH", birthDate);
				m_sqlView.Write ("?GND", gender);
				m_sqlView.Write ("?COND",  medicalCondition);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
			
			public bool DeletePatient(string username, string password)
			{
				m_sqlView.BeginQuery ("DELETE FROM patient WHERE CAST(Username AS BINARY) = @username AND" +
				                      " CAST(Password AS BINARY) = @password");
				m_sqlView.Write ("username", username);
				m_sqlView.Write ("password", password);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}

			public bool ExtractPatient(string username, ref Patient patient)
			{
				m_sqlView.BeginQuery ("SELECT *, DATE_FORMAT('BirthDate', '%d/%m/%Y') " +
				                      "FROM patient " +
				                      "WHERE CAST(Username AS BINARY) = @username");

				m_sqlView.Write ("username", username);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();
				if(rdr != null)
				{
					patient.m_username = rdr.GetString("Username");
					patient.m_password = rdr.GetString("Password");
					patient.m_gender = rdr.GetString("Gender");
					patient.m_birthDate = rdr.GetDateTime("BirthDate").ToShortDateString();
					patient.m_medicalCondition = rdr.GetString("MedicalCondition");
				}
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();
				
				return rdr != null;
			}
			
			public bool ExtractPatient(string username, string password, ref Patient patient)
			{
				m_sqlView.BeginQuery ("SELECT *, DATE_FORMAT('BirthDate', '%d/%m/%Y') " +
									  "FROM patient " +
				                      "WHERE CAST(Username AS BINARY) = @username AND" +
				                      " CAST(Password AS BINARY) = @password");

				m_sqlView.Write ("username", username);
				m_sqlView.Write ("password", password);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();
				if(rdr != null)
				{
					patient.m_username = rdr.GetString("Username");
					patient.m_password = rdr.GetString("Password");
					patient.m_gender = rdr.GetString("Gender");
					patient.m_birthDate = rdr.GetDateTime("BirthDate").ToShortDateString();
					patient.m_medicalCondition = rdr.GetString("MedicalCondition");
				}
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();

				return rdr != null;
			}

			public bool FindPatient(string username, string password)
			{
				m_sqlView.BeginQuery ("SELECT COUNT(1) FROM patient WHERE CAST(Username AS BINARY) = @username AND" +
					" CAST(Password AS BINARY) = @password");
				m_sqlView.Write ("username", username);
				m_sqlView.Write ("password", password);

				object obj = m_sqlView.CommitScalar();
				m_sqlView.EndQuery ();
				
				return Convert.ToInt32 (obj) > 0;
			}
			
			public bool UpdatePatient(string username, string password, string birthDate, string condition, string gender)
			{
				m_sqlView.BeginQuery ("UPDATE patient SET " +
				                      "Gender = @gender, " +
				                      "BirthDate = STR_TO_DATE(@birthDate, '%d/%m/%Y'), " +
				                      "MedicalCondition = @medicalCondition " +
				                      "WHERE CAST(Username AS BINARY) = @username AND" +
				                      " CAST(Password AS BINARY) = @password");

				m_sqlView.Write ("gender", gender);
				m_sqlView.Write ("medicalCondition", condition);
				m_sqlView.Write ("birthDate", birthDate);
				m_sqlView.Write ("username", username);
				m_sqlView.Write ("password", password);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
		}
	}
}
