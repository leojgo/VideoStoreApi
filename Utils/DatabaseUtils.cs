using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VideoStoreApi.Utils
{
    public class DatabaseUtils
    {
        private DatabaseUtils()
        {
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DatabaseUtils _instance = null;
        public static DatabaseUtils Instance()
        {
            if (_instance == null)
                _instance = new DatabaseUtils();
            return _instance;
        }

        public bool IsConnect()
        {
            
            System.Text.EncodingProvider ppp;
            ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            

            if (Connection == null)
            {
                try
                {
                    if (String.IsNullOrEmpty(databaseName))
                        return false;
                    string connstring = string.Format(PasswordUtils.DbConnectionString, databaseName);
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }
                catch (Exception e)
                {
                    if (connection.Ping())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Close()
        {
            connection.Close();
            connection = null;
        }

        public bool makeDBQuery(string DBQuery, bool readResponse)
        {
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = "forclass";
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(DBQuery, dbCon.Connection);

                //Used for Reading response only!!!  This needs to be implemented in it's own class!!!
                /*
                if (readResponse)
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string StringOne = reader.GetString(0);
                        string StringTwo = reader.GetString(1);
                    }
                }
                else
                {
                */
                    try
                    {
                        cmd.ExecuteNonQuery();
                        DBQuery = null;
                        dbCon.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        DBQuery = null;
                        dbCon.Close();
                        return false;
                    }

                //}

                DBQuery = null;
                dbCon.Close();
            }

            return false;
        }
    }
}
