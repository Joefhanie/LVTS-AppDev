using Microsoft.AspNetCore.Identity;

namespace LVTS.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string PlaceOfBirth{ get; set; } = null!;
    }
}
