using Microsoft.Data.Sqlite;

namespace auth2._0
{
    public class DB
    {
        public SqliteConnection connection = null;
        const string connectionString = "Data Source=usersdatas.db";

        public DB()
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            var cmd = new SqliteCommand();
            cmd.Connection = connection;
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'Users' ('_id' INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,'login' TEXT NOT NULL UNIQUE,'password'TEXT NOT NULL,'email' TEXT NOT NULL)";
            cmd.ExecuteNonQuery();
        }
        // return 0 if incorrect password
        // return -1 if User Not found in DB
        // return 1 if user exist with given password
        public int CheckUserInDB(string login, string password = "")
        {

            var cmd = new SqliteCommand();
            cmd.Connection = connection;
            cmd.CommandText = $"SELECT password FROM [Users] where login = '{login}'";
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                string pass = (string)reader["password"];
                if (pass != password) return 0;
                else return 1;
            }
            else
            {
                return -1; 
            }
        }
        public bool AddUserInDB(string login, string password, string email) {
            var cmd = new SqliteCommand();
            cmd.Connection = connection;
            cmd.CommandText = $"INSERT INTO Users (login, email, password) VALUES ('{login}','{email}','{password}')";
            int number = cmd.ExecuteNonQuery();
            return number > 0;
        }
    }
}
