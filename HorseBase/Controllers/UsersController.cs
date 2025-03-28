using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HorseBase.Models.ViewModels.User;
using HorseBase.Models;
using HorseBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HorseBase.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = userRegister.UserName,
                    Email = userRegister.Email,
                    FirstName = userRegister.FirstName,
                    MiddleName = userRegister.MiddleName,
                    LastName = userRegister.LastName,
                    IsActive = true
                };
                var result = await _userManager.CreateAsync(user, userRegister.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                ViewData["error"] = "Вече има потребител с тези данни.";
            }
            return View(userRegister);
        }


        [HttpGet]
        public IActionResult LogIn()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserLogin logInRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(logInRequest.Email);

                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, logInRequest.Password);

                    if (passwordCheck)
                    {
                        if (!user.IsActive)
                        {
                            // If the user is inactive, redirect to the No Access page
                            return RedirectToAction("Banned", "Home");
                        }

                        // Otherwise, sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(logInRequest);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Fetch all users from the UserManager
            var users = _userManager.Users.ToList();

            // Create a list of UserViewModel to pass to the view
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(user);

                // Create a UserViewModel for each user
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = roles.ToList()
                };

                userViewModels.Add(userViewModel);
            }

            // Pass the list of UserViewModel to the view
            return View(userViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> List(string? searchInput)
        {
            // Fetch all users from the UserManager
            var users = _userManager.Users.ToList();
            if (!string.IsNullOrEmpty(searchInput))
            {
                users = users.Where(x =>
                    x.UserName.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                    x.Email.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                    x.FirstName.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                    x.MiddleName.Contains(searchInput, StringComparison.OrdinalIgnoreCase) ||
                    x.LastName.Contains(searchInput, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // Create a list of UserViewModel to pass to the view
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(user);

                // Create a UserViewModel for each user
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Roles = roles.ToList()
                };

                userViewModels.Add(userViewModel);
            }

            // Pass the list of UserViewModel to the view
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string[]IsActive)
        {
            foreach(var user in _context.users)
            {
                if (IsActive.Contains(user.Id))
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    user.IsActive = false;
                    await _userManager.UpdateAsync(user);
                }
            }
            //Iterate through all users' IsActive checkbox states
            //foreach (var key in form.Keys)
            //{
            //    if (key.StartsWith("isActive_"))
            //    {
            //        var userId = key.Substring("isActive_".Length);
            //        var user = await _userManager.FindByIdAsync(userId);
            //        if (user != null)
            //        {
            //            user.IsActive = form[key] == "on";
            //            await _userManager.UpdateAsync(user);
            //        }
            //    }
            //}

            return RedirectToAction("List");
        }


        public class HomeController : Controller
        {
            public IActionResult NoAccess()
            {
                return View();
            }
        }

        public IActionResult Reservations()
        {
            List<Reservation> userReservation = _context.reservations.Include(x => x.Horse).ThenInclude(x => x.Breed).Where(x => x.User.UserName == User.Identity.Name).ToList();
            return View(userReservation);
        }

    }
}
