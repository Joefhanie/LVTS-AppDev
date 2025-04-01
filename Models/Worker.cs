using Microsoft.AspNetCore.Identity;

namespace LVTS.Models
{
    public class Worker : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Role { get; set; } = null!;

    }
}
