using LVTS.Models;
using LVTS.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LVTS.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<Admin> _signInManager;
        private readonly UserManager<Admin> _userManager;

        public AdminController(SignInManager<Admin> signInManager, UserManager<Admin> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult AdminLogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogIn(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View(model);
                }                    
            }
            return View(model);
        }

        public IActionResult AdminSignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminSignUp(AdminSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                Admin admin = new Admin
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Email = model.Email,
                    UserName = model.Username,
                };

                var result = await _userManager.CreateAsync(admin, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminLogin", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult AdminVerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminVerifyEmail(AdminVerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _userManager.FindByEmailAsync(model.Email);
                if (admin != null)
                {
                    return RedirectToAction("AdminChangePassword", "Admin", new {email = admin.Email});
                } else
                {
                    ModelState.AddModelError("", "Invalid Email");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult AdminChangePassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("AdminVerifyEmail", "Admin");
            }
            return View(new AdminChangePasswordViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> AdminChangePassword(AdminChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _userManager.FindByEmailAsync(model.Email);
                if (admin != null)
                {
                    var result = await _userManager.RemovePasswordAsync(admin);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(admin, model.NewPassword);
                        return RedirectToAction("AdminLogin", "Admin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong!");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> AdminLogout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
