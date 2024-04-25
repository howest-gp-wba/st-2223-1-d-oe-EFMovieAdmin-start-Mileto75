using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wba.Oefening.RateAMovie.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        // GET: MoviesController
        public ActionResult Index()
        {
            return View();
        }
    }
}
