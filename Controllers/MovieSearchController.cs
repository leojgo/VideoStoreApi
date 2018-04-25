using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/MovieSearch")]
    public class MovieSearchController : Controller
    {
        [HttpPost]
        public IActionResult Search([FromBody] Movie searchInfo)  // Add New Movie
        {
            var searchResults = new List<Movie>();
            var newMovieUtils = new MovieUtils();

            searchResults = newMovieUtils.SearchMovies(searchInfo);

            if (searchResults != null)
            {
                if (searchResults.Count >= 1)
                {
                    return new ObjectResult(searchResults);
                }
            }

            return NotFound("No Results were found given the search terms entered!");
        }
    }
}