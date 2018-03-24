using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VideoStoreApi.Models
{
    public class Movie
    {
        public long MovieId { get; set; }
        public string Title { get; set; }
        public string ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string Upc { get; set; }
        public int Status { get; set; }
    }

    public class NewMovie
    {
        public string Title { get; set; }
        public string ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string Upc { get; set; }
    }

    public class MovieId
    {
        public long Id { get; set; }
    }
}

