﻿using Microsoft.AspNetCore.Mvc;

namespace Wba.Oefening.RateAMovie.Web.ViewModels
{
    public class MoviesUpdateViewModel : MoviesAddViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string ImagePath { get; set; }
    }
}
