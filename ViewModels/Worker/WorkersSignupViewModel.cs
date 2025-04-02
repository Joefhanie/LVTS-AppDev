using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.Worker
{
    public class WorkersSignupViewModel
    {
        public string? Id { get; set; }
       
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = null!;
       
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = null!;
       
        [Required(ErrorMessage = "Age is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Age should be higher than 0")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Mobile Number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number!")]
        public string ContactNumber { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
       
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;
       
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} character.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
