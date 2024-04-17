using System;
using System.Collections.Generic;
using System.Text;

namespace Wba.Oefening.RateAMovie.Web.Models
{
    public class MovieItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
