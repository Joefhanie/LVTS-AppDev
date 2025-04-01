using LVTS.Models;
using LVTS.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LVTS.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> _userSignInManager;
        private readonly UserManager<User> _userManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userSignInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult UserLogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogIn(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userSignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult UserSignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserSignUp(UserSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Gender = model.Gender,
                    Address = model.Address,
                    ContactNumber = model.ContactNumber,
                    BirthDate = model.BirthDate,
                    PlaceOfBirth = model.PlaceOfBirth,
                    Email = model.Email,
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserLogin", "User");
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

        public IActionResult UserVerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserVerifyEmail(UserVerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    return RedirectToAction("UserChangePassword", "User", new { email = user.Email });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult UserChangePassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("UserVerifyEmail", "User");
            }
            return View(new UserChangePasswordViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> UserChangePassword(UserChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("UserLogin", "User");
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

        public async Task<IActionResult> UserLogout()
        {
            await _userSignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
