using LVTS.Models;
using LVTS.ViewModels.Worker;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LVTS.Controllers
{
    public class WorkerController : Controller
    {
        private readonly SignInManager<Worker> _workerSignInManager;
        private readonly UserManager<Worker> _workerUserManager;

        public WorkerController(SignInManager<Worker> signInManager, UserManager<Worker> userManager)
        {
            _workerSignInManager = signInManager;
            _workerUserManager = userManager;
        }

        public IActionResult WorkersLogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WorkersLogIn(WorkersLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _workerSignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
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

        public IActionResult WorkersVerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WorkersVerifyEmail(WorkersVerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var worker = await _workerUserManager.FindByEmailAsync(model.Email);
                if (worker != null)
                {
                    return RedirectToAction("WorkersChangePassword", "Workers", new { email = worker.Email });
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult WorkersChangePassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("WorkersVerifyEmail", "Workers");
            }
            return View(new WorkersChangePasswordViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> WorkersChangePassword(WorkersChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var workers = await _workerUserManager.FindByEmailAsync(model.Email);
                if (workers != null)
                {
                    var result = await _workerUserManager.RemovePasswordAsync(workers);
                    if (result.Succeeded)
                    {
                        result = await _workerUserManager.AddPasswordAsync(workers, model.NewPassword);
                        return RedirectToAction("WorkersLogin", "Workers");
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

        public async Task<IActionResult> WorkersLogout()
        {
            await _workerSignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
