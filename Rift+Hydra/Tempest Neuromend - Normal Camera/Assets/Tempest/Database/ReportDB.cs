using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Tempest
{
	namespace Database
	{
		public class ReportDB
		{
			private SQLView m_sqlView;

			public struct Report
			{
				public int m_reportID;
				public string m_device;
				public string m_task;
				public System.DateTime m_timestamp; //some other format?
				public int m_score;
				public float m_sens;
				public float m_speed;

				public override string ToString ()
				{
					return m_reportID + "\n" +
						   m_device + "\n" +
						   m_task + "\n" +
						   m_timestamp + "\n" +
						   m_score + "\n" +
						   m_sens + "\n" +
						   m_speed;
				}
			}

			public ReportDB(SQLView view)
			{
				m_sqlView = view;
			}

			public void CreateRelation()
			{
				m_sqlView.BeginQuery("CREATE TABLE IF NOT EXISTS report(" +
				                     "ReportID INT NOT NULL AUTO_INCREMENT," +
				                     "Username VARCHAR(30) NOT NULL," +
				                     "DeviceName VARCHAR(30) NOT NULL," +
				                     "ActivityName VARCHAR(30) NOT NULL," +
				                     "CompletionDate DATETIME NOT NULL," +
				                     "Score INT NOT NULL," +
				                     "Sensitivity FLOAT(6,4) NOT NULL, " +
				                     "Speed FLOAT(6,4) NOT NULL, " +
				                     "CONSTRAINT PRIMARY KEY(ReportID), " +
				                     "CONSTRAINT FOREIGN KEY(DeviceName) REFERENCES device(DeviceName) ON UPDATE CASCADE ON DELETE CASCADE," +
				                     "CONSTRAINT FOREIGN KEY(ActivityName) REFERENCES activity(ActivityName) ON UPDATE CASCADE ON DELETE CASCADE," +
				                     "CONSTRAINT FOREIGN KEY(Username) REFERENCES patient(Username) ON UPDATE CASCADE ON DELETE CASCADE)" );
				m_sqlView.CommitQuery();
				m_sqlView.EndQuery ();
			}

			public void DropRelation()
			{
				m_sqlView.BeginQuery ("DROP TABLE report");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropRows()
			{
				m_sqlView.BeginQuery ("DELETE FROM report");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public bool AddReport(string username, string device, string activity, DateTime timestamp, int score, float sens, float speed)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO report(Username, DeviceName, ActivityName, CompletionDate, Score, Sensitivity, Speed) " +
				                     "VALUES(?USER, ?DEVICE, ?TASK, STR_TO_DATE(?TIME, '%d/%m/%Y %k:%i'), ?SCORE, ?SENS, ?SPEED)");

				m_sqlView.Write ("?USER", username);
				m_sqlView.Write ("?DEVICE", device);
				m_sqlView.Write ("?TASK", activity);
				m_sqlView.Write ("?TIME", timestamp.ToString("d/M/yyyy HH:mm")); //???
				m_sqlView.Write ("?SCORE", score);
				m_sqlView.Write ("?SENS", sens);
				m_sqlView.Write ("?SPEED", speed);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}

			public bool AddReport(string levelData)
			{
				XElement root = XElement.Load (@levelData);
		
				if(root != null)
				{
					string username = root.Element("Username").Value;
					string device = root.Element("Controller").Value;
					string activity = root.Element("Level").Value;
					string time = root.Element("Timestamp").Value;
					string score = root.Element("Score").Value;
					string sens = root.Element("Sensitivity").Value;
					string speed = root.Element("Speed").Value;

					return AddReport(username, device, activity, 
					       	DateTime.ParseExact(time, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
					       	Int32.Parse(score), float.Parse(sens), float.Parse(speed));
				}
				return false;
			}

			public bool DeleteReport(int reportID)
			{
				m_sqlView.BeginQuery ("DELETE FROM report WHERE ReportID = @ID");
				m_sqlView.Write ("ID", reportID);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}

			public bool FindReport(int reportID)
			{
				m_sqlView.BeginQuery ("SELECT COUNT(1) FROM report WHERE ReportID = @ID");
				m_sqlView.Write ("ID", reportID);

				object obj = m_sqlView.CommitScalar ();
				m_sqlView.EndQuery ();

				return Convert.ToInt32 (obj) > 0;
			}

			public void ExtractReport(string username, string csv_filename, string fieldBreak, string lineBreak, string enclose)
			{
				m_sqlView.GrantFilePrivileges ();

				m_sqlView.BeginQuery ("SELECT * FROM report " +
				                      "WHERE CAST(Username AS BINARY) = @username " +
				                      "ORDER BY CompletionDate " + 
				                      "INTO OUTFILE @csv " +
				                      "FIELDS TERMINATED BY @field_break " +
				                      "ENCLOSED BY @enclose " +
				                      "LINES TERMINATED BY @line_break");

				m_sqlView.Write ("username", username);
				m_sqlView.Write ("csv", csv_filename);
				m_sqlView.Write ("field_break", fieldBreak);
				m_sqlView.Write ("line_break", lineBreak);
				m_sqlView.Write ("enclose", enclose);

				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void ExtractReport(string username, List<Report> list)
			{
				m_sqlView.BeginQuery ("SELECT *, DATE_FORMAT(CompletionDate, '%d/%m/%Y %k:%i') " +
									  "FROM report " +
				                      "WHERE CAST(Username AS BINARY) = @username " +
				                      "ORDER BY CompletionDate DESC");

				m_sqlView.Write ("username", username);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = null;

				while((rdr = m_sqlView.Read()) != null)
				{
					Report rep = new Report();

					rep.m_reportID = rdr.GetInt32("ReportID");
					rep.m_task = rdr.GetString("ActivityName");
					rep.m_device = rdr.GetString("DeviceName");
					rep.m_timestamp = rdr.GetDateTime("CompletionDate");
					rep.m_score = rdr.GetInt32("Score");
					rep.m_sens = rdr.GetFloat("Sensitivity");
					rep.m_speed = rdr.GetFloat("Speed");
				
					list.Add(rep);
				}
		
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();
			}
		}
	}
}

