using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class EmployeeUtils
    {
        public Employee LogIn(int userIdNumber, string password)
        {
            try
            {
                return HandleLogIn(userIdNumber, password);

            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool CreateNewUser(TempEmployee temp)
        {
            try
            {
                if (PasswordUtils.IsPasswordFormatValid(temp.RawPw))
                {
                    Employee newEmployee = temp;
                    newEmployee.PwHash = PasswordUtils.Hash(temp.RawPw);

                    string createUserQuery = "INSERT INTO {DatabaseUtils.databasename}.employeelist(EMP_Name_First, EMP_Name_Last, EMP_PW_Hash,EMP_Active, EMP_Type) " +
                                                $"VALUES('{newEmployee.FirstName}', '{newEmployee.LastName}','{newEmployee.PwHash}','1','{newEmployee.EmployeeType}');";
                    DatabaseUtils createUser = DatabaseUtils.Instance();
                    return createUser.makeDBQuery(createUserQuery);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Employee ViewEmployeeAccount(int empId)
        {
            string loginStringQuery = $"SELECT * FROM {DatabaseUtils.databasename}.employeelist WHERE EMP_ID = {empId};";
            return SqlGetEmployee(loginStringQuery);
        }

        public bool EditEmployeeAccount(TempEmployee updatedEmployee)
        {
            try
            {
                if (PasswordUtils.IsPasswordFormatValid(updatedEmployee.RawPw))
                {
                    Employee newInfo = updatedEmployee;
                    newInfo.PwHash = PasswordUtils.Hash(updatedEmployee.RawPw);

                    string updateEmployeeInfoQuery = $"UPDATE {DatabaseUtils.databasename}.employeelist " +
                                                     $"SET EMP_Name_First = '{newInfo.FirstName}', " +
                                                     $"EMP_Name_Last = '{newInfo.LastName}', " +
                                                     $"EMP_PW_Hash = '{newInfo.PwHash}', " +
                                                     $"EMP_Active = '{Convert.ToInt32(newInfo.Active)}', " +
                                                     $"EMP_Type = '{newInfo.EmployeeType}' " +
                                                     $"WHERE EMP_ID = '{newInfo.EmployeeId}';";

                    var updateEmployee = DatabaseUtils.Instance();
                    return updateEmployee.makeDBQuery(updateEmployeeInfoQuery);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        //PRIVATES GO BELOW!!!!

        //TESTED, Works
        private Employee HandleLogIn(int userIdNumber, string password)
        {
            try
            {
                string loginStringQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.databasename}.employeelist " +
                                          $"WHERE EMP_ID = {userIdNumber};";

                Employee toLogIn = SqlGetEmployee(loginStringQuery);

                if (PasswordUtils.Verify(password, toLogIn.PwHash))
                {
                    GetEmployeeTitle(toLogIn);
                    return toLogIn;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void GetEmployeeTitle(Employee empToCheck)
        {
            try
            {
                string employerTitleQuery = "SELECT Emp_Title " +
                                            $"FROM {DatabaseUtils.databasename}.employeetitles " +
                                            $"WHERE EMP_LVL = {empToCheck.EmployeeType}";

                empToCheck.EmployeeTitle = SqlGetEmployeeTitle(employerTitleQuery);
            }
            catch
            {
            }
        }

        //private Permission GetEmployeePermissions(int PermissionLvl)
        //{
        //    try
        //    {
        //        string sqlQueryString = "SELECT * " +
        //                                $"FROM {DatabaseUtils.databasename}.employeepermissions " +
        //                                $"WHERE EMP_Permiss = {PermissionLvl};";

        //        return SqlGetEmployeePermissions(sqlQueryString, PermissionLvl);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        private string SqlGetEmployeeTitle(string dbQuery)
        {
            string employeeTitle = null;
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employeeTitle = reader.GetString("EMP_TITLE");
                }

                dbCon.Close();
            }
            return employeeTitle;
        }

        //private Permission SqlGetEmployeePermissions(string dbQuery, int lookupKey)
        //{
        //    Permission temp = new Permission();

        //    var dbCon = DatabaseUtils.Instance();
        //    dbCon.DatabaseName = DatabaseUtils.databasename;
        //    if (dbCon.IsConnect())
        //    {
        //        var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

        //        var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            temp.EMP_Permiss = lookupKey;
        //            temp.EMP_Create = reader.GetBoolean("EMP_Create");
        //            temp.EMP_Edit = reader.GetBoolean("EMP_Edit");
        //            temp.EMP_Disable = reader.GetBoolean("EMP_Disable");
        //            temp.CUST_Create = reader.GetBoolean("CUST_Create");
        //            temp.CUST_Edit = reader.GetBoolean("CUST_Edit");
        //            temp.CUST_Disable = reader.GetBoolean("CUST_Disable");
        //            temp.CUST_Search = reader.GetBoolean("CUST_Search");
        //            temp.CUST_ViewHist = reader.GetBoolean("CUST_ViewHist");
        //            temp.Cust_EditHist = reader.GetBoolean("Cust_EditHist");
        //            temp.INV_Add = reader.GetBoolean("INV_Add");
        //            temp.INV_Edit = reader.GetBoolean("INV_Edit");
        //            temp.INV_Disable = reader.GetBoolean("INV_Disable");
        //            temp.INV_Rent = reader.GetBoolean("INV_Rent");
        //            temp.INV_Return = reader.GetBoolean("INV_Return");
        //            temp.INV_Hold = reader.GetBoolean("INV_Hold");
        //            temp.REP_Overdue = reader.GetBoolean("REP_Overdue");
        //            temp.REP_Popular = reader.GetBoolean("REP_Popular");
        //            temp.REP_CheckedOut = reader.GetBoolean("REP_CheckedOut");
        //            temp.REP_OnHold = reader.GetBoolean("REP_OnHold");
        //        }

        //        dbCon.Close();
        //    }

        //    return temp;
        //}

        public Employee SqlGetEmployee(string dbQuery)
        {
            Employee temp = new Employee();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.EmployeeId = Convert.ToInt32(reader.GetString("EMP_ID"));
                    temp.FirstName = reader.GetString("EMP_Name_First");
                    temp.LastName = reader.GetString("EMP_Name_Last");
                    temp.PwHash = reader.GetString("EMP_PW_Hash");
                    temp.Active = Convert.ToBoolean(reader.GetString("EMP_Active"));
                    temp.EmployeeType = Convert.ToInt32(reader.GetString("EMP_Type"));
                }

                dbCon.Close();
            }

            if (temp.EmployeeId == 0)
                //No Match in the DB
                return null;
            else
                return temp;
        }
    }
}
