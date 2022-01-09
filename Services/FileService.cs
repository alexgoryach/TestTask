using Microsoft.AspNetCore.Hosting;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class FileService : IFileService
    {
        private static IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
    }
}