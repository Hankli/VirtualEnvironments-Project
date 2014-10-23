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

				public override string ToString ()
				{
					return m_reportID + "\n" +
						   m_device + "\n" +
						   m_task + "\n" +
						   m_timestamp + "\n" +
						   m_score;
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
				                     "Username VARCHAR(20) NOT NULL," +
				                     "DeviceName VARCHAR(30) NOT NULL," +
				                     "ActivityName VARCHAR(30) NOT NULL," +
				                     "EndDate DATETIME NOT NULL," +
				                     "Score INT NOT NULL," +
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

			public bool AddReport(string username, string device, string activity, DateTime timestamp, int score)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO report(Username, DeviceName, ActivityName, EndDate, Score) " +
				                     "VALUES(?USER, ?DEVICE, ?TASK, STR_TO_DATE(?ENDTIME, '%d/%m/%Y %k:%i'), ?SCORE)");

				m_sqlView.Write ("?USER", username);
				m_sqlView.Write ("?DEVICE", device);
				m_sqlView.Write ("?TASK", activity);
				m_sqlView.Write ("?ENDTIME", timestamp.ToString("d/M/yyyy HH:mm")); //???
				m_sqlView.Write ("?SCORE", score);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}

			public void AddReport(string levelDataXML)
			{
				XElement root = XElement.Load (@levelDataXML);

				if(root != null)
				{
					foreach(XElement xe in root.Elements("Level Summary"))
					{
						string username = xe.Element("Username").Value;
						string device = xe.Element("Controller").Value;
						string activity = xe.Element("Level").Value;
						string time = xe.Element("EndDate").Value;
						string score = xe.Element("Score").Value;
					
						AddReport(username, device, activity, Convert.ToDateTime(time), Int32.Parse(score));
					}
				}
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


			public void ExtractReport(string username, List<Report> list)
			{
				m_sqlView.BeginQuery ("SELECT *, DATE_FORMAT(EndDate, '%d/%m/%Y %k:%i') " +
									  "FROM report " +
						              "WHERE Username = @user " +
				                      "ORDER BY EndDate DESC");

				m_sqlView.Write ("user", username);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = null;

				while((rdr = m_sqlView.Read()) != null)
				{
					Report rep = new Report();

					rep.m_reportID = rdr.GetInt32("ReportID");
					rep.m_task = rdr.GetString("ActivityName");
					rep.m_device = rdr.GetString("DeviceName");
					rep.m_score = rdr.GetInt32("Score");
					rep.m_timestamp = rdr.GetDateTime("EndDate");
				
					list.Add(rep);
				}
		
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();
			}
		}
	}
}
