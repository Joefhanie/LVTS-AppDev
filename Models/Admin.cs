using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LVTS.Models
{
    public class Admin : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

    }
}
    