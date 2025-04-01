﻿using Microsoft.AspNetCore.Identity;

namespace LVTS.Models
{
    public class Admin : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
    