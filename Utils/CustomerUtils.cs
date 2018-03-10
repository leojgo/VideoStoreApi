using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LackLusterVideo.Models;
using MySql.Data.MySqlClient;

namespace VideoStoreApi.Utils
{
    public class CustomerUtils
    {
        public bool MakeNewCustomer(Customer ToAdd)
        {
            string createUserQuery;

            if (ToAdd.Add_Line2 == null)
            {
                createUserQuery =
                    $"INSERT INTO {DatabaseUtils.databasename}.customers(CUST_Name_First, CUST_Name_Middle_In, CUST_Name_Last, CUST_Add_Line1, CUST_Add_Line2, CUST_Add_City, CUST_Add_State, CUST_Add_Zip, CUST_PhoneNumber, CUST_Email, CUST_Newsletter, CUST_AccountBalance, CUST_Active ) " +
                    $"VALUES('{ToAdd.Name_First}', '{ToAdd.Name_Middle_In}', '{ToAdd.Name_Last}', '{ToAdd.Add_Line1}', null , '{ToAdd.Add_City}', '{ToAdd.Add_State}', '{ToAdd.Add_Zip}', '{ToAdd.PhoneNumber}', '{ToAdd.Email}', {ToAdd.Newsletter}, '{ToAdd.AccountBalance}', {ToAdd.Active});";
            }
            else
            {
                createUserQuery =
                    $"INSERT INTO {DatabaseUtils.databasename}.customers(CUST_Name_First, CUST_Name_Middle_In, CUST_Name_Last, CUST_Add_Line1, CUST_Add_Line2, CUST_Add_City, CUST_Add_State, CUST_Add_Zip, CUST_PhoneNumber, CUST_Email, CUST_Newsletter, CUST_AccountBalance, CUST_Active ) " +
                    $"VALUES('{ToAdd.Name_First}', '{ToAdd.Name_Middle_In}', '{ToAdd.Name_Last}', '{ToAdd.Add_Line1}', '{ToAdd.Add_Line2}', '{ToAdd.Add_City}', '{ToAdd.Add_State}', '{ToAdd.Add_Zip}', '{ToAdd.PhoneNumber}', '{ToAdd.Email}', {ToAdd.Newsletter}, '{ToAdd.AccountBalance}', {ToAdd.Active});";
            }

            DatabaseUtils createCustomer = DatabaseUtils.Instance();
            return createCustomer.makeDBQuery(createUserQuery);
        }

        public Customer GetCustomerById(int id)
        {
            string GetCustomerQuery = $"SELECT * " +
                                      $"FROM {DatabaseUtils.databasename}.customers " +
                                      $"WHERE CUST_ID = {id};";

            return SQLGetCustomerById(GetCustomerQuery);
        }

        public bool MakeCustomerInactive(int id)
        {
             string disableCustomerQuery = $"UPDATE {DatabaseUtils.databasename}.customers " + 
                                            $"SET CUST_Active = 0 " + 
                                            $"WHERE CUST_ID = {id};";

            var updateEmployee = DatabaseUtils.Instance();
            return updateEmployee.makeDBQuery(disableCustomerQuery);
        }

        public bool UpdateCustomer(Customer UpdatedInfo)
        {
            string UpdateCustomerQuery;

            if (UpdatedInfo.Add_Line2 == null)
            {
                UpdateCustomerQuery = $"UPDATE {DatabaseUtils.databasename}.customers " +
                                      "SET " +
                                      $"CUST_Name_First = '{UpdatedInfo.Name_First}', " +
                                      $"CUST_Name_Middle_In = '{UpdatedInfo.Name_Middle_In}', " +
                                      $"CUST_Name_Last = '{UpdatedInfo.Name_Last}', " +
                                      $"CUST_Add_Line1 = '{UpdatedInfo.Add_Line1}', " +
                                      $"CUST_Add_City = '{UpdatedInfo.Add_City}', " +
                                      $"CUST_Add_State = '{UpdatedInfo.Add_State}', " +
                                      $"CUST_Add_Zip = '{UpdatedInfo.Add_Zip}', " +
                                      $"CUST_PhoneNumber = '{UpdatedInfo.PhoneNumber}', " +
                                      $"CUST_Email = '{UpdatedInfo.Email}', " +
                                      $"CUST_Newsletter = '{Convert.ToInt32(UpdatedInfo.Newsletter)}', " +
                                      $"CUST_AccountBalance = '{UpdatedInfo.AccountBalance}', " +
                                      $"CUST_Active = '{Convert.ToInt32(UpdatedInfo.Active)}' " +
                                      $"WHERE CUST_ID = '{UpdatedInfo.CustomerId}';";
            }
            else
            {
                UpdateCustomerQuery = $"UPDATE {DatabaseUtils.databasename}.customers " +
                                      "SET " +
                                      $"CUST_Name_First = '{UpdatedInfo.Name_First}', " +
                                      $"CUST_Name_Middle_In = '{UpdatedInfo.Name_Middle_In}', " +
                                      $"CUST_Name_Last = '{UpdatedInfo.Name_Last}', " +
                                      $"CUST_Add_Line1 = '{UpdatedInfo.Add_Line1}', " +
                                      $"CUST_Add_Line2 = '{UpdatedInfo.Add_Line2}', " +
                                      $"CUST_Add_City = '{UpdatedInfo.Add_City}', " +
                                      $"CUST_Add_State = '{UpdatedInfo.Add_State}', " +
                                      $"CUST_Add_Zip = '{UpdatedInfo.Add_Zip}', " +
                                      $"CUST_PhoneNumber = '{UpdatedInfo.PhoneNumber}', " +
                                      $"CUST_Email = '{UpdatedInfo.Email}', " +
                                      $"CUST_Newsletter = '{Convert.ToInt32(UpdatedInfo.Newsletter)}', " +
                                      $"CUST_AccountBalance = '{UpdatedInfo.AccountBalance}', " +
                                      $"CUST_Active = '{Convert.ToInt32(UpdatedInfo.Active)}' " +
                                      $"WHERE CUST_ID = '{UpdatedInfo.CustomerId}';";
            }

            var updateEmployee = DatabaseUtils.Instance();
            return updateEmployee.makeDBQuery(UpdateCustomerQuery);
        }


        private Customer SQLGetCustomerById(string dbQuery)
        {
            Customer temp = new Customer();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.CustomerId = reader.GetInt32($"CUST_ID");
                    temp.Name_First = reader.GetString($"CUST_Name_First");
                    temp.Name_Middle_In = reader.GetString($"CUST_Name_Middle_In");
                    temp.Name_Last = reader.GetString($"CUST_Name_Last");
                    temp.Add_Line1 = reader.GetString($"CUST_Add_Line1");

                    int location = reader.GetOrdinal($"CUST_Add_Line2");
                    if (!reader.IsDBNull(location))
                    {
                        temp.Add_Line2 = reader.GetString($"CUST_Add_Line2");
                    }

                    temp.Add_City = reader.GetString($"CUST_Add_City");
                    temp.Add_State = reader.GetString($"CUST_Add_State");
                    temp.Add_Zip = reader.GetInt32($"CUST_Add_Zip");
                    temp.PhoneNumber = reader.GetInt64($"CUST_PhoneNumber");
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
            else
                return temp;
        }
    }
}
