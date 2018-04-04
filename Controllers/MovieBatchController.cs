using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/movieBatch")]
    public class MovieBatchController : Controller
    {
        

        [HttpPost]
        public IActionResult Create([FromBody] BatchMovieInput batchJob)  // Add New Movie
        {
            var newMovieUtils = new MovieUtils();
            try
            {
                if (batchJob.MovieList.Count > 0)
                {
                    bool result = newMovieUtils.BatchProcess(batchJob);

                    if (result)
                    {
                        return Ok();
                    }
                    return StatusCode(500, "Couldnt complete the batch Processing!");
                }
                return StatusCode(500, "You Selected 0 Copies of the movie...  Nothing to do!");

            }
            catch (Exception e)
            {
                return StatusCode(500, "Couldn't Process the batch job!" + e);
            }
        }
    }
}