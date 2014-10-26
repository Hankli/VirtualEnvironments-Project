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
			private string m_latestError = null;
			private MySqlConnection m_connection = null;
			private MySqlCommand m_command = null;
			private MySqlDataReader m_reader = null;

			public string LatestError
			{
				get { return m_latestError; }
			}

			public void OpenConnection(string config)
			{
				if(m_connection != null)
				{
					CloseConnection();
					m_connection.ConnectionString = config;
				}
				else
				{
					m_connection = new MySqlConnection (config);
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

			public string GetQueryResult()
			{
				string txt = null;
				MySqlDataReader rdr;

				BeginRead ();
				while((rdr = Read ()) != null)
				{
					for(int i=0; i<rdr.FieldCount; i++)
					{
						txt += rdr.GetValue(i).ToString();
						txt += " ";
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

