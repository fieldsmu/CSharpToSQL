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

		static List<User> users = new List<User>();

		void Run() {
			User user = new User() {
				Id = 42,
				Username = "fieldsmu1828",
				Password = "pard",
				Firstname = "marcus",
				Lastname = "fields",
				Phone = "513-444-4444",
				Email = "johndoe@gmail.com",
				IsReviewer = true,
				IsAdmin = true
			};
			Update(user);
		}

		static void Main(string[] args) {
			(new Program()).Run();
			(new Program()).Select();
			//(new Program()).DeleteAll();
			//(new Program()).Update();
			//(new Program()).Insert("user4", "password", "marcus", "fields", "513-444-4444", "johndoe@gmail.com", 1, 1);
			//(new Program()).Insert("user3", "password", "riana", "mallin", "513-444-4444", "janedoe@gmail.com", 1, 1);
			//(new Program()).Insert("user2", "password", "greg", "doud", "513-444-4444", "johndoe@gmail.com", 1, 1);
		}

		void Update(User user) {
			Connect();
			// update user
			string sql = "update [user] " +
				"set username = @username, " +
				"password = @password, " +
				"firstname = @firstname, " +
				"lastname = @lastname, " +
				"phone = @phone, " +
				"email = @email, " +
				"isreviewer = @isreviewer, " +
				"isadmin = @isadmin " +
				"where id = @id;";
			// gives the command to the SqlCommand class. Tells it what to command (sql) and where to command it (conn) 
			SqlCommand sqlcommand = new SqlCommand(sql, Connect());
			// create and set parameters in SQL
			sqlcommand.Parameters.Add(new SqlParameter("@id", user.Id));
			sqlcommand.Parameters.Add(new SqlParameter("@username", user.Username));
			sqlcommand.Parameters.Add(new SqlParameter("@password", user.Password));
			sqlcommand.Parameters.Add(new SqlParameter("@firstname", user.Firstname));
			sqlcommand.Parameters.Add(new SqlParameter("@lastname", user.Lastname));
			sqlcommand.Parameters.Add(new SqlParameter("@phone", user.Phone));
			sqlcommand.Parameters.Add(new SqlParameter("@email", user.Email));
			sqlcommand.Parameters.Add(new SqlParameter("@isreviewer", user.IsReviewer));
			sqlcommand.Parameters.Add(new SqlParameter("@isadmin", user.IsAdmin));
			// actually executes the command
			int recordsaffected = sqlcommand.ExecuteNonQuery();
			// checks if the command was carried out successfully
			if (recordsaffected != 1) {
				Debug.WriteLine("Record insert failed");
			}
			// closes the connection after what we need to do is done
			Connect().Close();
		}

		SqlConnection Connect() {
			// opens a connection from .NET to SQL Server Management
			string connStr = @"server=localhost\SQLEXPRESS;database=prssql;Trusted_connection=true";
			SqlConnection conn = new SqlConnection(connStr);
			conn.Open();

			// checks to see if the connection was successful and throws an error if not
			if (conn.State != ConnectionState.Open) {
				throw new ApplicationException("Connection did not open");
			}
			return conn;
		}

		void DeleteAll() {
			Connect();
			// insert into user
			string sql = "delete from [user]";
			// gives the command to the SqlCommand class. Tells it what to command (sql) and where to command it (conn) 
			SqlCommand sqlcommand = new SqlCommand(sql, Connect());
			// actually executes the command
			int recordsaffected = sqlcommand.ExecuteNonQuery();
			// closes the connection after what we need to do is done
			Connect().Close();
		}

		void Insert(User user) {
		//void Insert(string username, string password, string firstname, string lastname, string phone, string email, int isreviewer, int isadmin) {
			Connect();

			// insert into user
			//string sql = "insert into [user] (username, password, firstname, lastname, phone, email, isreviewer, isadmin)" +
			//	$"values ('{username}', '{password}', '{firstname}', '{lastname}', '{phone}', '{email}', {isreviewer}, {isadmin})";
			
			// insert into user
			string sql = "insert into [user] (username, password, firstname, lastname, phone, email, isreviewer, isadmin)" +
				$"values (@username, @password, @firstname, @lastname, @phone, @email, @isreviewer, @isadmin)";
			// gives the command to the SqlCommand class. Tells it what to command (sql) and where to command it (conn) 
			SqlCommand sqlcommand = new SqlCommand(sql, Connect());
			// create and set parameters in SQL
			sqlcommand.Parameters.Add(new SqlParameter("@username", user.Username));
			sqlcommand.Parameters.Add(new SqlParameter("@password", user.Password));
			sqlcommand.Parameters.Add(new SqlParameter("@firstname", user.Firstname));
			sqlcommand.Parameters.Add(new SqlParameter("@lastname", user.Lastname));
			sqlcommand.Parameters.Add(new SqlParameter("@phone", user.Phone));
			sqlcommand.Parameters.Add(new SqlParameter("@email", user.Email));
			sqlcommand.Parameters.Add(new SqlParameter("@isreviewer", user.IsReviewer));
			sqlcommand.Parameters.Add(new SqlParameter("@isadmin", user.IsAdmin));
			// actually executes the command
			int recordsaffected = sqlcommand.ExecuteNonQuery();
			// checks if the command was carried out successfully
			if(recordsaffected != 1) {
				Debug.WriteLine("Record insert failed");
			}
			// closes the connection after what we need to do is done
			Connect().Close();
		}

		void Select() {
			Connect();
			// selects the information that should be read
			string sql = "select * from [user]";
			// gives the command to the SqlCommand class. Tells it what to command (sql) and where to command it (conn) 
			SqlCommand sqlcommand = new SqlCommand(sql, Connect());
			// executes the command. Since the command was only reading data, use SqlDataReader
			SqlDataReader reader = sqlcommand.ExecuteReader();
			// if there is data to be read, do whatever is in the while loops. If there is no data to be read, the statement is set to false
			while (reader.Read()) {
				int id = reader.GetInt32(reader.GetOrdinal("id"));
				string username = reader.GetString(reader.GetOrdinal("username"));
				string password = reader.GetString(reader.GetOrdinal("password"));
				string firstname = reader.GetString(reader.GetOrdinal("firstname"));
				string lastname = reader.GetString(reader.GetOrdinal("lastname"));
				string phone = reader.GetString(reader.GetOrdinal("phone"));
				string email = reader.GetString(reader.GetOrdinal("email"));
				bool isreviewer = reader.GetBoolean(reader.GetOrdinal("isreviewer"));
				bool isadmin = reader.GetBoolean(reader.GetOrdinal("isadmin"));
				bool active = reader.GetBoolean(reader.GetOrdinal("active"));
				// adds the new user to the User class 
				User user = new User() {
					Id = id,
					Username = username,
					Password = password,
					Firstname = firstname,
					Lastname = lastname,
					Phone = phone,
					Email = email,
					IsReviewer = isreviewer,
					IsAdmin = isadmin,
					Active = active
				};
				users.Add(user);
			}
			// closes the connection after what we need to do is done
			Connect().Close();
		}
	}
}
