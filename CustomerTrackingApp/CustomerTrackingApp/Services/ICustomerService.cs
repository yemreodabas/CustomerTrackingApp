using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Services
{
    public interface ICustomerService
    {
        void AddNewCustomer(Customer customer);
        List<CustomerModel> GetAllCustomers();
        List<CustomerModel> GetCustomersByPageNumber(int pageNumber);
        int PhoneCounter(string PhoneCounter);
        CustomerModel GetById(int id);
    }
}
