using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Movies")]
    public class MovieController : Controller
    {
        

        [HttpPost]
        public IActionResult Create([FromBody] NewMovie newMovie)  // Add New Movie
        {
            var newMovieUtils = new MovieUtils();
            try
            {
                if (newMovie.Qty > 0)
                {
                    List<MovieId> addedMovies = newMovieUtils.AddMovie(newMovie);
                    foreach (var movId in addedMovies)
                    {
                        if (movId.Id <= 0) //Create worked properly..
                        {
                            return StatusCode(500, "Couldn't Add the Movie(s)!");
                        }
                    }
                    return Ok(addedMovies);
                    
                }
                return StatusCode(500, "You Selected 0 Copies of the movie...  I cant add 0!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Couldn't Add the Movie(s)!" + e);
            }
        }
    }
}