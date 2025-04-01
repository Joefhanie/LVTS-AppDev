using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.User
{
    public class UserVerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
