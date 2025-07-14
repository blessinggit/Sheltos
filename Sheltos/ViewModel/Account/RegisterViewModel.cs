using System.ComponentModel.DataAnnotations;

namespace Sheltos.ViewModel
{
    public class RegisterViewModel
    {
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters long.")]
        public string Password { get; set; }

        
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters long.")]
        public string ConfirmPassword { get; set; }
    }
}
