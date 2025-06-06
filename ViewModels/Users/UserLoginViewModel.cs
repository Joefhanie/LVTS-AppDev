﻿using System.ComponentModel.DataAnnotations;

namespace LVTS.ViewModels.User
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
