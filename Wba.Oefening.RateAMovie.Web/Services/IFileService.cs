using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Web.Services.Models;

namespace Wba.Oefening.RateAMovie.Web.Services
{
    public interface IFileService
    {
        Task<ResultModel> CreateFile(IFormFile file);
        ResultModel DeleteFile(string filename,string subfolder);
    }
}
