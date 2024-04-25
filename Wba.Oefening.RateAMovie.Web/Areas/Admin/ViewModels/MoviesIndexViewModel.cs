using System.Collections.Generic;
using Wba.Oefening.RateAMovie.Web.ViewModels;

namespace Wba.Oefening.RateAMovie.Web.Areas.Admin.ViewModels
{
    public class MoviesIndexViewModel
    {
        public IEnumerable<MoviesShowInfoViewModel> Movies { get; set; }
    }
}
