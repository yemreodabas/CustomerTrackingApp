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
    [Route("api/Customer")]
    public class CustomerApiController : Controller
    {
        private readonly ICustomerService _customerService;
		private readonly IUserService _userService;

		public CustomerApiController(ICustomerService customerService, IUserService userService)
        {
			_customerService = customerService;
			_userService = userService;
		}

		[HttpGet]
		[Route(nameof(GetActiveCustomers))]
		public ActionResult<ApiResponse<List<CustomerModel>>> GetActiveCustomers()
		{
			try
			{
				var customers = this._customerService.GetAllCustomers();
				//List<UserModel> userList = new List<UserModel>();
				/*
				for(int i =0; i < users.Count; i++)
				{
					if(i == 5)
					{
						break;
					}
					userList.Add(users[i]);
				}*/

				var response = ApiResponse<List<CustomerModel>>.WithSuccess(customers);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<CustomerModel>>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetCustomersByPageNumber))]
		public ActionResult<ApiResponse<List<CustomerModel>>> GetCustomersByPageNumber(int pageNumber)
		{
			try
			{
				var users = this._customerService.GetCustomersByPageNumber(pageNumber);

				var response = ApiResponse<List<CustomerModel>>.WithSuccess(users);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<List<CustomerModel>>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateCustomer))]
		public ActionResult<ApiResponse<CustomerModel>> CreateCustomer([FromBody]CreateCustomerModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				if(onlineUser.Type == UserType.Admin)
				{
					if(onlineUser.Type != UserType.Admin)
					{
						return Json(ApiResponse.WithError("Not Authority"));
					}
				}

				if (onlineUser.Type == UserType.Manager)
				{
					if (onlineUser.Type != UserType.Admin)
					{
						return Json(ApiResponse.WithError("Not Authority"));
					}
				}

				CustomerModel result = null;

				var newCustomer = new Customer();
				newCustomer.Fullname = model.Fullname;
				newCustomer.Phone = model.Phone;
				var phoneContains = this._customerService.PhoneCounter(model.Phone.ToString());

				if (phoneContains >= 1)
				{
					return Json(ApiResponse.WithError("This Phone Exists"));
				}

				this._customerService.AddNewCustomer(newCustomer);

				result = this._customerService.GetById(newCustomer.Id);
				return Json(ApiResponse<CustomerModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<CustomerModel>.WithError(exp.ToString()));
			}
		}

		[HttpPost]
		[Route(nameof(CreateActivity))]
		public ActionResult<ApiResponse<ActivityModel>> CreateActivity([FromBody]CreateActivityModel model)
		{
			try
			{
				var onlineUser = this._userService.GetOnlineUser(this.HttpContext);

				if (onlineUser == null)
				{
					return Json(ApiResponse.WithError("Not Authority"));
				}

				var customerLastActivity = this._customerService.GetCustomerLastActivity(model.CustomerId);

				ActivityModel result = null;

				var newActivity = new Activity();
				newActivity.Description = model.Description;
				newActivity.CustomerId = model.CustomerId;
				newActivity.ActivityType = model.ActivityType;
				newActivity.UserId = model.UserId;
				newActivity.Amount = model.Amount;
				if (model.ActivityType == ActivityType.Purchase)
				{
					if(customerLastActivity != null)
					{
						newActivity.CurrentDept = model.CurrentDept + customerLastActivity.CurrentDept;
					}
					else
					{
						newActivity.CurrentDept = model.CurrentDept + 0;
					}
				}

				else if (model.ActivityType == ActivityType.Payment)
				{
					
					if (customerLastActivity != null)
					{
						newActivity.CurrentDept = customerLastActivity.CurrentDept - model.CurrentDept;
					}
					else
					{
						newActivity.CurrentDept = model.CurrentDept + 0;
					}
				}

				else if (model.ActivityType == ActivityType.ProductReturns)
				{
					if (customerLastActivity != null)
					{
						newActivity.CurrentDept = model.CurrentDept - customerLastActivity.CurrentDept;
					}
					else
					{
						newActivity.CurrentDept = model.CurrentDept + 0;
					}
				}


				this._customerService.AddNewActivity(newActivity);

				result = this._customerService.GetActivityById(newActivity.Id);
				return Json(ApiResponse<ActivityModel>.WithSuccess(result));
			}
			catch (Exception exp)
			{
				return Json(ApiResponse<ActivityModel>.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetCustomerById))]
		public ActionResult<ApiResponse> GetCustomerById(int customerId)
		{
			try
			{
				var customer = this._customerService.GetById(customerId);

				var response = ApiResponse<CustomerModel>.WithSuccess(customer);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetCustomerCurrentDept))]
		public ActionResult<ApiResponse> GetCustomerCurrentDept(int customerId)
		{
			try
			{
				var customerLastActivity = this._customerService.GetCustomerLastActivity(customerId);
				/*
				if(customerDept == null)
				{
					customerDept.CurrentDept = 0;
				}*/

				var response = ApiResponse<ActivityModel>.WithSuccess(customerLastActivity);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}

		[HttpGet]
		[Route(nameof(GetActivityByCustomerId))]
		public ActionResult<ApiResponse<List<ActivityModel>>> GetActivityByCustomerId(int customerId)
		{
			try
			{
				var activity = this._customerService.GetActivityByCustomerId(customerId);

				var response = ApiResponse<List<ActivityModel>>.WithSuccess(activity);

				return Json(response);
			}
			catch (Exception exp)
			{
				return Json(ApiResponse.WithError(exp.ToString()));
			}
		}
	}
}
