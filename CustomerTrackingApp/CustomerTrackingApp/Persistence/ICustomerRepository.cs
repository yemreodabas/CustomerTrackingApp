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
        IEnumerable<CustomerModel> GetCustomersByPageNumber(int pageNumber);
        IEnumerable<CustomerModel> GetAll();
        void ActivityInsert(Activity activity);
        ActivityModel GetActivityById(int id);
        IEnumerable<ActivityModel> GetActivityByCustomerId(int id);
        ActivityModel GetCustomerLastActivity(int id);
    }
}
