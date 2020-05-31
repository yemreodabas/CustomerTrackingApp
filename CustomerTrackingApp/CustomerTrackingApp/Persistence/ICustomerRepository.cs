using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence
{
    public interface ICustomerRepository
    {
        void Insert(Customer customer);
        CustomerModel GetById(int id);
        int PhoneCounter(string phone);
        IEnumerable<CustomerModel> GetFiveCustomers(int pageNumber);
        IEnumerable<CustomerModel> GetAll();
    }
}
