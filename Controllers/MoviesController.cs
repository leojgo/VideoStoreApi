using System;
using Microsoft.AspNetCore.Mvc;
using LackLusterVideo.Models;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Movies")]
    public class MovieController : Controller
    {
        private string _msg;

        [HttpPost]
        public IActionResult Create([FromBody] NewMovie newMovie)  // Add New Movie
        {
            MovieUtils newMovieUtils = new MovieUtils();
            MovieId addedMovie = new MovieId();
            try
            {
                addedMovie.Id = newMovieUtils.AddMovie(newMovie, ref _msg);
                if (addedMovie.Id > 0) //Create worked properly..
                {
                    return Ok(addedMovie);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return StatusCode(500);
        }

        [HttpGet]
        public IActionResult Search(Movie searchInfo)
        {
            _msg = "Not Implemented Yet!";
            return BadRequest(_msg);
        }
    }
}