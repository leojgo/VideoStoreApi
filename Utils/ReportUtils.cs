﻿using System;
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

        public List<Movie2ClassResultsString> RunPopularReport(int numberofRecords)
        {

            string PopularMovieQuery = $"SELECT TRANS2MOV_MOV_ID, count(TRANS2MOV_MOV_ID) " +
                                       $"FROM {DatabaseUtils.Databasename}.transactions2movies " +
                                       $"GROUP by TRANS2MOV_MOV_ID;";

            List<Movie2ClassResults> popularMovies = SQLGetPopularList(PopularMovieQuery);

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

            return popularMovieTitles;
        }

        public List<Customer> RunCustomerReport(int NumberofResults)
        {
            List<Customer> BestCustomers = new List<Customer>();

            return null;
        }



        private List<Movie2ClassResults> SQLGetPopularList(string dbQuery)
        {
            var temp = new List<Movie2ClassResults>();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
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

        private string SqlGetMovieById(string dbQuery)
        {
            string temp = null;
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
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
}
