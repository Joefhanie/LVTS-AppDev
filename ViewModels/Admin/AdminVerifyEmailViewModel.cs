using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.Admin
{
    public class AdminVerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
