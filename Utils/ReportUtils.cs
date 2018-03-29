using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class ReportUtils
    {
        //1 checked out
        //0 in stock
        //-1 on hold

        public List<MovieTitles> RunOverdueReport(int NumberofResults)
        {
            List<MovieTitles> Overdue = new List<MovieTitles>();

            string overdueReportSql = $"SELECT * " +
                                      $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                      $"WHERE MOV_STATUS > 0;";

            Overdue = SqlGetMovieList(overdueReportSql);
            List<MovieTitles> toRemove = new List<MovieTitles>();

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

            return Overdue;
        }

        public List<Movie> RunPopularReport(int NumberofResults)
        {
            List<Movie> Popular = new List<Movie>();

            return null;
        }

        public List<Customer> RunCustomerReport(int NumberofResults)
        {
            List<Customer> BestCustomers = new List<Customer>();

            return null;
        }





        private List<MovieTitles> SqlGetMovieList(string dbQuery)
        {
            var temp = new List<MovieTitles>();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mov = new MovieTitles();

                    mov.Title = reader.GetString("MOV_INFO_TITLE");
                    mov.ReturnDate = reader.GetString("MOV_RETURN_DATE");

                    temp.Add(mov);
                    mov = null;
                }

                dbCon.Close();
            }
            return temp;
        }
    }
}
