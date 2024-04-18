using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wba.Oefening.RateAMovie.Web.Models;

namespace Wba.Oefening.RateAMovie.Web.ViewModels
{
    public class MoviesAddViewModel
    {
        [Required]
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public long SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem> Directors { get; set; }
        public IEnumerable<long> SelectedDirectorIds { get; set;}
        public List<PersonCheckbox> Actors { get; set; }
    }
}
