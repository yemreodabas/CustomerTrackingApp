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

        public UserController(IServices services)
        {
            this.services = services;
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

        public IActionResult Logout()
        {
            this.services.UserService.Logout(this.HttpContext);

            return RedirectToAction("Index", "Home");
        }
    }
}
