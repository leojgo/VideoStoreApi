using System;
using LackLusterVideo.Models;
using MySql.Data.MySqlClient;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class MovieUtils
    {
        public long AddMovie(NewMovie toAdd, ref string msg)
        {
            string addMovieStringQuery = $"INSERT INTO {DatabaseUtils.Databasename}.movieinfo(MOV_INFO_TITLE, MOV_INFO_RELEASE_YEAR, MOV_INFO_GENRE, MOV_INFO_UPC, MOV_STATUS) " +
                                         $"VALUES('{toAdd.Title}', '{toAdd.ReleaseYear}', '{toAdd.Genre}', '{toAdd.Upc}', 0)";

            var addMovie = DatabaseUtils.Instance();
            return addMovie.MakeDbQuery(addMovieStringQuery,true);
        }

        public Movie GetMovieById(long movieId, ref string msg)
        {
            string getMovieByIdQuery = $"SELECT * " +
                                       $"FROM {DatabaseUtils.Databasename}.movieinfo " +
                                       $"WHERE MOV_INFO_UNIQ_ID = {movieId};";

            return SqlGetMovieById(getMovieByIdQuery);
        }

        public bool UpdateMovieInfo(Movie newMovieInfo, ref string msg)
        {
            string UpdateMovieInfoQuery = $"UPDATE {DatabaseUtils.Databasename}.movieinfo " +
                                            "SET " +
                                            $"MOV_INFO_TITLE = '{newMovieInfo.Title}', " +
                                            $"MOV_INFO_RELEASE_YEAR = '{newMovieInfo.ReleaseYear}', " +
                                            $"MOV_INFO_GENRE = '{newMovieInfo.Genre}', " +
                                            $"MOV_INFO_UPC = '{newMovieInfo.Upc}', " +
                                            $"MOV_STATUS = '{newMovieInfo.Status}' " +
                                            $"WHERE MOV_INFO_UNIQ_ID = '{newMovieInfo.MovieId}';";

            var updateMovie = DatabaseUtils.Instance();
            return updateMovie.MakeDbQuery(UpdateMovieInfoQuery);
        }



        // PRIVATES
        private Movie SqlGetMovieById(string dbQuery)
        {
            Movie temp = new Movie();
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

    }
}
