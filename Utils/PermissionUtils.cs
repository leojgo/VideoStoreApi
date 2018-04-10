using System;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class PermissionUtils
    {
        public Permission GetPermissionById(int id)
        {
            var getPermissionByIdQuery = "SELECT * " +
                                            $"FROM {DatabaseUtils.Databasename}.employeepermissions " +
                                            $"WHERE `EMP_Permiss` = {id};";

            return SqlGetPermissionById(getPermissionByIdQuery);

        }

        private Permission SqlGetPermissionById(string dbQuery)
        {
            var temp = new Permission();
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
                        temp.EmpPermiss = reader.GetInt32("EMP_Permiss");
                        temp.EmpCreate = reader.GetBoolean("EMP_Create");
                        temp.EmpEdit = reader.GetBoolean("EMP_Edit");
                        temp.EmpDisable = reader.GetBoolean("EMP_Disable");
                        temp.CustCreate = reader.GetBoolean("CUST_Create");
                        temp.CustEdit = reader.GetBoolean("CUST_Edit");
                        temp.CustDisable = reader.GetBoolean("CUST_Disable");
                        temp.CustSearch = reader.GetBoolean("CUST_Search");
                        temp.CustViewHist = reader.GetBoolean("CUST_ViewHist");
                        temp.CustEditHist = reader.GetBoolean("Cust_EditHist");
                        temp.InvAdd = reader.GetBoolean("INV_Add");
                        temp.InvEdit = reader.GetBoolean("INV_Edit");
                        temp.InvDisable = reader.GetBoolean("INV_Disable");
                        temp.InvRent = reader.GetBoolean("INV_Rent");
                        temp.InvReturn = reader.GetBoolean("INV_Return");
                        temp.InvHold = reader.GetBoolean("INV_Hold");
                        temp.RepOverdue = reader.GetBoolean("REP_Overdue");
                        temp.RepPopular = reader.GetBoolean("REP_Popular");
                        temp.RepCheckedOut = reader.GetBoolean("REP_CheckedOut");
                        temp.RepOnHold = reader.GetBoolean("REP_OnHold");

                    }

                    dbCon.Close();
                }

                if (temp.EmpPermiss == 0)
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
    }
}
