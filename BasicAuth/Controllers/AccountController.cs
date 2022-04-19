using BasicAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuth.Controllers
{
    public class AccountController : Controller
    {
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
        public IActionResult Login(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //validate user data against db 

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, account.Username),
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("CustomClaimType", "Custom claim value")
            };

            var identity = new ClaimsIdentity(claims, "Basic Identity");
            var principal = new ClaimsPrincipal(new[] { identity });

            HttpContext.SignInAsync(principal);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
