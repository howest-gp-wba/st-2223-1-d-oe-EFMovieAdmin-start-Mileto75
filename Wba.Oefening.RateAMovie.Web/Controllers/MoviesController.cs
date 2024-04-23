using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Core.Entities;
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
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MoviesAddViewModel moviesAddViewModel = new MoviesAddViewModel
            {
                Actors = await _movieContext
                            .Actors.Select
                            (a => new PersonCheckbox
                            {
                                Id = a.Id,
                                Name = $"{a.FirstName} {a.LastName}"
                            }).ToListAsync(),
                Directors = await _movieContext
                            .Directors.Select(d => new SelectListItem
                            {
                                Value = d.Id.ToString(),
                                Text = $"{d.FirstName} {d.LastName}"
                            }).ToListAsync(),
                Companies = await _movieContext
                            .Companies.Select(c => new SelectListItem
                            {
                                Value = c.Id.ToString(),
                                Text = $"{c.Name}"
                            }).ToListAsync(),
                ReleaseDate = DateTime.Now
            };
            return View(moviesAddViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MoviesAddViewModel moviesAddViewModel)
        {
            //custom validation
            if(moviesAddViewModel.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Date must be in the past!");
            }
            //model validation
            if(!ModelState.IsValid)
            {
                //return model to the view
                moviesAddViewModel.Actors = await _movieContext
                            .Actors.Select
                            (a => new PersonCheckbox
                            {
                                Id = a.Id,
                                Name = $"{a.FirstName} {a.LastName}"
                            }).ToListAsync();
                moviesAddViewModel.Directors = await _movieContext
                            .Directors.Select(d => new SelectListItem
                            {
                                Value = d.Id.ToString(),
                                Text = $"{d.FirstName} {d.LastName}"
                            }).ToListAsync();
                moviesAddViewModel.Companies = await _movieContext
                            .Companies.Select(c => new SelectListItem
                            {
                                Value = c.Id.ToString(),
                                Text = $"{c.Name}"
                            }).ToListAsync();
                return View(moviesAddViewModel);
            }
            //create movie
            var selectedActorIds = moviesAddViewModel
                .Actors.Where(a => a.Selected == true).Select(a => a.Id);
            var newMovie = new Movie 
            {
                Title = moviesAddViewModel.Title,
                ReleaseDate = moviesAddViewModel.ReleaseDate,
                CompanyId = moviesAddViewModel.SelectedCompanyId,
                Directors = await _movieContext
                            .Directors.Where(d => moviesAddViewModel
                            .SelectedDirectorIds.Contains(d.Id)).ToListAsync(),
                Actors = await _movieContext
                            .Actors.Where(d => selectedActorIds.Contains(d.Id)).ToListAsync(),
            };
            //check for image
            if(moviesAddViewModel.Image != null)
            {
                //handle image
                //create unique filename
                var fileName = $"{Guid.NewGuid()}_{moviesAddViewModel.Image.FileName}";
                //build path to store file
                var pathToImageFolder = Path.Combine(_webHostEnvironment.WebRootPath,"images","movies");
                //check if path exists
                if(!Directory.Exists(pathToImageFolder))
                {
                    //create directory
                    try
                    {
                        Directory.CreateDirectory(pathToImageFolder);
                    }
                    catch(DirectoryNotFoundException directoryNotFoundException)
                    {
                        _logger.LogCritical(directoryNotFoundException.Message);
                        ModelState.AddModelError("", "Something went wrong, please try again later");
                    }
                }
                //copy file to full path
                var fullPathToFile = Path.Combine(pathToImageFolder, fileName);
                using(FileStream fileStream = new FileStream(fullPathToFile,FileMode.CreateNew))
                {
                    try
                    {
                        await moviesAddViewModel.Image.CopyToAsync(fileStream);
                    }
                    catch (FileNotFoundException fileNotFoundException)
                    {
                        _logger.LogCritical(fileNotFoundException.Message);
                        ModelState.AddModelError("", "Something went wrong, please try again later");
                    }
                }
                if(!ModelState.IsValid)
                {
                    moviesAddViewModel.Actors = await _movieContext
                            .Actors.Select
                            (a => new PersonCheckbox
                            {
                                Id = a.Id,
                                Name = $"{a.FirstName} {a.LastName}"
                            }).ToListAsync();
                    moviesAddViewModel.Directors = await _movieContext
                                .Directors.Select(d => new SelectListItem
                                {
                                    Value = d.Id.ToString(),
                                    Text = $"{d.FirstName} {d.LastName}"
                                }).ToListAsync();
                    moviesAddViewModel.Companies = await _movieContext
                                .Companies.Select(c => new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = $"{c.Name}"
                                }).ToListAsync();
                    return View(moviesAddViewModel);
                }
                //add filename to movie entity
                newMovie.ImageFileName = fileName;
            }
            //add to context
            _movieContext.Movies.Add(newMovie);
            //save the changes
            try
            {
                await _movieContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
                return View("Error", new ErrorViewModel { ErrorMessage = "Movie not created!" });
            }
            return RedirectToAction("Index");
        }
    }
}
