using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
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

        public FileController(IWebHostEnvironment environment, UserManager<User> userManager, IFileService fileService, FileContext context)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post([FromForm]FileViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var uploadedUrl = _fileService.UploadFile(model, userId);
                return Task.FromResult<IActionResult>(Ok("Uploaded file link: /" + uploadedUrl.Result));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex.Message.ToString()));
            }
        }

        [HttpGet("{fileUrl}")]
        [Authorize]
        public async Task<IActionResult> Get(string fileUrl)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var filePath = _fileService.GetFileByUrl(userId, fileUrl).Result;
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return Ok(File(fileBytes, contentType, Path.GetFileName(filePath)).FileContents);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        // Needs correction
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Ok(_fileService.GetAllUserFiles(userId));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}