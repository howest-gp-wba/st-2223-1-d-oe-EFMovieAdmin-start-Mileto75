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
using Wba.Oefening.RateAMovie.Web.Data;
using Wba.Oefening.RateAMovie.Web.Models;
using Wba.Oefening.RateAMovie.Web.Services;
using Wba.Oefening.RateAMovie.Web.ViewModels;

namespace Wba.Oefening.RateAMovie.Web.Controllers
{
    public class MoviesController : Controller
    {
        private MovieContext _movieContext;
        private readonly ILogger<MoviesController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IFileService _fileService;

        public MoviesController(MovieContext movieContext, ILogger<MoviesController> logger, IWebHostEnvironment webHostEnvironment, IFormBuilderService formBuilderService, IFileService fileService)
        {
            _movieContext = movieContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _formBuilderService = formBuilderService;
            _fileService = fileService;
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
            if (movie == null)
            {
                var errorViewModel =
                    new ErrorViewModel { ErrorMessage = "Movie not found!" };
                Response.StatusCode = 404;
                return View("Error", errorViewModel);
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
            if (movie.Ratings.Count > 0)
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
                Actors = await _formBuilderService.GetPersonCheckboxes(),
                Directors = await _formBuilderService.GetDirectors(),
                Companies = await _formBuilderService.GetCompanies(),
                ReleaseDate = DateTime.Now
            };
            return View(moviesAddViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MoviesAddViewModel moviesAddViewModel)
        {
            //custom validation
            if (moviesAddViewModel.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Date must be in the past!");
            }
            //model validation
            if (!ModelState.IsValid)
            {
                //return model to the view
                moviesAddViewModel.Actors = await _formBuilderService.GetPersonCheckboxes();
                moviesAddViewModel.Directors = await _formBuilderService.GetDirectors();
                moviesAddViewModel.Companies = await _formBuilderService.GetCompanies();
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
            if (moviesAddViewModel.Image != null)
            {
                var result = await _fileService.CreateFile(moviesAddViewModel.Image);
                if (!result.Issuccess)
                {
                    ModelState.AddModelError("", "Something went wrong!");
                }
                else
                {
                    //add filename to movie entity
                    newMovie.ImageFileName = result.Filename;
                }
            }
            if (!ModelState.IsValid)
            {
                moviesAddViewModel.Actors = await _formBuilderService.GetPersonCheckboxes();
                moviesAddViewModel.Directors = await _formBuilderService.GetDirectors();
                moviesAddViewModel.Companies = await _formBuilderService.GetCompanies();
                return View(moviesAddViewModel);
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
        [HttpGet]
        public async Task<IActionResult> Update(long id)
        {
            var movie = await _movieContext
                .Movies
                .Include(m => m.Company)
                .Include(m => m.Actors)
                .Include(m => m.Directors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(movie == null)
            {
                Response.StatusCode = 404;
                return View("Error",new ErrorViewModel { ErrorMessage = "Movie not found!" });
            }
            var moviesUpdateViewModel = new MoviesUpdateViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Actors = await _formBuilderService.GetPersonCheckboxes(),
                Directors = await _formBuilderService.GetDirectors(),
                Companies = await _formBuilderService.GetCompanies(),
                ImagePath = movie.ImageFileName,
                ReleaseDate = movie.ReleaseDate,
                SelectedCompanyId = movie.CompanyId,
                SelectedDirectorIds = movie.Directors
                .Select(d => d.Id)
            };
            //select the checkboxes
            foreach(var actor in movie.Actors)
            {
                moviesUpdateViewModel.Actors.First(a => a.Id == actor.Id)
                    .Selected = true;
            }
            return View(moviesUpdateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MoviesUpdateViewModel moviesUpdateViewModel)
        {
            //custom validation
            if (moviesUpdateViewModel.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Date must be in the past!");
            }
            //model validation
            if (!ModelState.IsValid)
            {
                //return model to the view
                moviesUpdateViewModel.Actors = await _formBuilderService.GetPersonCheckboxes();
                moviesUpdateViewModel.Directors = await _formBuilderService.GetDirectors();
                moviesUpdateViewModel.Companies = await _formBuilderService.GetCompanies();
                return View(moviesUpdateViewModel);
            }
            //get the movie
            var movie = await _movieContext.Movies
                .Include(m => m.Company)
                .Include(m => m.Actors)
                .Include(m => m.Directors)
                .FirstOrDefaultAsync
                (m => m.Id == moviesUpdateViewModel.Id);
            if(movie == null)
            {
                Response.StatusCode = 404;
                return View("Error", new ErrorViewModel { ErrorMessage = "Movie not found!"});
            }
            var selectedActorIds = moviesUpdateViewModel
                .Actors.Where(a => a.Selected == true).Select(a => a.Id);
            movie.Title = moviesUpdateViewModel.Title;
            movie.ReleaseDate = moviesUpdateViewModel.ReleaseDate;
            movie.CompanyId = moviesUpdateViewModel.SelectedCompanyId;
            movie.Directors.Clear();
            movie.Directors = await _movieContext
                .Directors.Where(d => moviesUpdateViewModel
                .SelectedDirectorIds.Contains(d.Id)).ToListAsync();
            movie.Actors.Clear();
            movie.Actors = await _movieContext
                    .Actors.Where(d => selectedActorIds.Contains(d.Id)).ToListAsync();
            
            //check for image
            if (moviesUpdateViewModel.Image != null)
            {
                //handle image
                //build path to file
                var fileName = movie.ImageFileName;
                //build path to store file
                var pathToImageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "movies");
                //check if path exists
                if (!Directory.Exists(pathToImageFolder))
                {
                    //create directory
                    try
                    {
                        Directory.CreateDirectory(pathToImageFolder);
                    }
                    catch (DirectoryNotFoundException directoryNotFoundException)
                    {
                        _logger.LogCritical(directoryNotFoundException.Message);
                        ModelState.AddModelError("", "Something went wrong, please try again later");
                    }
                }
                //copy file to full path
                var fullPathToFile = Path.Combine(pathToImageFolder, fileName);
                using (FileStream fileStream = new FileStream(fullPathToFile, FileMode.Create))
                {
                    try
                    {
                        await moviesUpdateViewModel.Image.CopyToAsync(fileStream);
                    }
                    catch (FileNotFoundException fileNotFoundException)
                    {
                        _logger.LogCritical(fileNotFoundException.Message);
                        ModelState.AddModelError("", "Something went wrong, please try again later");
                    }
                }
                if (!ModelState.IsValid)
                {
                    moviesUpdateViewModel.Actors = await _movieContext
                            .Actors.Select
                            (a => new PersonCheckbox
                            {
                                Id = a.Id,
                                Name = $"{a.FirstName} {a.LastName}"
                            }).ToListAsync();
                    moviesUpdateViewModel.Directors = await _movieContext
                                .Directors.Select(d => new SelectListItem
                                {
                                    Value = d.Id.ToString(),
                                    Text = $"{d.FirstName} {d.LastName}"
                                }).ToListAsync();
                    moviesUpdateViewModel.Companies = await _movieContext
                                .Companies.Select(c => new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = $"{c.Name}"
                                }).ToListAsync();
                    return View(moviesUpdateViewModel);
                }
                //add filename to movie entity
                movie.ImageFileName = fileName;
            }
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
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var movie = await _movieContext
                .Movies.FirstOrDefaultAsync(m => m.Id == id);
            if(movie == null)
            {
                Response.StatusCode = 404;
                return View("Error", new ErrorViewModel { ErrorMessage = "Movie not found!" });
            }
            MoviesDeleteViewModel moviesDeleteViewModel
                = new MoviesDeleteViewModel
                {
                    Id = movie.Id,
                    Name = movie.Title
                };
            return View(moviesDeleteViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(MoviesDeleteViewModel moviesDeleteViewModel)
        {
            //get the movie
            var movie = await _movieContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == moviesDeleteViewModel.Id);
            //check if null
            if(movie == null)
            {
                Response.StatusCode = 404;
                return View("Error", new ErrorViewModel { ErrorMessage = "Movie not found!" });
            }
            //delete the movie
            //remove from context
            //delete the image
            var result = _fileService.DeleteFile(movie.ImageFileName,"movies");
            _movieContext.Movies.Remove(movie);
            //save the changes
            try 
            {
                await _movieContext.SaveChangesAsync();
            }
            catch(DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException.Message);
            }
            //redirect to index
            return RedirectToAction("Index");
        }
    }
}
