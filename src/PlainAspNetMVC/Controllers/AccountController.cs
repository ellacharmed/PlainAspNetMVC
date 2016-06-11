using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlainAspNetMVC.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PlainAspNetMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger _logger;

        public AccountController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // For God sake, NEVER DO THIS! This is just an example!
                if (!String.IsNullOrWhiteSpace(model.Email) && model.Password == "12345678")
                {
                    var claims = new List<Claim> {
                        new Claim("ID", "1234567890"),
                        new Claim("name", "It's me!"),
                        new Claim("username", "admin"),
                        new Claim("role", "admin") };

                    var id = new ClaimsIdentity(claims, "password");
                    await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));

                    return new LocalRedirectResult(returnUrl ?? "/Home");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult forbidden()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
