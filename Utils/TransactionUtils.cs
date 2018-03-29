using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class TransactionUtils
    {
        public bool MakeTransaction(TransInput forTrans)
        {
            Transaction trans = new Transaction();
            var AddMov2TransInfo = DatabaseUtils.Instance();

            string trans2MovIdQuery = $"SELECT MAX(`TRANS2MOV_TRANS_ID`) FROM transactions2movies;";
            int transID = SqlGetMappingId(trans2MovIdQuery);
            string addMov2TransInfoQuery =
                $"INSERT INTO {DatabaseUtils.Databasename}.transactions2movies (TRANS2MOV_TRANS_ID,TRANS2MOV_MOV_ID) " +
                $"VALUES ";
            foreach (var movId in forTrans.MovieList)
            {
                addMov2TransInfoQuery += $"({transID},{movId.Id})";
                if (movId.Id != forTrans.MovieList[forTrans.MovieList.Count - 1].Id)
                {
                    addMov2TransInfoQuery += ", ";
                }
                else
                {
                    addMov2TransInfoQuery += ";";
                }
            }

            if (AddMov2TransInfo.MakeDbQuery(addMov2TransInfoQuery))
            {
                trans.TransId = transID;
                trans.Date = DateTime.Now.ToString("yyyy-MM-dd");
                trans.EmpId = forTrans.EmployeeId;
                trans.Fees = 0;
                trans.FeesPaid = 0;
                trans.RemBalance = 0;

                int runningCost = 0;

                foreach (var movieCost in forTrans.MovieList)
                {
                    runningCost += movieCost.Cost;
                }

                trans.TotalPaid = runningCost;
                trans.CustId = forTrans.CustomerId;

                string newTransQuery =
                    $"INSERT INTO {DatabaseUtils.Databasename}.transactions(TRANS_ID, TRANS_Date, TRANS_Employee, TRANS_Fees, TRANS_Fees_Paid, TRANS_Total_Paid, TRANS_Rem_Balance, TRANS_Cust_ID) " + 
                    $"VALUES('{trans.TransId}', '{trans.Date}', '{trans.EmpId}', '{trans.Fees}', '{trans.FeesPaid}', '{trans.RemBalance}', '{trans.TotalPaid}', '{trans.CustId}');";

                if (AddMov2TransInfo.MakeDbQuery(newTransQuery))
                {
                    //-1 checked out
                    //0 in stock
                    // 1 on hold

                    foreach (var movId in forTrans.MovieList)
                    {
                        string updateMovieStatusQuery = $"UPDATE {DatabaseUtils.Databasename}.movieinfo " +
                                                        $"SET MOV_STATUS = 1, MOV_RETURN_DATE = \"{movId.DueDate}\" " + 
                                                        $"WHERE MOV_INFO_UNIQ_ID = {movId.Id};";

                        if (!AddMov2TransInfo.MakeDbQuery(updateMovieStatusQuery))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private int SqlGetMappingId(string dbQuery)
        {
            int maxKey = 1;
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var location = reader.GetOrdinal("MAX(`TRANS2MOV_TRANS_ID`)");
                    if (!reader.IsDBNull(location))
                    {
                        maxKey = reader.GetInt32("MAX(`TRANS2MOV_TRANS_ID`)") + 1;
                    }
                }

                dbCon.Close();
            }
            return maxKey;
        }
    }
}
