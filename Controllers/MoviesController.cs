using System;
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
            MovieUtils newMovieUtils = new MovieUtils();
            MovieId addedMovie = new MovieId();
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

        [HttpGet]
        public IActionResult Search(Movie searchInfo)
        {
            return BadRequest("Not Implemented Yet!");
        }
    }
}