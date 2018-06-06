using System;
using System.Collections.Generic;
using System.Data; // need this for ConnectionState.Open
using System.Data.SqlClient; // need this for SqlConnection class
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToSQL {
	class Program {
		static void Main(string[] args) {


			// opens a connection from .NET to SQL Server Management
			string connStr = @"server=localhost\SQLEXPRESS;database=prssql;Trusted_connection=true";
			SqlConnection conn = new SqlConnection(connStr);
			conn.Open();

			// checks to see if the connection was successful and throws an error if not
			if (conn.State != ConnectionState.Open) {
				throw new ApplicationException("Connection did not open");
			}

			// selects the information that should be read
			string sql = "select * from [user]";
			// gives the command to the SqlCommand class. Tells it what to command (sql) and where to command it (conn) 
			SqlCommand sqlcommand = new SqlCommand(sql, conn);
			// executes the command. Since the command was only reading data, use SqlDataReader
			SqlDataReader reader = sqlcommand.ExecuteReader();
			// if there is data to be read, do whatever is in the while loops. If there is no data to be read, the statement is set to false
			while (reader.Read()) {
				int id = reader.GetInt32(reader.GetOrdinal("id"));
				string username = reader.GetString(reader.GetOrdinal("username"));
				Debug.WriteLine($"{id}, {username}");
			}

			// closes the connection after what we need to do is done
			conn.Close();
		}
	}
}
