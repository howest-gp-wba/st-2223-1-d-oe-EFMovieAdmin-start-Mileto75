using System.Collections.Generic;

namespace Wba.Oefening.RateAMovie.Web.ViewModels
{
    public class MoviesShowInfoViewModel : BaseViewModel
    {
        public int Year { get; set; }
        public IEnumerable<BaseViewModel> Actors { get; set; }
        public IEnumerable<BaseViewModel> Directors { get; set; }
        public BaseViewModel Company { get; set; }
        public double AverageRating { get; set; }
        public string Image{ get; set; }
    }
}
