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
        public const string DbConnectionString = "user id=root;" + // database username  Badger_dev01
                                                "password=Badger_dev01;" + //database password
                                                "server=localhost;" + //server/computer name/IP
                                                "database=badger_database_rev1;"; //database/schema name

        public const string Databasename = "badger_database_rev1";


        private DatabaseUtils()
        {
        }

        private string _databaseName = string.Empty;
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection _connection = null;
        public MySqlConnection Connection
        {
            get { return _connection; }
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
                    if (String.IsNullOrEmpty(_databaseName))
                        return false;
                    string connstring = string.Format(DbConnectionString, _databaseName);
                    _connection = new MySqlConnection(connstring);
                    _connection.Open();
                }
                catch (Exception e)
                {
                    if (_connection.Ping())
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
            _connection.Close();
            _connection = null;
        }

        public bool MakeDbQuery(string dbQuery)
        {
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

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
                        dbQuery = null;
                        dbCon.Close();
                        return true;
                    }
                    catch (Exception e)
                    {
                        dbQuery = null;
                        dbCon.Close();
                        return false;
                    }

                //}

                dbQuery = null;
                dbCon.Close();
            }

            return false;
        }
    }
}
