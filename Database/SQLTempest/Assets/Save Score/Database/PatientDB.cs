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
				private string m_password;
			}
			
			public PatientDB(SQLView view)
			{
				m_sqlView = view;
				CreateRelation ();
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
				m_sqlView.BeginQuery ("DELETE FROM patient");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void AddPatient(string username, string password, string birthDate, string gender, string medicalCondition)
			{
				m_sqlView.BeginQuery("INSERT INTO patient(Username, Password, Gender, MedicalCondition, BirthDate) " +
					"VALUES (?USER, ?PASS, ?GND, ?COND, STR_TO_DATE(?BIRTH,'%d/%m/%Y'))");

				m_sqlView.Write ("?USER", username);
				m_sqlView.Write ("?PASS", password);
			    m_sqlView.Write ("?BIRTH", birthDate);
				m_sqlView.Write ("?GND", gender);
				m_sqlView.Write ("?COND",  medicalCondition);

				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}
			
			public void DeletePatient(string username)
			{
				m_sqlView.BeginQuery ("DELETE FROM patient WHERE Username = @username");
				m_sqlView.Write ("username", username);

				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}
			
			public void ReadPatient(string username, ref Patient patient)
			{
				m_sqlView.BeginQuery ("SELECT * FROM patient WHERE Username = @username");
				m_sqlView.Write ("username", username);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();
				if(rdr != null)
				{
					patient.m_username = rdr.GetString("Username");
					patient.m_gender = rdr.GetString("Gender");
					patient.m_birthDate = rdr.GetDateTime("BirthDate").ToShortDateString();
					patient.m_medicalCondition = rdr.GetString("MedicalCondition");
				}
				m_sqlView.EndRead ();

				m_sqlView.EndQuery ();
			}

			public bool FindPatient(string username)
			{
				m_sqlView.BeginQuery ("SELECT COUNT(1) FROM patient WHERE Username = @username");
				m_sqlView.Write ("username", username);

				object obj = m_sqlView.CommitScalar();
				m_sqlView.EndQuery ();
				
				return Convert.ToInt32 (obj) > 0;
			}
			
			public void UpdatePatient(string username, string birthDate, string condition, string gender)
			{
				m_sqlView.BeginQuery ("UPDATE patient SET " +
				                      "Gender = @gender, " +
				                      "BirthDate = STR_TO_DATE(@birthDate, '%d/%m/%Y'), " +
				                      "MedicalCondition = @medicalCondition " +
				                      "WHERE Username = @username");

				m_sqlView.Write ("gender", gender);
				m_sqlView.Write ("medicalCondition", condition);
				m_sqlView.Write ("birthDate", birthDate);
				m_sqlView.Write ("username", username);

				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}
		}
	}
}
