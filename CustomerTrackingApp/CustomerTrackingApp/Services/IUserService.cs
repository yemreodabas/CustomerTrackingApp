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
        void AddNewUser(User user);
        List<UserModel> GetAllUsers();
        List<UserModel> GetUsersByPage(int pageNumber);
        UserModel GetById(int id);
        int EmailCounter(string email);
        int UsernameCounter(string username);
        List<UserModel> GetManagerById();
        UserModel GetOnlineUser(HttpContext httpContext);
        void Logout(HttpContext httpContext);
        bool TryLogin(UserLoginModel loginData, HttpContext httpContext);
    }
}
