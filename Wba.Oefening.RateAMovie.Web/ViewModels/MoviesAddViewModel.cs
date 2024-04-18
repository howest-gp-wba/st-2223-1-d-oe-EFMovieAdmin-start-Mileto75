using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Wba.Oefening.RateAMovie.Web.Models;

namespace Wba.Oefening.RateAMovie.Web.ViewModels
{
    public class MoviesAddViewModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public long SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem> Directors { get; set; }
        public IEnumerable<long> SelectedDirectorIds { get; set;}
        public List<PersonCheckbox> Actors { get; set; }
    }
}
