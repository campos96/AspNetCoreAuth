using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var identityUser = await _userManager.FindByNameAsync(login.Username);

            if (identityUser != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(identityUser, login.Password, false, false);

                if (signInResult.Succeeded)
                {
                    TempData["Message"] = "Login succeeded";
                    return RedirectToAction(nameof(Index));
                }
            }

            ModelState.AddModelError(nameof(Account.Password), "Incorrect username or password.");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityUser = new IdentityUser
            {
                UserName = account.Username,
                Email = account.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, account.Password);

            if (identityResult.Succeeded)
            {
                TempData["Message"] = "Account was created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Message"] = "There was a problem creating your account, please try again later.";
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
