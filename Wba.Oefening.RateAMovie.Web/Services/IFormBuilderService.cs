using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Web.Models;

namespace Wba.Oefening.RateAMovie.Web.Services
{
    public interface IFormBuilderService
    {
        Task<List<PersonCheckbox>> GetPersonCheckboxes();
        Task<IEnumerable<SelectListItem>> GetDirectors();
        Task<IEnumerable<SelectListItem>> GetCompanies();
    }
}
