using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.Worker
{
    public class WorkersVerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
