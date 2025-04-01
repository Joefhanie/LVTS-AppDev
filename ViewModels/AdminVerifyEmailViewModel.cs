using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels
{
    public class AdminVerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
