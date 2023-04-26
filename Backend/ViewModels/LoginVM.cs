using System.ComponentModel.DataAnnotations;

namespace MixedDreams.WebAPI.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
