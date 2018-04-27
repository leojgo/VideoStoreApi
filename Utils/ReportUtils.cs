using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class ReportUtils
    {
        public List<MovieTitles> RunOverdueReport(int NumberofResults)
        {
            List<MovieTitles> Overdue = new List<MovieTitles>();

            string overdueReportSql = $"SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                      $"WHERE MOV_STATUS = 1;";

            Overdue = SqlGetMovieList(overdueReportSql);
            List<MovieTitles> toRemove = new List<MovieTitles>();

            if (Overdue != null)
            {
                foreach (var movie in Overdue)
                {
                    if (DateTime.Now.Date < Convert.ToDateTime(movie.ReturnDate).Date)
                    {
                        toRemove.Add(movie);
                    }
                }

                foreach (var movie in toRemove)
                {
                    Overdue.Remove(movie);
                }


                foreach (var movie in Overdue)
                {
                    long CustId = 0;
                    long TransId = 0;

                    string GetTransIdQuery = $"SELECT MAX(TRANS2MOV_TRANS_ID) " +
                                            $"FROM {DatabaseUtils.Databasename}.transactions2movies " +
                                            $"WHERE TRANS2MOV_MOV_ID = {movie.MovieId};";

                    TransId = SqlGetTransInfo(GetTransIdQuery);

                    string GetCustomerIdQuery = $"SELECT TRANS_Cust_ID " +
                                                $"FROM {DatabaseUtils.Databasename}.transactions " +
                                                $"WHERE TRANS_ID = {TransId};";

                    CustId = SqlGetCustomerInfo(GetCustomerIdQuery);

                    var getCustomerQuery = $"SELECT * " +
                                           $"FROM {DatabaseUtils.Databasename}.customers " +
                                           $"WHERE CUST_ID = {CustId};";

                    Customer CustInfo = new Customer();
                    CustInfo = SqlGetCustomerById(getCustomerQuery);

                    movie.CustFirstName = CustInfo.NameFirst;
                    movie.CustLastName = CustInfo.NameLast;
                    movie.CustPhoneNumber = CustInfo.PhoneNumber;
                }

                List<MovieTitles> SortedList = Overdue.OrderBy(o => o.ReturnDate).ToList();

                return SortedList;
            }
            return null;
        }

        public List<Movie2ClassResultsString> RunPopularReport(int numberofRecords)
        {

            string PopularMovieQuery = $"SELECT TRANS2MOV_MOV_ID, count(TRANS2MOV_MOV_ID) " +
                                       $"FROM {DatabaseUtils.Databasename}.transactions2movies " +
                                       $"GROUP by TRANS2MOV_MOV_ID;";

            List<Movie2ClassResults> popularMovies = SQLGetPopularList(PopularMovieQuery);
            if (popularMovies != null)
            {

                if (numberofRecords > 0)
                {
                    var count = popularMovies.Count - numberofRecords;
                    if (count > 0)
                    {
                        popularMovies.RemoveRange(popularMovies.Count - count, count);
                    }
                }

                List<Movie2ClassResultsString> popularMovieTitles = new List<Movie2ClassResultsString>();

                foreach (var movieID in popularMovies)
                {
                    Movie2ClassResultsString entry = new Movie2ClassResultsString();
                    string getTitleQuery = $"SELECT * " +
                                           $"FROM {DatabaseUtils.Databasename}.MovieInfo " +
                                           $"WHERE MOV_INFO_UNIQ_ID = {movieID.MovieId};";

                    entry.Title = SqlGetMovieById(getTitleQuery);
                    entry.count = movieID.count;
                    popularMovieTitles.Add(entry);
                    entry = null;
                }

                var SortedList = popularMovieTitles.OrderByDescending(o => o.count).ToList();

                return SortedList;
            }
            else
            {
                return null;
            }
        }

        public List<CustomerInfo> RunCustomerReport(int NumberofResults)
        {
            List<CustomerInfo> BestCustomers = new List<CustomerInfo>();

            string GetBestInfoQuery = $"SELECT TRANS_Cust_ID, COUNT(*) as count " +
                                      $"FROM {DatabaseUtils.Databasename}.transactions " +
                                      $"GROUP BY TRANS_Cust_ID ORDER BY count DESC;";

            List<PopularCustomerInfo> CustomerInfo = SqlGetCustIDs(GetBestInfoQuery, NumberofResults);
            if (CustomerInfo != null)
            {

                foreach (var cust in CustomerInfo)
                {
                    var getCustomerQuery = $"SELECT * " +
                                           $"FROM {DatabaseUtils.Databasename}.customers " +
                                           $"WHERE CUST_ID = {cust.customerId};";

                    Customer CustInfo = new Customer();
                    CustInfo = SqlGetCustomerById(getCustomerQuery);
                    if (CustInfo != null)
                    {
                        if (CustInfo.Active)
                        {
                            CustomerInfo bestCust = new CustomerInfo();
                            bestCust.CustomerId = CustInfo.CustomerId;
                            bestCust.NameFirst = CustInfo.NameFirst;
                            bestCust.NameLast = CustInfo.NameLast;
                            bestCust.PhoneNumber = CustInfo.PhoneNumber;
                            bestCust.TransactionCount = cust.transactionCount;

                            BestCustomers.Add(bestCust);
                        }
                    }
                }

                return BestCustomers;
            }
            else
            {
                return null;
            }
        }

        private List<Movie2ClassResults> SQLGetPopularList(string dbQuery)
        {
            var temp = new List<Movie2ClassResults>();
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
                        Movie2ClassResults results = new Movie2ClassResults();
                        results.MovieId = reader.GetInt32("TRANS2MOV_MOV_ID");
                        results.count = reader.GetInt32("count(TRANS2MOV_MOV_ID)");

                        temp.Add(results);
                        results = null;
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

        private List<MovieTitles> SqlGetMovieList(string dbQuery)
        {
            var temp = new List<MovieTitles>();
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
                        var mov = new MovieTitles();

                        mov.MovieId = reader.GetInt64("MOV_INFO_UNIQ_ID");
                        mov.Title = reader.GetString("MOV_INFO_TITLE");
                        mov.ReturnDate = reader.GetString("MOV_STATUS_DATE");

                        temp.Add(mov);
                        mov = null;
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

        private string SqlGetMovieById(string dbQuery)
        {
            string temp = null;
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
                        temp = reader.GetString("MOV_INFO_TITLE");
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

        private long SqlGetTransInfo(string dbQuery)
        {
            long temp = 0;
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
                        temp = reader.GetInt64("MAX(TRANS2MOV_TRANS_ID)");
                    }

                    dbCon.Close();
                }
                return temp;
            }
            catch
            {
                dbCon.Close();
                return -1;
            }
        }

        private long SqlGetCustomerInfo(string dbQuery)
        {
            long temp = 0;
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
                        temp = reader.GetInt64("TRANS_Cust_ID");
                    }

                    dbCon.Close();
                }
                return temp;
            }
            catch
            {
                dbCon.Close();
                return -1;
            }
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
                        temp.NameLast = reader.GetString($"CUST_Name_Last");
                        temp.PhoneNumber = reader.GetString($"CUST_PhoneNumber");
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

        private List<PopularCustomerInfo> SqlGetCustIDs(string dbQuery, int numberOfResults)
        {
            var temp = new List<PopularCustomerInfo>();
            int numOfEntriesRead = 0;
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
                        var curCust = new PopularCustomerInfo();

                        curCust.customerId = reader.GetInt64($"TRANS_CUST_ID");
                        curCust.transactionCount = reader.GetInt32($"count");

                        temp.Add(curCust);
                        numOfEntriesRead++;
                        if (numOfEntriesRead >= numberOfResults && numberOfResults != 0)
                        {
                            break;
                        }
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

    public class Movie2ClassResults
    {
        public int MovieId { get; set; }
        public int count { get; set; }
    }
    public class Movie2ClassResultsString
    {
        public string Title { get; set; }
        public int count { get; set; }
    }

    public class PopularCustomerInfo
    {
        public long customerId { get; set; }
        public int transactionCount { get; set; }
    }
}
