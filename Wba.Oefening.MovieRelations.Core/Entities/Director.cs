using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wba.Oefening.RateAMovie.Core.Entities
{
    public class Director
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
