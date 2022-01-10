namespace TestTask.Models
{
    public class FileModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
        public string url { get; set; }
        public bool autoDelete { get; set; }

        public FileModel(int id, string name, string userId, string url, bool autoDelete)
        {
            this.id = id;
            this.name = name;
            this.userId = userId;
            this.url = url;
            this.autoDelete = autoDelete;
        }
    }
}