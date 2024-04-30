using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Core.Entities;
using Wba.Oefening.RateAMovie.Web.Areas.Admin.ViewModels;
using Wba.Oefening.RateAMovie.Web.Data;
using Wba.Oefening.RateAMovie.Web.Models;
using Wba.Oefening.RateAMovie.Web.ViewModels;

namespace Wba.Oefening.RateAMovie.Web.Controllers
{
    public class MoviesController : Controller
    {
        private MovieContext _movieContext;
        private readonly ILogger<MoviesController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(MovieContext movieContext, ILogger<MoviesController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _movieContext = movieContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            //show a list of movies
            var moviesIndexViewModel = new MoviesIndexViewModel
            { 
                Movies = await _movieContext.Movies
                .Include(m => m.Company)
                .Select(m =>
                new MoviesShowInfoViewModel 
                {
                    Id = m.Id,
                    Name = m.Title,
                    Company = new BaseViewModel 
                    {
                        Id = m.Company.Id,
                        Name = m.Company.Name,
                    },
                    Image = m.ImageFileName,
                }).ToListAsync()
            };
            return View(moviesIndexViewModel);
        }
        public async Task<IActionResult> ShowInfo(long id)
        {
            //get the movie
            var movie = await _movieContext
                .Movies
                .Include(m => m.Company)
                .Include(m => m.Actors)
                .Include(m => m.Directors)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == id);
            //check if null
            if(movie == null)
            {
                var errorViewModel =
                    new ErrorViewModel { ErrorMessage = "Movie not found!" };
                Response.StatusCode = 404;
                return View("Error",errorViewModel);
            }
            //fill the model
            var moviesShowMovieInfoViewModel = new MoviesShowInfoViewModel
            {
                Id = id,
                Name = movie.Title,
                Company = new BaseViewModel
                {
                    Id = movie.Company.Id,
                    Name = movie.Company.Name,
                },
                Image = movie.ImageFileName,
                Year = movie.ReleaseDate?.Year.ToString() ?? "Unkown",
                Actors = movie.Actors
                .Select(a => new BaseViewModel
                {
                    Id = a.Id,
                    Name = $"{a.FirstName} {a.LastName}"
                }),
                Directors = movie.Directors
                .Select(d => new BaseViewModel
                {
                    Id = d.Id,
                    Name = $"{d.FirstName} {d.LastName}"
                }),
            };
            if(movie.Ratings.Count > 0)
            {
                moviesShowMovieInfoViewModel.AverageRating
                    = movie.Ratings.Average(m => m.Score);
            }
            //pass to the view
            return View(moviesShowMovieInfoViewModel);
        }
    }
}
