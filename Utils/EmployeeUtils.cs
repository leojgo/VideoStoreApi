using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class EmployeeUtils
    {
        public Employee LogIn(int userIdNumber, string password, ref string msg)
        {
            try
            {
                Employee toLogIn = GetEmployeeById(userIdNumber);

                if (PasswordUtils.Verify(password, toLogIn.PwHash))
                {
                    GetEmployeeTitle(toLogIn);
                    msg = "Login Successful!";
                    return toLogIn;
                }

                msg = "Login or Password were incorrect!";
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                msg = "An Exception was thrown! " + e;
                return null;
            }
        }

        public bool CreateNewUser(TempEmployee temp, ref string msg)
        {
            try
            {
                if (PasswordUtils.IsPasswordFormatValid(temp.RawPw))
                {
                    Employee newEmployee = temp;
                    newEmployee.PwHash = PasswordUtils.Hash(temp.RawPw);

                    string createUserQuery = $"INSERT INTO {DatabaseUtils.Databasename}.employeelist(EMP_Name_First, EMP_Name_Last, EMP_PW_Hash,EMP_Active, EMP_Type) " +
                                                $"VALUES('{newEmployee.FirstName}', '{newEmployee.LastName}','{newEmployee.PwHash}','1','{newEmployee.EmployeeType}');";
                    DatabaseUtils createUser = DatabaseUtils.Instance();
                    return createUser.MakeDbQuery(createUserQuery);
                }

                msg = "Password is an invalid format!!!!";
                return false;
            }
            catch (Exception e)
            {
                msg = " An Exception Occurred! " + e;
                return false;
            }
        }

        public Employee ViewEmployeeAccount(int empId)
        {
            string loginStringQuery = $"SELECT * FROM {DatabaseUtils.Databasename}.employeelist WHERE EMP_ID = {empId};";
            return SqlGetEmployee(loginStringQuery);
        }

        public bool EditEmployeeAccount(Employee updatedEmployee, ref string msg)
        {
            try
            {
                string updateEmployeeInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.employeelist " +
                                                    $"SET EMP_Name_First = '{updatedEmployee.FirstName}', " +
                                                    $"EMP_Name_Last = '{updatedEmployee.LastName}', " +
                                                    $"EMP_Active = '{Convert.ToInt32(updatedEmployee.Active)}', " +
                                                    $"EMP_Type = '{updatedEmployee.EmployeeType}' " +
                                                    $"WHERE EMP_ID = '{updatedEmployee.EmployeeId}';";

                var updateEmployee = DatabaseUtils.Instance();
                return updateEmployee.MakeDbQuery(updateEmployeeInfoQuery);
            }
            catch (Exception e)
            {
                msg = "An Exception Occurred!!" + e;
                return false;
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            string getAllemployeesQuery = $"SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.employeelist;";
            return SqlGetAllEmployees(getAllemployeesQuery);
        }

        public bool MakeEmployeeInactive(int id, ref string msg)
        {
            try
            {
                string disableEmployeeQuery = $"UPDATE {DatabaseUtils.Databasename}.employeelist " + 
                                              $"SET EMP_Active = 0 " + 
                                              $"WHERE EMP_ID = {id};";

                var updateEmployee = DatabaseUtils.Instance();
                return updateEmployee.MakeDbQuery(disableEmployeeQuery);
            }
            catch (Exception e)
            {
                msg = "An Exception was Thrown! " + e;
                return false;
            }
        }

        public Employee GetEmployeeById(int id)
        {
            string loginStringQuery = "SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.employeelist " +
                                      $"WHERE EMP_ID = {id};";

            return SqlGetEmployee(loginStringQuery);
        }


        //PRIVATES GO BELOW!!!!
        private void GetEmployeeTitle(Employee empToCheck)
        {
            try
            {
                string employerTitleQuery = "SELECT Emp_Title " +
                                            $"FROM {DatabaseUtils.Databasename}.employeetitles " +
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
            dbCon.DatabaseName = DatabaseUtils.Databasename;
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
            dbCon.DatabaseName = DatabaseUtils.Databasename;
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
            return temp;
        }

        public IEnumerable<Employee> SqlGetAllEmployees(string dbQuery)
        {
            List<Employee> allEmployees  = new List<Employee>();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee temp = new Employee();
                    temp.EmployeeId = Convert.ToInt32(reader.GetString("EMP_ID"));
                    temp.FirstName = reader.GetString("EMP_Name_First");
                    temp.LastName = reader.GetString("EMP_Name_Last");
                    temp.PwHash = reader.GetString("EMP_PW_Hash");
                    temp.Active = Convert.ToBoolean(reader.GetString("EMP_Active"));
                    temp.EmployeeType = Convert.ToInt32(reader.GetString("EMP_Type"));
                    allEmployees.Add(temp);
                }

                dbCon.Close();
            }

            return allEmployees;
        }
    }
}
