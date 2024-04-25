using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Web.Data;
using Wba.Oefening.RateAMovie.Web.Models;

namespace Wba.Oefening.RateAMovie.Web.Services
{
    public class FormBuilderService : IFormBuilderService
    {
        private readonly MovieContext _movieContext;

        public FormBuilderService(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<IEnumerable<SelectListItem>> GetCompanies()
        {
            return await _movieContext
                     .Companies.Select(c => new SelectListItem
                     {
                        Value = c.Id.ToString(),
                        Text = $"{c.Name}"
                     }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetDirectors()
        {
            return await _movieContext
                      .Directors.Select(d => new SelectListItem
                      {
                        Value = d.Id.ToString(),
                        Text = $"{d.FirstName} {d.LastName}"
                      }).ToListAsync();
        }

        public async Task<List<PersonCheckbox>> GetPersonCheckboxes()
        {
            return await _movieContext
                    .Actors.Select
                    (a => new PersonCheckbox
                    {
                        Id = a.Id,
                        Name = $"{a.FirstName} {a.LastName}"
                    }).ToListAsync();
        }
    }
}
