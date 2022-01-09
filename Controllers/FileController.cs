using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [Authorize]
        public async Task<string> Post([FromForm]FileViewModel model)
        {
            try
            {
                if (model.File.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }

                    using (FileStream fileStream =
                           System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + model.File.FileName))
                    {
                        model.File.CopyTo(fileStream);
                        fileStream.Flush();
                        return _environment.WebRootPath + "\\Upload\\" + model.File.FileName;
                    }
                }
                else
                {
                    return "Failed to upload file";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}