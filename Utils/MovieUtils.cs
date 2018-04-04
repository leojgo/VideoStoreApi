using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class MovieUtils
    {
        private List<Movie> _searchResults = null;

        public List<MovieId> AddMovie(NewMovie toAdd)
        {
            List<MovieId> addedMovies = new List<MovieId>();
            for(int i = 0; i < toAdd.Qty; i++)
            {
                var addMovieStringQuery = $"INSERT INTO {DatabaseUtils.Databasename}.movieinfo(MOV_INFO_TITLE, MOV_INFO_RELEASE_YEAR, MOV_INFO_GENRE, MOV_INFO_UPC, MOV_STATUS) " +
                                          $"VALUES('{toAdd.Title}', '{toAdd.ReleaseYear}', '{toAdd.Genre}', '{toAdd.Upc}', 0)";

                var addMovie = DatabaseUtils.Instance();
                MovieId addedMovie = new MovieId();
                addedMovie.Id = addMovie.MakeDbQuery(addMovieStringQuery, true);
                addedMovies.Add(addedMovie);
            }

            return addedMovies;
        }

        public Movie GetMovieById(long movieId)
        {
            var getMovieByIdQuery = $"SELECT * " +
                                       $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                       $"WHERE MOV_INFO_UNIQ_ID = {movieId};";

            return SqlGetMovieById(getMovieByIdQuery);
        }

        public bool UpdateMovieInfo(Movie newMovieInfo)
        {
            string updateMovieInfoQuery;

            if (newMovieInfo.Status == 0)
            {
                updateMovieInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.movieinfo " +
                                           "SET " +
                                           $"MOV_INFO_TITLE = '{newMovieInfo.Title}', " +
                                           $"MOV_INFO_RELEASE_YEAR = '{newMovieInfo.ReleaseYear}', " +
                                           $"MOV_INFO_GENRE = '{newMovieInfo.Genre}', " +
                                           $"MOV_INFO_UPC = '{newMovieInfo.Upc}', " +
                                           $"MOV_STATUS = '{newMovieInfo.Status}', " +
                                           $"MOV_RETURN_DATE = null " +
                                           $"WHERE MOV_INFO_UNIQ_ID = '{newMovieInfo.MovieId}';";
            }
            else
            {
                updateMovieInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.movieinfo " +
                                       "SET " +
                                       $"MOV_INFO_TITLE = '{newMovieInfo.Title}', " +
                                       $"MOV_INFO_RELEASE_YEAR = '{newMovieInfo.ReleaseYear}', " +
                                       $"MOV_INFO_GENRE = '{newMovieInfo.Genre}', " +
                                       $"MOV_INFO_UPC = '{newMovieInfo.Upc}', " +
                                       $"MOV_STATUS = '{newMovieInfo.Status}' " +
                                       $"WHERE MOV_INFO_UNIQ_ID = '{newMovieInfo.MovieId}';";
            }


            var updateMovie = DatabaseUtils.Instance();

            return updateMovie.MakeDbQuery(updateMovieInfoQuery);
        }

        public List<Movie> SearchMovies(Movie searchinfo)
        {
            _searchResults = null;

            if (searchinfo.Genre != null)
            {
                var genreSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                          $"WHERE MOV_INFO_GENRE " +
                                          $"LIKE \"%{searchinfo.Genre}%\";";

                var tempSearchResults = SqlGetMoviesBySearch(genreSearchQuery);

                CompareResults(tempSearchResults);
            }

            if (searchinfo.ReleaseYear != null)
            {
                var yearSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                          $"WHERE MOV_INFO_RELEASE_YEAR " +
                                          $"LIKE \"%{searchinfo.ReleaseYear}%\";";

                var tempSearchResults = SqlGetMoviesBySearch(yearSearchQuery);

                CompareResults(tempSearchResults);
            }

            if (searchinfo.Title != null)
            {
                var titleSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                          $"WHERE MOV_INFO_TITLE " + 
                                          $"LIKE \"%{searchinfo.Title}%\";";

                var tempSearchResults = SqlGetMoviesBySearch(titleSearchQuery);

                CompareResults(tempSearchResults);
            }

            if (searchinfo.Upc != null)
            {
                var upcSearchQuery = "SELECT * " +
                                          $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                          $"WHERE MOV_INFO_UPC " +
                                          $"LIKE \"%{searchinfo.Upc}%\";";

                var tempSearchResults = SqlGetMoviesBySearch(upcSearchQuery);

                CompareResults(tempSearchResults);
            }

            List<Movie> SortedList = _searchResults.OrderBy(o=>o.Upc).ToList();

            return SortedList;
        }

        public bool BatchProcess(BatchMovieInput batchJob)
        {
            Movie newMovieInfo = new Movie();

            foreach (var curMovie in batchJob.MovieList)
            {
                newMovieInfo.MovieId = curMovie.Id;
                newMovieInfo.Genre = batchJob.Genre;
                newMovieInfo.ReleaseYear = batchJob.ReleaseYear;
                newMovieInfo.Status = curMovie.Status;
                newMovieInfo.Title = batchJob.Title;
                newMovieInfo.Upc = batchJob.Upc;

                bool result = UpdateMovieInfo(newMovieInfo);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }
        



        // PRIVATES
        private void CompareResults(List<Movie> newResults)
        {
            if (_searchResults != null)
            {
                if (newResults != null)
                {
                    foreach (var movie in newResults)
                    {
                        if (_searchResults.All(item => item.MovieId != movie.MovieId))
                        {
                            _searchResults.Add(movie);
                        }
                    }
                }
            }
            else
            {
                if (newResults != null)
                {
                    _searchResults = newResults;
                }
            }
        }
        private Movie SqlGetMovieById(string dbQuery)
        {
            var temp = new Movie();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.MovieId = reader.GetInt64("MOV_INFO_UNIQ_ID");
                    temp.Title = reader.GetString("MOV_INFO_TITLE");
                    temp.ReleaseYear = reader.GetString("MOV_INFO_RELEASE_YEAR");
                    temp.Genre = reader.GetString("MOV_INFO_GENRE");
                    temp.Upc = reader.GetString("MOV_INFO_UPC");
                    temp.Status = reader.GetInt32("MOV_STATUS");
                }

                dbCon.Close();
            }

            if (temp.MovieId == 0)
                //No Match in the DB
                return null;
            return temp;
        }
        private List<Movie> SqlGetMoviesBySearch(string dbQuery)
        {
            var temp = new List<Movie>();
            var dbCon = DatabaseUtils.Instance();
            dbCon.DatabaseName = DatabaseUtils.Databasename;
            if (dbCon.IsConnect())
            {
                var cmd = new MySqlCommand(dbQuery, dbCon.Connection);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mov = new Movie();

                    mov.MovieId = reader.GetInt64("MOV_INFO_UNIQ_ID");
                    mov.Title = reader.GetString("MOV_INFO_TITLE");
                    mov.ReleaseYear = reader.GetString("MOV_INFO_RELEASE_YEAR");
                    mov.Genre = reader.GetString("MOV_INFO_GENRE");
                    mov.Upc = reader.GetString("MOV_INFO_UPC");
                    mov.Status = reader.GetInt32("MOV_STATUS");

                    temp.Add(mov);
                    mov = null;
                }

                dbCon.Close();
            }
            return temp;
        }

    }
}
