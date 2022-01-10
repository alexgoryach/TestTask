using System.ComponentModel.DataAnnotations;

namespace TestTask.ViewModels
{
    public class MessageViewModel
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public bool AutoDelete { get; set; }
    }
}