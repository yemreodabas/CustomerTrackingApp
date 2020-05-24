using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Persistence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public class UserService : IUserService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;

        public UserService(ILogRepository logRepository, IUserRepository userRepository)
        {
            this._logRepository = logRepository;
            this._userRepository = userRepository;
        }

        public UserModel GetOnlineUser(HttpContext httpContext)
        {
            int? onlineUserId = httpContext.Session.GetInt32("onlineUserId");
            if (!onlineUserId.HasValue)
            {
                return null;
            }

            return this._userRepository.GetById(onlineUserId.Value);
        }

        public bool AddNewUser(User user)
        {
            bool userCheck = true;

            List<UserModel> userList = GetAllUsers();

            for (int i = 0; i < userList.Count; i++)
            {
                if (user.Username == userList[i].Username)
                {
                    return false;
                }
            }

            this._userRepository.Insert(user);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");

            return userCheck;
        }

        public UserModel GetById(int id)
        {
            return this._userRepository.GetById(id);
        }
        public List<UserModel> GetManagerById()
        {
            return this._userRepository.GetManagerById().ToList();
        }

        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Remove("onlineUserId");
        }

        public List<UserModel> GetAllUsers()
        {
            var users = this._userRepository.GetAll().ToList();

            return users;
        }

        public bool TryLogin(UserLoginModel loginData, HttpContext httpContext)
        {
            int userId = this._userRepository.GetUserIdByLogin(loginData.Username, loginData.Password);

            if (userId > 0)
            {
                httpContext.Session.SetInt32("onlineUserId", userId);
                return true;
            }

            return false;
        }
    }
}
