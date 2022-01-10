using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class MessageModel
    {
        public int id { get; set; }
        public string message { get; set; }
        public string userId { get; set; }
        public string url { get; set; }
        public bool autoDelete { get; set; }
        
        public MessageModel(int id, string message, string userId, string url, bool autoDelete)
        {
            this.id = id;
            this.message = message;
            this.userId = userId;
            this.url = url;
            this.autoDelete = autoDelete;
        }
    }
}