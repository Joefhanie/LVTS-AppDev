using LVTS.Data;
using LVTS.Models;
using LVTS.ViewModels.Admin;
using LVTS.ViewModels.Worker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LVTS.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<Admin> _signInManager;
        private readonly UserManager<Admin> _userManager;
        private readonly UserManager<Worker> _workerUserManager;
        private readonly LVTSContext _context;

        public AdminController(SignInManager<Admin> signInManager, UserManager<Admin> userManager, UserManager<Worker> workerUserManager, LVTSContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _workerUserManager = workerUserManager;
            _context = context;
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
                    return RedirectToAction("AdminDashboard", "Admin");
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
        public async Task<IActionResult> AdminSignUp(WorkersSignupViewModel model)
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

        [Authorize]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // Transactions
        [Authorize]
        public IActionResult AdminTransactions()
        {
            return View();
        }

        // Healthcare Workers
        [Authorize]
        public IActionResult AdminHealthcareWorkers()
        {
            var workers = _context.Workers.ToList();
            return View(workers);
        }

        public IActionResult AdminAddWorker()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminAddWorker(WorkersSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists
                var existingUser = await _workerUserManager.FindByNameAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username already exists!");
                    return View(model);
                }

                Worker worker = new Worker
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Address = model.Address,
                    ContactNumber = model.ContactNumber,
                    Email = model.Email,
                    UserName = model.Username,
                    Role = model.Role,
                };

                var result = await _workerUserManager.CreateAsync(worker, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminHealthcareWorkers", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            } else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
            }
                return View(model);
        }

        public async Task<IActionResult> AdminEditWorker(string id)
        {
            var worker = await _workerUserManager.FindByIdAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            var model = new WorkersSignupViewModel
            {
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Age = worker.Age,
                Address = worker.Address,
                ContactNumber = worker.ContactNumber,
                Email = worker.Email,
                Username = worker.UserName,
                Role = worker.Role
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEditWorker(WorkersSignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var worker = await _context.Workers.FindAsync(model.Id);
                if (worker == null)
                {
                    return NotFound();
                }

                worker.FirstName = model.FirstName;
                worker.LastName = model.LastName;
                worker.Age = model.Age;
                worker.Address = model.Address;
                worker.ContactNumber = model.ContactNumber;
                worker.Email = model.Email;
                worker.UserName = model.Username;
                worker.Role = model.Role;

                var updateResult = await _workerUserManager.UpdateAsync(worker);
                if (updateResult.Succeeded)
                {
                    return RedirectToAction("AdminHealthcareWorkers", "Admin");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
            }
                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteWorker(string id)
        {
            var worker = await _workerUserManager.FindByIdAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            var result = await _workerUserManager.DeleteAsync(worker);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminHealthcareWorkers", "Admin");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("AdminHealthcareWorkers", "Admin");
        }

        // Patients
        [Authorize]
        public IActionResult AdminPatients()
        {
            return View();
        }

        // Vaccines
        [Authorize]
        public IActionResult AdminVaccines()
        {
            return View();
        }


    }
}
