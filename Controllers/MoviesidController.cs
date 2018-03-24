using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LackLusterVideo.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Movies")]
    public class MovieIdController : Controller
    {
        private string _msg;

        private readonly MovieContext _context;

        public MovieIdController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public IActionResult GetMovieById(long id)
        {
            MovieUtils newMovieUtils = new MovieUtils();

            Movie lookedUpMovie = newMovieUtils.GetMovieById(id, ref _msg);
            try
            {
                if (lookedUpMovie.Title != null)
                {
                    return new ObjectResult(lookedUpMovie);
                }

                _msg = $"There was no movie '{id}' found!";
                return NotFound(_msg);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }


        [HttpPost("{id}")]
        public IActionResult UpdateMovieInfo([FromBody]Movie UpdatedMovieInfo, long id)
        {
            try
            {
                MovieUtils newMovieUtils = new MovieUtils();
                UpdatedMovieInfo.MovieId = id;
                bool result = newMovieUtils.UpdateMovieInfo(UpdatedMovieInfo, ref _msg);

                if (result)
                {
                    return StatusCode(200);
                }

                _msg = "Couldnt Update the Movie Info! :'( ";
                return BadRequest(_msg);

            }
            catch (Exception e)
            {
                _msg = "Something Broke while updating the Movie Info!! " + e;
                return BadRequest(_msg);
            }
        }
    }
}