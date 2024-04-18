using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Web.Data;
using Wba.Oefening.RateAMovie.Web.ViewModels;

namespace Wba.Oefening.RateAMovie.Web.Controllers
{
    public class MoviesController : Controller
    {
        private MovieContext _movieContext;

        public MoviesController(MovieContext movieContext)
        {
            _movieContext = movieContext;
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
        public IActionResult ShowInfo(int id)
        {
            return View();
        }
    }
}
