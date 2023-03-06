using ePizzaHub.Entities;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ePizzaHub.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        IAuthenticationService _authService;
        public AccountController(IAuthenticationService authService, UserManager<User> userManager):base(userManager)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.AuthenticateUser(loginModel.Email, loginModel.Password);
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(returnUrl)&& Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    if (user.Roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }

                    else if (user.Roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "User" });
                    }

                }
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                bool result = _authService.CreateUser(user, model.Password);
                if (result)
                {
                    RedirectToAction("Login");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await  _authService.SignOut();
            return RedirectToAction("LogOutComplete");
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }
        public IActionResult LogOutComplete()
        {
            return View();
        }
    }
}
