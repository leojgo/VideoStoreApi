using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class CustomerUtils
    {
        private List<Customer> _searchResults = null;

        public int MakeNewCustomer(Customer toAdd)
        {
            string createUserQuery;

            if (toAdd.AddLine2 == null)
            {
                createUserQuery =
                    $"INSERT INTO {DatabaseUtils.Databasename}.customers(CUST_Name_First, CUST_Name_Middle_In, CUST_Name_Last, CUST_Add_Line1, CUST_Add_Line2, CUST_Add_City, CUST_Add_State, CUST_Add_Zip, CUST_PhoneNumber, CUST_Email, CUST_Newsletter, CUST_AccountBalance, CUST_Active ) " +
                    $"VALUES('{toAdd.NameFirst}', '{toAdd.NameMiddleIn}', '{toAdd.NameLast}', '{toAdd.AddLine1}', null , '{toAdd.AddCity}', '{toAdd.AddState}', '{toAdd.AddZip}', '{toAdd.PhoneNumber}', '{toAdd.Email}', {toAdd.Newsletter}, '{toAdd.AccountBalance}', {toAdd.Active})";
            }
            else
            {
                createUserQuery =
                    $"INSERT INTO {DatabaseUtils.Databasename}.customers(CUST_Name_First, CUST_Name_Middle_In, CUST_Name_Last, CUST_Add_Line1, CUST_Add_Line2, CUST_Add_City, CUST_Add_State, CUST_Add_Zip, CUST_PhoneNumber, CUST_Email, CUST_Newsletter, CUST_AccountBalance, CUST_Active ) " +
                    $"VALUES('{toAdd.NameFirst}', '{toAdd.NameMiddleIn}', '{toAdd.NameLast}', '{toAdd.AddLine1}', '{toAdd.AddLine2}', '{toAdd.AddCity}', '{toAdd.AddState}', '{toAdd.AddZip}', '{toAdd.PhoneNumber}', '{toAdd.Email}', {toAdd.Newsletter}, '{toAdd.AccountBalance}', {toAdd.Active})";
            }

            var makeCustomer = DatabaseUtils.Instance();
            return makeCustomer.MakeDbQuery(createUserQuery,true);
        }

        public Customer GetCustomerById(int id)
        {
            var getCustomerQuery = $"SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.customers " +
                                      $"WHERE CUST_ID = {id};";

            return SqlGetCustomerById(getCustomerQuery);
        }

        public bool MakeCustomerInactive(int id)
        {
            var disableCustomerQuery = $"UPDATE {DatabaseUtils.Databasename}.customers " +
                                            $"SET CUST_Active = 0 " +
                                            $"WHERE CUST_ID = {id}";

            var updateCustomer = DatabaseUtils.Instance();
            return updateCustomer.MakeDbQuery(disableCustomerQuery);
        }

        public bool UpdateCustomer(Customer updatedInfo)
        {
            string updateCustomerQuery;

        if (updatedInfo.AddLine2 == null)
        {
            updateCustomerQuery = $"UPDATE {DatabaseUtils.Databasename}.customers " +
                                    "SET " +
                                    $"CUST_Name_First = '{updatedInfo.NameFirst}', " +
                                    $"CUST_Name_Middle_In = '{updatedInfo.NameMiddleIn}', " +
                                    $"CUST_Name_Last = '{updatedInfo.NameLast}', " +
                                    $"CUST_Add_Line1 = '{updatedInfo.AddLine1}', " +
                                    $"CUST_Add_City = '{updatedInfo.AddCity}', " +
                                    $"CUST_Add_State = '{updatedInfo.AddState}', " +
                                    $"CUST_Add_Zip = '{updatedInfo.AddZip}', " +
                                    $"CUST_PhoneNumber = '{updatedInfo.PhoneNumber}', " +
                                    $"CUST_Email = '{updatedInfo.Email}', " +
                                    $"CUST_Newsletter = '{Convert.ToInt32(updatedInfo.Newsletter)}', " +
                                    $"CUST_AccountBalance = '{updatedInfo.AccountBalance}', " +
                                    $"CUST_Active = '{Convert.ToInt32(updatedInfo.Active)}' " +
                                    $"WHERE CUST_ID = '{updatedInfo.CustomerId}';";
        }
        else
        {
            updateCustomerQuery = $"UPDATE {DatabaseUtils.Databasename}.customers " +
                                    "SET " +
                                    $"CUST_Name_First = '{updatedInfo.NameFirst}', " +
                                    $"CUST_Name_Middle_In = '{updatedInfo.NameMiddleIn}', " +
                                    $"CUST_Name_Last = '{updatedInfo.NameLast}', " +
                                    $"CUST_Add_Line1 = '{updatedInfo.AddLine1}', " +
                                    $"CUST_Add_Line2 = '{updatedInfo.AddLine2}', " +
                                    $"CUST_Add_City = '{updatedInfo.AddCity}', " +
                                    $"CUST_Add_State = '{updatedInfo.AddState}', " +
                                    $"CUST_Add_Zip = '{updatedInfo.AddZip}', " +
                                    $"CUST_PhoneNumber = '{updatedInfo.PhoneNumber}', " +
                                    $"CUST_Email = '{updatedInfo.Email}', " +
                                    $"CUST_Newsletter = '{Convert.ToInt32(updatedInfo.Newsletter)}', " +
                                    $"CUST_AccountBalance = '{updatedInfo.AccountBalance}', " +
                                    $"CUST_Active = '{Convert.ToInt32(updatedInfo.Active)}' " +
                                    $"WHERE CUST_ID = '{updatedInfo.CustomerId}';";
            }

            var updateEmployee = DatabaseUtils.Instance();
            return updateEmployee.MakeDbQuery(updateCustomerQuery);
        }

        public List<Customer> SearchCustomers(CustomerSearchInfo searchinfo)
        {
            _searchResults = null;

            if (searchinfo.SearchTerm != null)
            {
                var genreSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.customers " +
                                          $"WHERE CUST_Name_First " +
                                          $"LIKE \"%{searchinfo.SearchTerm}%\";";

                var tempSearchResults = SqlGetCustomersBySearch(genreSearchQuery);

                CompareResults(tempSearchResults);
            }

            if (searchinfo.SearchTerm != null)
            {
                var yearSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.customers " +
                                          $"WHERE CUST_Name_Last " +
                                          $"LIKE \"%{searchinfo.SearchTerm}%\";";

                var tempSearchResults = SqlGetCustomersBySearch(yearSearchQuery);

                CompareResults(tempSearchResults);
            }

            if (searchinfo.PhoneNumber != null)
            {
                var titleSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.customers " +
                                          $"WHERE CUST_PhoneNumber " + 
                                          $"LIKE \"%{searchinfo.PhoneNumber}%\";";

                var tempSearchResults = SqlGetCustomersBySearch(titleSearchQuery);

                CompareResults(tempSearchResults);
            }
            return _searchResults;
        }


        private Customer SqlGetCustomerById(string dbQuery)
        {
            var temp = new Customer();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            try
            {
                if (dbCon.IsConnect())
                {
                    var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        temp.CustomerId = reader.GetInt32($"CUST_ID");
                        temp.NameFirst = reader.GetString($"CUST_Name_First");
                        temp.NameMiddleIn = reader.GetString($"CUST_Name_Middle_In");
                        temp.NameLast = reader.GetString($"CUST_Name_Last");
                        temp.AddLine1 = reader.GetString($"CUST_Add_Line1");

                        var location = reader.GetOrdinal($"CUST_Add_Line2");
                        if (!reader.IsDBNull(location))
                        {
                            temp.AddLine2 = reader.GetString($"CUST_Add_Line2");
                        }

                        temp.AddCity = reader.GetString($"CUST_Add_City");
                        temp.AddState = reader.GetString($"CUST_Add_State");
                        temp.AddZip = reader.GetInt32($"CUST_Add_Zip");
                        temp.PhoneNumber = reader.GetString($"CUST_PhoneNumber");
                        temp.Email = reader.GetString($"CUST_Email");
                        temp.Newsletter = reader.GetBoolean($"CUST_Newsletter");
                        temp.AccountBalance = reader.GetInt32($"CUST_AccountBalance");
                        temp.Active = reader.GetBoolean($"CUST_Active");
                    }

                    dbCon.Close();
                }

                if (temp.CustomerId == 0)
                    //No Match in the DB
                    return null;
                return temp;
            }
            catch
            {
                dbCon.Close();
                return null;
            }
        }

        private void CompareResults(List<Customer> newResults)
        {
            if (_searchResults != null)
            {
                if (newResults != null)
                {
                    foreach (var movie in newResults)
                    {
                        if (_searchResults.All(item => item.CustomerId != movie.CustomerId))
                        {
                            _searchResults.Add(movie);
                        }
                    }
                }
            }
            else
            {
                if (newResults != null)
                {
                    _searchResults = newResults;
                }
            }
        }

        private List<Customer> SqlGetCustomersBySearch(string dbQuery)
        {
            var temp = new List<Customer>();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            try
            {
                if (dbCon.IsConnect())
                {
                    var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var cust = new Customer();

                        cust.CustomerId = reader.GetInt32($"CUST_ID");
                        cust.NameFirst = reader.GetString($"CUST_Name_First");
                        cust.NameMiddleIn = reader.GetString($"CUST_Name_Middle_In");
                        cust.NameLast = reader.GetString($"CUST_Name_Last");
                        cust.AddLine1 = reader.GetString($"CUST_Add_Line1");

                        var location = reader.GetOrdinal($"CUST_Add_Line2");
                        if (!reader.IsDBNull(location))
                        {
                            cust.AddLine2 = reader.GetString($"CUST_Add_Line2");
                        }

                        cust.AddCity = reader.GetString($"CUST_Add_City");
                        cust.AddState = reader.GetString($"CUST_Add_State");
                        cust.AddZip = reader.GetInt32($"CUST_Add_Zip");
                        cust.PhoneNumber = reader.GetString($"CUST_PhoneNumber");
                        cust.Email = reader.GetString($"CUST_Email");
                        cust.Newsletter = reader.GetBoolean($"CUST_Newsletter");
                        cust.AccountBalance = reader.GetInt32($"CUST_AccountBalance");
                        cust.Active = reader.GetBoolean($"CUST_Active");

                        temp.Add(cust);
                        cust = null;
                    }

                    dbCon.Close();
                }
                return temp;
            }
            catch
            {
                dbCon.Close();
                return null;
            }
        }
    }
}
