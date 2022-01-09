using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Services;
using TestTask.Services.Interfaces;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private static IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public FileController(IWebHostEnvironment environment, UserManager<User> userManager, IFileService fileService)
        {
            _environment = environment;
            _userManager = userManager;
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromForm]FileViewModel model)
        {
            try
            {
                if (model.File.Length > 0)
                { 
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\");
                    }
                    
                    using (FileStream fileStream =
                           System.IO.File.Create(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\" + model.File.FileName))
                    {
                        model.File.CopyTo(fileStream);
                        fileStream.Flush();
                        var x = Guid.NewGuid() + ".jpeg";
                        var y = Path.GetRandomFileName() + ".jpeg";
                        return Ok(model.File.FileName); // TO DO Fix filename
                    }
                }
                else
                {
                    return BadRequest("Failed to upload file");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            return Ok(fileName);
        }
    }
}