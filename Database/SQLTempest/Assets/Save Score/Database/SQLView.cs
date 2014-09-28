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
			private string m_currentConfig = null;

			private MySqlConnection m_connection = null;
			private MySqlCommand m_command = null;
			private MySqlDataReader m_reader = null;

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
			
			public int CommitQuery()
			{
				try
				{
					return m_command.ExecuteNonQuery ();
				}

				catch(MySqlException ex)
				{
					return ex.ErrorCode;
				}
			}

			public string ConnectionServer
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

