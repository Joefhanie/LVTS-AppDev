using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.User
{
    public class UserSignupViewModel
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = null!;
       
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = null!;
       
        [Required(ErrorMessage = "Age is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Age should be higher than 0")]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [Phone(ErrorMessage = "Invalid Mobile Number!")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "BirthDate")]
        public DateOnly BirthDate { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string Email { get; set; } = null!;
       
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;
       
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
