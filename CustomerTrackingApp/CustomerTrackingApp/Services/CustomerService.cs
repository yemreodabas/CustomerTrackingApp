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
    public class CustomerService : ICustomerService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ILogRepository logRepository, IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            this._logRepository = logRepository;
            this._userRepository = userRepository;
            this._customerRepository = customerRepository;
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

        public void AddNewCustomer(Customer customer)
        {
            this._customerRepository.Insert(customer);
            this._logRepository.Log(Enums.LogType.Info, $"Inserted New User : {customer.Fullname}");
        }

        public CustomerModel GetById(int id)
        {
            return this._customerRepository.GetById(id);
        }

        public int PhoneCounter(string PhoneCounter)
        {
            return this._customerRepository.PhoneCounter(PhoneCounter);
        }

        public List<CustomerModel> GetFiveCustomers(int pageNumber)
        {
            var customers = this._customerRepository.GetFiveCustomers(pageNumber).ToList();

            return customers;
        }

        public List<CustomerModel> GetAllCustomers()
        {
            var customers = this._customerRepository.GetAll().ToList();

            return customers;
        }
    }
}
