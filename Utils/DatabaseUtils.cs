using System;
using System.Text;
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
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }

        private static DatabaseUtils _instance;
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
                    var connstring = string.Format(DbConnectionString, _databaseName);
                    _connection = new MySqlConnection(connstring);
                    _connection.Open();
                }
                catch (Exception)
                {
                    if (_connection.Ping())
                    {
                        return true;
                    }

                    return false;
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
            try
            {
                if (dbCon.IsConnect())
                {
                    var cmd = new MySqlCommand(dbQuery, dbCon.Connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        dbQuery = null;
                        dbCon.Close();
                        return true;
                    }
                    catch (Exception)
                    {
                        dbQuery = null;
                        dbCon.Close();
                        return false;
                    }
                }

                return false;
            }
            catch
            {
                dbCon.Close();
                return false;
            }
        }


        public int MakeDbQuery(string dbQuery, bool getLastKey)
        {
            var key = -1;
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            try
            {
                if (dbCon.IsConnect())
                {
                    var cmd = new MySqlCommand(dbQuery, dbCon.Connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        dbQuery = null;
                        if (getLastKey)
                        {
                            key = SQL_GetLastInsertedId();
                        }
                        dbCon.Close();
                        return key;
                    }
                    catch (Exception)
                    {
                        dbQuery = null;
                        dbCon.Close();
                        return key;
                    }
                }
                return key;
            }
            catch
            {
                dbCon.Close();
                return -1;
            }
        }


        private int SQL_GetLastInsertedId()
        {
            var dbQuery = "SELECT LAST_INSERT_ID();";
            var lastInsertedKey = -1;
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            try
            {
                if (dbCon.IsConnect())
                {
                    var cmd = new MySqlCommand(dbQuery, dbCon.Connection);
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            lastInsertedKey = reader.GetInt32(0);
                        }
                        return lastInsertedKey;
                    }
                    catch (Exception)
                    {
                        return lastInsertedKey;
                    }
                }

                return lastInsertedKey;
            }
            catch
            {
                dbCon.Close();
                return -1;
            }
        }

    }
}
