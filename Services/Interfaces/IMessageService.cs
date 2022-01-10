using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models;
using TestTask.ViewModels;

namespace TestTask.Services.Interfaces
{
    public interface IMessageService
    {
        public Task<string> UploadMessage(MessageViewModel model, string userId);

        public Task<string> GetMessageByUrl(string url);

        public Task<List<MessageModel>> GetAllUserMessages(string userId);

        public Task<string> DeleteMessageByUrl(string url);
    }
}