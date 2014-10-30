using System;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

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
				public string m_device;
				public string m_task;
				public System.DateTime m_timestamp; //some other format?
				public int m_score;
				public float m_sens;
				public float m_speed;

				public override string ToString ()
				{
					return m_device + "\n" +
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

			public bool AddReport(string username, string device, string level, DateTime date, int score, float sens, float speed)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO report(Username, DeviceName, ActivityName, CompletionDate, Score, Sensitivity, Speed) " +
				                     "VALUES(?USER, ?DEVICE, ?LEVEL, STR_TO_DATE(?DATE, '%d/%m/%Y %k:%i:%s'), ?SCORE, ?SENS, ?SPEED)");

				m_sqlView.Write ("?USER", username);
				m_sqlView.Write ("?DEVICE", device);
				m_sqlView.Write ("?LEVEL", level);
				m_sqlView.Write ("?DATE", date.ToString("d/M/yyyy HH:mm:ss")); //???
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
					string level = root.Element("Level").Value;
					string time = root.Element("Timestamp").Value;
					string score = root.Element("Score").Value;
					string sens = root.Element("Sensitivity").Value;
					string speed = root.Element("Speed").Value;

					DateTime date = DateTime.ParseExact(time, "d/M/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

					return AddReport(username, device, level, date, Int32.Parse(score), float.Parse(sens), float.Parse(speed));
				}
				return false;
			}

			public bool DeleteReport(int reportID)
			{
				m_sqlView.BeginQuery ("DELETE FROM Report WHERE ReportID = @ID");
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

			public bool ExtractReport(string username, string csv_path, string csv_file)
			{
				m_sqlView.BeginQuery("SELECT *, DATE_FORMAT(CompletionDate, '%d/%m/%Y %k:%i:%s')" +
						"FROM Report " +
				    	"WHERE CAST(Username AS BINARY) = @User " +
				    	"ORDER BY CompletionDate DESC");

				m_sqlView.Write ("User", username);
				m_sqlView.CommitQuery ();

				DataTable dt = new DataTable ("Progression");
				dt.Columns.Add (new DataColumn ("Device", typeof(string)));
				dt.Columns.Add (new DataColumn ("Level", typeof(string)));
				dt.Columns.Add (new DataColumn ("Date", typeof(string)));
				dt.Columns.Add (new DataColumn ("Score", typeof(int)));
				dt.Columns.Add (new DataColumn ("Sensitivity", typeof(float)));
				dt.Columns.Add (new DataColumn ("Speed", typeof(float)));

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();

				if(rdr == null)
				{
					m_sqlView.EndRead();
					m_sqlView.EndQuery();

					return false;
				}

				while(rdr != null)
				{
					DataRow dr = dt.NewRow();

					dr["Device"] = rdr.GetString("DeviceName");
					dr["Level"] = rdr.GetString("ActivityName");
					dr["Date"] = rdr.GetDateTime("CompletionDate").ToString("d/M/yyyy HH:mm:ss");
					dr["Score"] = rdr.GetInt32("Score");
					dr["Sensitivity"] = rdr.GetFloat("Sensitivity");
					dr["Speed"] = rdr.GetFloat("Speed");

					dt.Rows.Add(dr);

					rdr = m_sqlView.Read();
				}

				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();

				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				int index = 0;

				foreach(DataColumn col in dt.Columns)
				{
					sb.Append(col.ColumnName);

					if(index++ < dt.Columns.Count - 1)
					{
						sb.Append(",");
					}
				}

				sb.Append (System.Environment.NewLine);
				index = 0;

				foreach(DataRow row in dt.Rows)
				{
					string[] items = row.ItemArray.Select(x => x.ToString()).ToArray();
					string line = string.Join(",", items);
					sb.Append(line);

					if(index++ < dt.Rows.Count - 1)
					{
						sb.Append(System.Environment.NewLine);
					}
				}
			
				try
				{
					if(!System.IO.Directory.Exists(csv_path))
					{
						System.IO.Directory.CreateDirectory(csv_path);
					}

					string filename = csv_path + "/" + csv_file;
					if(!System.IO.File.Exists(filename))
					{
						System.IO.File.Create (filename);
					}

					System.IO.File.WriteAllText (filename, sb.ToString ());
				}
				catch(System.Exception)
				{

					return false;
				}

				return true;
			}

			public bool ExtractReport(string username, List<Report> list)
			{
				m_sqlView.BeginQuery ("SELECT *, DATE_FORMAT(CompletionDate, '%d/%m/%Y %k:%i:%s') " +
									  "FROM Report " +
				                      "WHERE CAST(Username AS BINARY) = @User " +
				                      "ORDER BY CompletionDate DESC");

				m_sqlView.Write ("User", username);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read();

				if(rdr == null)
				{
					m_sqlView.EndRead();
					m_sqlView.EndQuery();

					return false;
				}

				while(rdr != null)
				{
					Report rp = new Report();

					rp.m_task = rdr.GetString("ActivityName");
					rp.m_device = rdr.GetString("DeviceName");
					rp.m_timestamp = rdr.GetDateTime("CompletionDate");
					rp.m_score = rdr.GetInt32("Score");
					rp.m_sens = rdr.GetFloat("Sensitivity");
					rp.m_speed = rdr.GetFloat("Speed");
				
					list.Add(rp);
					rdr = m_sqlView.Read();
				}
		
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();

				return true;
			}
		}
	}
}

