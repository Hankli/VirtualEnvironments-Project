using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;
namespace Tempest
{
	namespace Database
	{
		public class DeviceDB
		{
			private SQLView m_sqlView = null;
			
			public struct Device
			{
				public string m_deviceName;
				public string m_description;

				public override string ToString ()
				{
					return m_deviceName + '\n' +
						   m_description;
				}
			}
			
			public DeviceDB(SQLView view)
			{
				m_sqlView = view;
			}

			public void CreateRelation()
			{
				m_sqlView.BeginQuery("CREATE TABLE IF NOT EXISTS device(" +
				                     "DeviceName VARCHAR(30)," +
				                     "Description VARCHAR(200), " +
				                     "CONSTRAINT PRIMARY KEY(DeviceName))");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropRelation()
			{
				m_sqlView.BeginQuery ("DROP TABLE device");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}

			public void DropRows()
			{
				m_sqlView.BeginQuery ("DELETE FROM device");
				m_sqlView.CommitQuery ();
				m_sqlView.EndQuery ();
			}
			
			public bool AddDevice(string deviceName, string description)
			{
				m_sqlView.BeginQuery("INSERT IGNORE INTO device(DeviceName, Description) VALUES (?NAME, ?DESCR)");

				m_sqlView.Write ("?NAME", deviceName);
				m_sqlView.Write ("?DESCR", description);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
			
			public bool UpdateDevice(string deviceName, string name, string descr)
			{
				m_sqlView.BeginQuery ("UPDATE device SET " +
				                      "DeviceName = @name, " +
				                      "Description = @descr " +    
				                      "WHERE DeviceName = @deviceName");

				m_sqlView.Write ("name", name); 
				m_sqlView.Write ("descr", descr);
				m_sqlView.Write ("deviceName", deviceName);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();
			
				return success;
			}

			public bool ExtractDevice(string deviceName, ref Device device)
			{
				m_sqlView.BeginQuery ("SELECT * FROM device WHERE DeviceName = @deviceName");
				m_sqlView.Write("deviceName", deviceName);
				m_sqlView.CommitQuery ();

				m_sqlView.BeginRead ();
				MySqlDataReader rdr = m_sqlView.Read ();
				if(rdr != null)
				{
					device.m_deviceName = rdr.GetString("DeviceName");
					device.m_description = rdr.GetString("Description");
				}
				m_sqlView.EndRead ();
				m_sqlView.EndQuery ();

				return rdr != null;
			}

			public bool FindDevice(string deviceName)
			{
				m_sqlView.BeginQuery ("SELECT COUNT(1) FROM device WHERE DeviceName = @deviceName");
				m_sqlView.Write ("deviceName", deviceName);
				
				object obj = m_sqlView.CommitScalar();
				m_sqlView.EndQuery ();
				
				return Convert.ToInt32 (obj) > 0;
			}
			
			public bool DeleteDevice(string deviceName)
			{
				m_sqlView.BeginQuery ("DELETE FROM device WHERE DeviceName = @deviceName");
				m_sqlView.Write ("deviceName", deviceName);

				bool success = (m_sqlView.CommitQuery () > 0);
				m_sqlView.EndQuery ();

				return success;
			}
		}
	}
}
