using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class EmployeeUtils
    {
        public EmployeeInfoToShare LogIn(int userIdNumber, string password)
        {
            var toLogIn = GetEmployeeById_RAW(userIdNumber);

            if (PasswordUtils.Verify(password, toLogIn.PwHash))
            {
                toLogIn.EmployeeTitle = GetEmployeeTitle(toLogIn);

                return GetEmployeeById(toLogIn.EmployeeId);
            }
            return null;
        }
        public static EmployeeInfoToShare RemovePersonalInfo(Employee toClean)
        {
            var cleanedInfo = new EmployeeInfoToShare();
            cleanedInfo.Active = toClean.Active;
            cleanedInfo.EmployeeId = toClean.EmployeeId;
            cleanedInfo.EmployeeTitle = toClean.EmployeeTitle;
            cleanedInfo.EmployeeType = toClean.EmployeeType;
            cleanedInfo.FirstName = toClean.FirstName;
            cleanedInfo.LastName = toClean.LastName;
            cleanedInfo.PhoneNumber = toClean.PhoneNumber;
            return cleanedInfo;
        }

        public int CreateNewUser(TempEmployee temp)
        {
            if (PasswordUtils.IsPasswordFormatValid(temp.RawPw))
            {
                Employee newEmployee = temp;
                newEmployee.PwHash = PasswordUtils.Hash(temp.RawPw);

                var createUserQuery = $"INSERT INTO {DatabaseUtils.Databasename}.employeelist(EMP_Name_First, EMP_Name_Last, EMP_PW_Hash,EMP_Active, EMP_Type, EMP_PhoneNumber) " +
                                            $"VALUES('{newEmployee.FirstName}', '{newEmployee.LastName}','{newEmployee.PwHash}','1','{newEmployee.EmployeeType}',{newEmployee.PhoneNumber});";
                var createUser = DatabaseUtils.Instance();
                return createUser.MakeDbQuery(createUserQuery, true);
            }
            return -1;
        }

        public EmployeeInfoToShare ViewEmployeeAccount(int empId)

        {
            var loginStringQuery = $"SELECT * FROM {DatabaseUtils.Databasename}.employeelist WHERE EMP_ID = {empId};";
            return RemovePersonalInfo(SqlGetEmployee(loginStringQuery));
        }

        public bool EditEmployeeAccount(EmployeeInfoToShare updatedEmployee)
        {
            var updateEmployeeInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.employeelist " +
                                                $"SET EMP_Name_First = '{updatedEmployee.FirstName}', " +
                                                $"EMP_Name_Last = '{updatedEmployee.LastName}', " +
                                                $"EMP_Active = '{Convert.ToInt32(updatedEmployee.Active)}', " +
                                                $"EMP_Type = '{updatedEmployee.EmployeeType}', " +
                                                $"EMP_PhoneNumber = '{updatedEmployee.PhoneNumber}' " + 
                                                $"WHERE EMP_ID = '{updatedEmployee.EmployeeId}';";

            var updateEmployee = DatabaseUtils.Instance();
            return updateEmployee.MakeDbQuery(updateEmployeeInfoQuery);
        }

        public bool EditEmployeePW(Login updatedEmployee)
        {
            if (PasswordUtils.IsPasswordFormatValid(updatedEmployee.Password))
            {
                var passwordHash = PasswordUtils.Hash(updatedEmployee.Password);

                var updateEmployeeInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.employeelist " +
                                              $"SET EMP_PW_Hash = '{passwordHash}' " +
                                              $"WHERE EMP_ID = '{updatedEmployee.Username}';";

                var updateEmployee = DatabaseUtils.Instance();
                return updateEmployee.MakeDbQuery(updateEmployeeInfoQuery);
            }

            return false;
        }


        public IEnumerable<EmployeeInfoToShare> GetAllEmployees()
        {
            var getAllemployeesQuery = $"SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.employeelist;";
            var allEmployees = SqlGetAllEmployees(getAllemployeesQuery);
            var cleanedEmployeeInfo = new List<EmployeeInfoToShare>();
            foreach (var employee in allEmployees)
            {
                employee.EmployeeTitle = GetEmployeeTitle(employee);
                cleanedEmployeeInfo.Add(RemovePersonalInfo(employee));
            }

            return cleanedEmployeeInfo;
        }

        public EmployeeInfoToShare GetEmployeeById(int id)
        {
            var loginStringQuery = "SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.employeelist " +
                                      $"WHERE EMP_ID = {id};";


            var toGet = SqlGetEmployee(loginStringQuery);
            toGet.EmployeeTitle = GetEmployeeTitle(toGet);

            return RemovePersonalInfo(toGet);
        }

        //PRIVATES GO BELOW!!!!
        private string GetEmployeeTitle(Employee empToCheck)
        {
            try
            {
                var employerTitleQuery = "SELECT Emp_Title " +
                                            $"FROM {DatabaseUtils.Databasename}.employeetitles " +
                                            $"WHERE EMP_LVL = {empToCheck.EmployeeType}";

                return SqlGetEmployeeTitle(employerTitleQuery);
            }
            catch
            {
                return null;
            }
        }

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
            var loginStringQuery = "SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.employeelist " +
                                      $"WHERE EMP_ID = {id};";


            var toGet = SqlGetEmployee(loginStringQuery);
            toGet.EmployeeTitle = GetEmployeeTitle(toGet);

            return toGet;
        }

        //SQL Queries go Below!
        public Employee SqlGetEmployee(string dbQuery)
        {
            var temp = new Employee();
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
                        temp.EmployeeId = reader.GetInt32("EMP_ID");
                        temp.FirstName = reader.GetString("EMP_Name_First");
                        temp.LastName = reader.GetString("EMP_Name_Last");
                        temp.PwHash = reader.GetString("EMP_PW_Hash");
                        temp.Active = reader.GetBoolean("EMP_Active");
                        temp.EmployeeType = reader.GetInt32("EMP_Type");
                        temp.PhoneNumber = reader.GetString("EMP_PhoneNumber");
                    }

                    dbCon.Close();
                }

                if (temp.EmployeeId == 0)
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

        public List<Employee> SqlGetAllEmployees(string dbQuery)
        {
            var allEmployees  = new List<Employee>();
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
                        var temp = new Employee();
                        temp.EmployeeId = reader.GetInt32("EMP_ID");
                        temp.FirstName = reader.GetString("EMP_Name_First");
                        temp.LastName = reader.GetString("EMP_Name_Last");
                        temp.PwHash = reader.GetString("EMP_PW_Hash");
                        temp.Active = reader.GetBoolean("EMP_Active");
                        temp.EmployeeType = reader.GetInt32("EMP_Type");
                        temp.PhoneNumber = reader.GetString("EMP_PhoneNumber");
                        allEmployees.Add(temp);
                    }

                    dbCon.Close();
                }

                return allEmployees;
            }
            catch
            {
                dbCon.Close();
                return null;
            }
        }
    }
}
