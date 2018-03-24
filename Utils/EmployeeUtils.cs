using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class EmployeeUtils
    {
        public EmployeeInfoToShare LogIn(int userIdNumber, string password, ref string msg)
        {
            try
            {
                Employee toLogIn = GetEmployeeById_RAW(userIdNumber);

                if (PasswordUtils.Verify(password, toLogIn.PwHash))
                {
                    toLogIn.EmployeeTitle = GetEmployeeTitle(toLogIn);
                    msg = "Login Successful!";

                    return GetEmployeeById(toLogIn.EmployeeId);
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
        public static EmployeeInfoToShare RemovePersonalInfo(Employee toClean)
        {
            EmployeeInfoToShare cleanedInfo = new EmployeeInfoToShare();
            cleanedInfo.Active = toClean.Active;
            cleanedInfo.EmployeeId = toClean.EmployeeId;
            cleanedInfo.EmployeeTitle = toClean.EmployeeTitle;
            cleanedInfo.EmployeeType = toClean.EmployeeType;
            cleanedInfo.FirstName = toClean.FirstName;
            cleanedInfo.LastName = toClean.LastName;
            cleanedInfo.PhoneNumber = toClean.PhoneNumber;
            return cleanedInfo;
        }

        public bool CreateNewUser(TempEmployee temp, ref string msg)
        {
            try
            {
                if (PasswordUtils.IsPasswordFormatValid(temp.RawPw))
                {
                    Employee newEmployee = temp;
                    newEmployee.PwHash = PasswordUtils.Hash(temp.RawPw);

                    string createUserQuery = $"INSERT INTO {DatabaseUtils.Databasename}.employeelist(EMP_Name_First, EMP_Name_Last, EMP_PW_Hash,EMP_Active, EMP_Type, EMP_PhoneNumber) " +
                                                $"VALUES('{newEmployee.FirstName}', '{newEmployee.LastName}','{newEmployee.PwHash}','1','{newEmployee.EmployeeType}',{newEmployee.PhoneNumber});";
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

        public EmployeeInfoToShare ViewEmployeeAccount(int empId)

        {
            string loginStringQuery = $"SELECT * FROM {DatabaseUtils.Databasename}.employeelist WHERE EMP_ID = {empId};";
            return RemovePersonalInfo(SqlGetEmployee(loginStringQuery));
        }

        public bool EditEmployeeAccount(EmployeeInfoToShare updatedEmployee, ref string msg)
        {
            try
            {
                string updateEmployeeInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.employeelist " +
                                                    $"SET EMP_Name_First = '{updatedEmployee.FirstName}', " +
                                                    $"EMP_Name_Last = '{updatedEmployee.LastName}', " +
                                                    $"EMP_Active = '{Convert.ToInt32(updatedEmployee.Active)}', " +
                                                    $"EMP_Type = '{updatedEmployee.EmployeeType}', " +
                                                    $"EMP_PhoneNumber = '{updatedEmployee.PhoneNumber}' " + 
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

        public IEnumerable<EmployeeInfoToShare> GetAllEmployees()
        {
            string getAllemployeesQuery = $"SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.employeelist;";
            List<Employee> allEmployees = SqlGetAllEmployees(getAllemployeesQuery);
            List<EmployeeInfoToShare> cleanedEmployeeInfo = new List<EmployeeInfoToShare>();
            foreach (var employee in allEmployees)
            {
                cleanedEmployeeInfo.Add(RemovePersonalInfo(employee));
            }

            return cleanedEmployeeInfo;
        }

        public EmployeeInfoToShare GetEmployeeById(int id)
        {
            string loginStringQuery = "SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.employeelist " +
                                      $"WHERE EMP_ID = {id};";


            Employee toGet = SqlGetEmployee(loginStringQuery);
            toGet.EmployeeTitle = GetEmployeeTitle(toGet);

            return RemovePersonalInfo(toGet);
        }

        //PRIVATES GO BELOW!!!!
        private string GetEmployeeTitle(Employee empToCheck)
        {
            try
            {
                string employerTitleQuery = "SELECT Emp_Title " +
                                            $"FROM {DatabaseUtils.Databasename}.employeetitles " +
                                            $"WHERE EMP_LVL = {empToCheck.EmployeeType}";

                return SqlGetEmployeeTitle(employerTitleQuery);
            }
            catch
            {
                return null;
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

        private Employee GetEmployeeById_RAW(int id)
        {
            string loginStringQuery = "SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.employeelist " +
                                      $"WHERE EMP_ID = {id};";


            Employee toGet = SqlGetEmployee(loginStringQuery);
            toGet.EmployeeTitle = GetEmployeeTitle(toGet);

            return toGet;
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


        //SQL Queries go Below!
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
                    temp.EmployeeId = reader.GetInt32("EMP_ID");
                    temp.FirstName = reader.GetString("EMP_Name_First");
                    temp.LastName = reader.GetString("EMP_Name_Last");
                    temp.PwHash = reader.GetString("EMP_PW_Hash");
                    temp.Active = reader.GetBoolean("EMP_Active");
                    temp.EmployeeType = reader.GetInt32("EMP_Type");
                    temp.PhoneNumber = reader.GetInt64("EMP_PhoneNumber");
                }

                dbCon.Close();
            }

            if (temp.EmployeeId == 0)
                //No Match in the DB
                return null;
            return temp;
        }

        public List<Employee> SqlGetAllEmployees(string dbQuery)
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
                    temp.EmployeeId = reader.GetInt32("EMP_ID");
                    temp.FirstName = reader.GetString("EMP_Name_First");
                    temp.LastName = reader.GetString("EMP_Name_Last");
                    temp.PwHash = reader.GetString("EMP_PW_Hash");
                    temp.Active = reader.GetBoolean("EMP_Active");
                    temp.EmployeeType = reader.GetInt32("EMP_Type");
                    temp.PhoneNumber = reader.GetInt64("EMP_PhoneNumber");
                    allEmployees.Add(temp);
                }

                dbCon.Close();
            }

            return allEmployees;
        }
    }
}
