using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace CommonLibs.Utils
{
    public class DataBaseUtils
    {
        private readonly MySqlConnection connection;

        public DataBaseUtils(string server, string database, string username, string password)
        {

            string connectionString = $"server={server};database={database};uid={username};pwd={password}";

            connection = new MySqlConnection(connectionString);

            Console.Write("Connection Established!");
        }

        public bool OpenConnection()
        {
            try
            {
                if (connection?.State != ConnectionState.Open)
                {
                    connection?.Open();
                }

                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.Write("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.Write("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        public void ExecuteSqlQuery(string query)
        {

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.ExecuteNonQuery();

            }
        }

        public DataTable ExecuteSelectSqlQuery(string query)
        {
            DataTable dataTable = new DataTable();

            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                dataTable.Load(dataReader);

                dataReader.Close();

            }

            return dataTable;
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    }