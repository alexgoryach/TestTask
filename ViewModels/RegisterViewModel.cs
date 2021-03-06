using System.ComponentModel.DataAnnotations;

namespace TestTask.ViewModels
{
    // This class represents the user who is registering
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}