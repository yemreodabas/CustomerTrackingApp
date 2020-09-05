using CustomerTrackingApp.Models;
using CustomerTrackingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IServices services;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public CustomerController(IServices services, ICustomerService customerService, IUserService userService)
        {
            this.services = services;
            _customerService = customerService;
            _userService = userService;
        }
        /*
        [HttpGet]
        public IActionResult Customers(int id)
        {
            var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
            if (onlineUser != null)
            {
                var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Customers));
                model.UserId = id;

                return View(model);
            }

            return View(ApiResponse.WithError("Not Authority"));
        }
        */
        public IActionResult Customers(int id)
        {
            var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
            if (onlineUser != null)
            {
                var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Customers));
                model.UserId = id;

                return View(model);
            }

            return View(ApiResponse.WithError("Not Authority"));
        }

        [HttpGet]
        public IActionResult CustomerProfile(int id)
        {
            var onlineUser = this._userService.GetOnlineUser(this.HttpContext);
            if (onlineUser != null)
            {
                var model = this.services.ViewService.CreateViewModel<UserViewModel>(this.HttpContext, nameof(this.Customers));
                model.UserId = id;

                return View(model);
            }

            return View(ApiResponse.WithError("Not Authority"));
        }
    }
}
