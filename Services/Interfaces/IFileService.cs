using System.Threading.Tasks;
using TestTask.ViewModels;

namespace TestTask.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadFile(FileViewModel model, string userId);

        public Task<string> GetFileByUrl(string userId, string url);

        public Task<string> GetAllUserFiles(string userId);
    }
}