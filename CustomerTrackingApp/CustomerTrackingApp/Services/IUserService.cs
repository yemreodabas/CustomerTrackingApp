using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public interface IUserService
    {
        bool AddNewUser(User user);
        List<UserModel> GetAllUsers();
        UserModel GetById(int id);
        UserModel GetOnlineUser(HttpContext httpContext);
        void Logout(HttpContext httpContext);
        bool TryLogin(UserLoginModel loginData, HttpContext httpContext);
    }
}
