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
            var addedMovie = new MovieId();
            try
            {
                addedMovie.Id = newMovieUtils.AddMovie(newMovie);
                if (addedMovie.Id > 0) //Create worked properly..
                {
                    return Ok(addedMovie);
                }

                return StatusCode(500, "Couldn't Add the Movie!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Couldn't Add the Movie!" + e);
            }
        }
    }
}