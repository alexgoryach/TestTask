using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TestTask.Services.Interfaces;

namespace TestTask.ViewModels
{
    public class FileViewModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public bool AutoDelete { get; set; }
    }
}