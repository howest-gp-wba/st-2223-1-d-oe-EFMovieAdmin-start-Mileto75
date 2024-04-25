using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Wba.Oefening.RateAMovie.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Wba.Oefening.RateAMovie.Web.Services.Models;

namespace Wba.Oefening.RateAMovie.Web.Services
{

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<ResultModel> CreateFile(IFormFile file)
        {
            ResultModel resultModel = new ResultModel();
            //handle image
            //create unique filename
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
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
                }
            }
            //copy file to full path
            var fullPathToFile = Path.Combine(pathToImageFolder, fileName);
            using (FileStream fileStream = new FileStream(fullPathToFile, FileMode.CreateNew))
            {
                try
                {
                    await file.CopyToAsync(fileStream);
                    resultModel.Issuccess = true;
                    resultModel.Filename = fileName;
                }
                catch (FileNotFoundException fileNotFoundException)
                {
                    _logger.LogCritical(fileNotFoundException.Message);
                }
            }
            return resultModel;
        }

        public ResultModel DeleteFile(string filename, string subfolder)
        {
            var resultModel = new ResultModel();
            var pathToFile = Path.Combine(_webHostEnvironment.WebRootPath,subfolder,filename);
            try 
            {
                File.Delete(pathToFile);
                resultModel.Issuccess = true;
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                _logger.LogError(fileNotFoundException.Message);
            }
            return resultModel;
        }
    }
}
