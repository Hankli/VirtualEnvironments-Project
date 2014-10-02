using UnityEngine;
using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Tempest
{
	namespace Database
	{
		public class ActivityDB
		{
			private SQLView m_sqlView = null;
			
			public struct Activity
			{
				public string m_activityName;
				public string m_description;

				public override string ToString ()
				{
					return "Activity Type: " + m_activityName + '\n' +
						   "Description: " + m_description;
				}
			}
			
			public ActivityDB(SQLView view)
			{
				m_sqlView = view;
			}

			public void CreateRelation()
			{
				m_sqlView.BeginQuery("CREATE TABLE IF NOT EXISTS activity(" +
				                     "ActivityName VARCHAR(30)," +
				                     "Description VARCHAR(200), " +
				                     "CONSTRAINT PRIMARY KEY(ActivityName))");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropRelation()
			{
				m_sqlView.BeginQuery ("DROP TABLE activity");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropTuples()
			{
				m_sqlView.BeginQuery ("DELETE FROM activity");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}
			
			public bool AddActivity(string activityName, string description)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO activity(ActivityName, Description) VALUES(?NAME, ?DESCR)");
				m_sqlView.Write ("?NAME", activityName);
				m_sqlView.Write ("?DESCR", description);
				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}

			public bool FindActivity(string activityName)
			{
				m_sqlView.BeginQuery ("SELECT COUNT(1) FROM activity WHERE ActivityName = @activityName");
				m_sqlView.Write ("activityName", activityName);

				object obj = m_sqlView.CommitScalar();
				m_sqlView.EndQuery ();

				return Convert.ToInt32 (obj) > 0;
			}

			public bool ReadActivity(string activityName, ref Activity activity)
			{
				m_sqlView.BeginQuery ("SELECT * FROM activity WHERE ActivityName = @activityName");
				m_sqlView.Write ("activityName", activityName);
				m_sqlView.CommitQuery ();
					
				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();
				if(rdr != null)
				{
					activity.m_activityName = rdr.GetString("ActivityName");
					activity.m_description = rdr.GetString("Description");
				}
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();

				return rdr != null;
			}

			
			public bool UpdateActivity(string activityName, string name, string descr)
			{
				m_sqlView.BeginQuery ("UPDATE activity SET " +
				                      "ActivityName = @name, " +
				                      "Description = @descr " +    
				                      "WHERE ActivityName = @activityName");

				m_sqlView.Write ("name", name); 
				m_sqlView.Write ("descr", descr);
				m_sqlView.Write ("activityName", activityName);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
			
			public bool DeleteActivity(string activityName)
			{
				m_sqlView.BeginQuery("DELETE FROM activity WHERE ActivityName = @activityName");
				m_sqlView.Write ("activityName", activityName);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
		}
	}
}
