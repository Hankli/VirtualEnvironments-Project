using UnityEngine;
using System.Collections;
using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Tempest
{
	namespace Database
	{
		public class SQLView
		{
			private string m_latestError = "";
			private string m_latestResult = "";

			private MySqlConnection m_connection = null;
			private MySqlCommand m_command = null;
			private MySqlDataReader m_reader = null;
			private MySqlConnectionStringBuilder m_connectionString = null;

			public string LatestError
			{
				get { return m_latestError; }
			}

			public string LatestResult
			{
				get { return m_latestResult; }
			}

			public void GrantFilePrivileges()
			{
				string q = "GRANT ALL PRIVILEGES ON @Database TO @UserID" + "@'%' " + 
					"IDENTIFIED BY @Password";
			
				BeginQuery (q);
				Write ("UserID", m_connectionString.UserID);
				Write ("Database", m_connectionString.Database);
				Write ("Password", m_connectionString.Password);
				CommitQuery ();

				q = "FLUSH PRIVILEGES";

				BeginQuery(q);
				CommitQuery();

				//q = "GRANT FILE *.* ON @UserID" + "@'%'";
				//BeginQuery (q);
				//Write ("UserID", m_connectionString.UserID);
			//	CommitQuery ();

				EndQuery ();
			}

			public void OpenConnection(string config)
			{
				if(m_connection != null)
				{
					CloseConnection();
					m_connection.ConnectionString = config;
					m_connectionString.ConnectionString = config;
				}
				else
				{
					m_connection = new MySqlConnection (config);
					m_connectionString = new MySqlConnectionStringBuilder(config);
				}

				try
				{
					m_connection.Open ();
				}
				catch(MySqlException ex)
				{
					m_latestError = ex.Message;
				}
			}

			public string Database
			{
				get 
				{
					if(m_connectionString != null)
					{
						return m_connectionString.Database;
					}
					return null;
				}
			}

			public string Server
			{
				get 
				{
					if(m_connectionString != null)
					{
						return m_connectionString.Server;
					}
					return null;
				}
			}

			public string UserID
			{
				get 
				{
					if(m_connectionString != null)
					{
						return m_connectionString.UserID;
					}
					return null;
				}
			}
			
			public void CloseConnection()
			{
				//MySqlConnection.ClearPool (m_connection);

				if(m_connection != null)
				{
					m_connection.Dispose();
					m_connection.Close ();
				}
			}

			public string DescribeRelation(string tablename)
			{
				BeginQuery ("describe " + tablename);
				CommitQuery ();

				string query = GetQueryResult ();
				EndQuery ();

				return query;
			}

			public bool OpenConnectionState
			{
				get 
				{ 
					if(m_connection != null)
					{
						return m_connection.State.Equals (System.Data.ConnectionState.Open);
					}
					else return false;
				}
			}

			public bool ClosedConnectionState
			{
				get 
				{
					if(m_connection != null)
					{
						return m_connection.State.Equals (System.Data.ConnectionState.Closed);
					}
					else return false;
				}
			}
			
			public int CommitQuery()
			{
				try
				{
					int rowsAffected = m_command.ExecuteNonQuery ();
					m_latestError = null; //no errors if we got here

					return rowsAffected;
				}

				catch(MySqlException ex)
				{
					Debug.Log (ex.Message);
					m_latestError = ex.Message;

					return 0; 
				}
			}

			public string ConnectionSettings
			{
				get { return m_connection != null ? m_connection.ConnectionString : null; }
			}

			public object CommitScalar()
			{
				return m_command.ExecuteScalar();
			}
			
			public void EndQuery()
			{
				m_command.Dispose ();
				m_command = null;
			}

			public void BeginQuery (string query)
			{
				m_command = new MySqlCommand (query, m_connection);
				m_command.CommandText = query;
				m_command.Prepare ();
			}

			private string GetQueryResult()
			{
				string txt = null;
				MySqlDataReader rdr;

				BeginRead ();
				while((rdr = Read ()) != null)
				{
					for(int i=0; i<rdr.FieldCount; i++)
					{
						txt += rdr.GetValue(i).ToString();

						if(i < rdr.FieldCount - 1)
						{
							txt += ", ";
						}

					}
					txt += "\n";
				}
				EndRead ();

				return txt;
			}

			public void Write(string param, object value)
			{
				m_command.Parameters.AddWithValue (param, value);
			}

			public void BeginRead()
			{
				m_reader = m_command.ExecuteReader();
			}

			public MySqlDataReader Read()
			{
				if(m_reader.HasRows && m_reader.Read ())
				{
					return m_reader;
				}
				return null;
			}

			public void EndRead()
			{
				m_reader.Close ();
				m_reader = null;
			}
		}
	}
}

