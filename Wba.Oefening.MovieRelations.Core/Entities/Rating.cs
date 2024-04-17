using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wba.Oefening.RateAMovie.Core.Entities
{
    public class Rating
    {
        public long Id { get; set; }

        public byte Score { get; set; }

        public string Review { get; set; }

        //unshadowed foreign key for RatedMovie
        public long RatedMovieId { get; set; }

        public Movie RatedMovie { get; set; }

        //unshadowed foreign key for Reviewer
        public long ReviewerId { get; set; }

        public User Reviewer { get; set; }
    }
}
