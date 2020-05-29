using CustomerTrackingApp.Models;
using CustomerTrackingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IServices services;
        private readonly IUserService _userService;

        public UserController(IServices services, IUserService userService)
        {
            this.services = services;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Login));
            return View(model);
        }
        public IActionResult Users()
        {
            var model = this.services.ViewService.CreateViewModel<BaseViewModel>(this.HttpContext, nameof(this.Users));
            return View(model);
        }
        public IActionResult Profile(int id)
        {
            var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
            if(onlineUser != null)
            {
                var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Profile));
                model.UserId = id;

                return View(model);
            }

            return View(ApiResponse.WithError("Not Authority"));
        }

        public IActionResult Logout()
        {
            this.services.UserService.Logout(this.HttpContext);

            return RedirectToAction("Index", "Home");
        }
    }
}
