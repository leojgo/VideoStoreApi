using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Return")]
    public class ReturnController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody] MovieList ReturnList)
        {
            var newMovieUtil = new MovieUtils();

            try
            {
                if (newMovieUtil.ReturnMovies(ReturnList))
                {
                    return Ok();
                }
                return BadRequest("Error, Couldnt return the movies!!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Error, Couldnt return the movies!! " + e);
            }
        }
    }

    public class MovieList
    {
        public List<MovieId> movieList { get; set; }
    }
}