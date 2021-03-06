﻿using CustomerTrackingApp.Entities;
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

        public void AddNewUser(User user)
        {
            this._userRepository.Insert(user);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {user.Username}");
        }

        public UserModel GetById(int id)
        {
            return this._userRepository.GetById(id);
        }
        public List<UserModel> GetManagers()
        {
            return this._userRepository.GetManagers().ToList();
        }
        public int UsernameCounter(string username)
        {
            return this._userRepository.UserCounter(username);
        }

        public int EmailCounter(string email)
        {
            return this._userRepository.EmailCounter(email);
        }

        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Remove("onlineUserId");
        }

        public List<UserModel> GetUsersByPage(int pageNumber)
        {
            var users = this._userRepository.GetUsersByPage(pageNumber).ToList();

            return users;
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
