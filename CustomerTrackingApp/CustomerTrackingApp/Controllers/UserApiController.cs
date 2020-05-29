using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

		[HttpGet]
		[Route(nameof(GetActiveUsers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetActiveUsers()
		{
			try
			{
				var users = this._userService.GetAllUsers();
				List<UserModel> userList = new List<UserModel>();

				for(int i =0; i < users.Count; i++)
				{
					if(i == 5)
					{
						break;
					}
					userList.Add(users[i]);
				}

				var response = ApiResponse<List<UserModel>>.WithSuccess(userList);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetManagers))]
		public ActionResult<ApiResponse<List<UserModel>>> GetManagers()
		{
			try
			{
				var managers = this._userService.GetManagerById();

				var response = ApiResponse<List<UserModel>>.WithSuccess(managers);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<UserModel>>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetUserById))]
		public ActionResult<ApiResponse> GetUserById(int userId)
		{
			try
			{
				var user = this._userService.GetById(userId);

				var response = ApiResponse<UserModel>.WithSuccess(user);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateUser))]
		public ActionResult<ApiResponse<UserModel>> CreateUser([FromBody]CreateUserModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				if(model.Type == UserType.Admin)
				{
					if(onlineUser.Type != UserType.Admin)
					{
						return Json(ApiResponse.WithError("Not Authority"));
					}
				}

				if (model.Type == UserType.Manager)
				{
					if (onlineUser.Type != UserType.Admin)
					{
						return Json(ApiResponse.WithError("Not Authority"));
					}
				}

				UserModel result = null;

				var newUser = new User();
				newUser.Username = model.Username;
				newUser.Email = model.Email;
				newUser.Password = model.Password;
				newUser.ManagerId = model.ManagerId;
				newUser.Phone = model.Phone;
				newUser.Type = model.Type;
				newUser.IsActive = model.IsActive;
				newUser.Gender = model.Gender;
				newUser.BirthYear = model.BirthYear;
				newUser.Fullname = model.Fullname;

				bool userCheck = this._userService.AddNewUser(newUser);

				if (userCheck == true)
				{
					result = this._userService.GetById(newUser.Id);
					return Json(ApiResponse<UserModel>.WithSuccess(result));
				}

				return Json(ApiResponse<UserModel>.WithError("User Exist"));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<UserModel>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(Login))]
		public ActionResult<ApiResponse> Login([FromBody]UserLoginModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser != null)
				{
					return Json(ApiResponse.WithError("There is currently an open account!"));
				}

				if (!this._userService.TryLogin(model, this.HttpContext))
				{
					return Json(ApiResponse.WithError("Invalid Username or Password!"));
				}

				return Json(ApiResponse.WithSuccess());
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
