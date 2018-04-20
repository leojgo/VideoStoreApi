using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Movies")]
    public class MovieIdController : Controller
    {
        [HttpPost("{id}")]
        public IActionResult UpdateMovieInfo([FromBody]Movie updatedMovieInfo, long id)
        {
            if (updatedMovieInfo == null)
            {
                var newMovieUtils = new MovieUtils();

                var lookedUpMovie = newMovieUtils.GetMovieById(id);
                try
                {
                    if (lookedUpMovie.Title != null)
                    {
                        return new ObjectResult(lookedUpMovie);
                    }
                    return NotFound($"There was no movie '{id}' found!");
                }
                catch (Exception e)
                {
                    return StatusCode(500, $"Couldnt find the movie you Specified ({id})! " + e);
                }
            }
            else
            {
                try
                {
                    var newMovieUtils = new MovieUtils();
                    updatedMovieInfo.MovieId = id;
                    var result = newMovieUtils.UpdateMovieInfo(updatedMovieInfo);

                    if (result)
                    {
                        return Ok();
                    }
                    return BadRequest("Couldnt Update the Movie Info! :'( ");

                }
                catch (Exception e)
                {
                    return BadRequest("Something Broke while updating the Movie Info!! " + e);
                }
            }
        }
    }
}