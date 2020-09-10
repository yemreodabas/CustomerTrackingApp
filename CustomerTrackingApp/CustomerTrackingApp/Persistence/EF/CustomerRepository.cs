using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
    public class CustomerRepository : BaseEFRepository, ICustomerRepository
    {
		public void Insert(Customer customer)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				dbConnection.Customer.Add(customer);
				dbConnection.SaveChanges();
			}
		}

		public IEnumerable<CustomerModel> GetCustomersByPageNumber(int pageNumber)
		{
			var skipCount = (pageNumber - 1) * 5;

			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var customers = dbConnection.Customer.Skip(skipCount).Take(5).ToList();
				return customers.Select(c => new CustomerModel(c)).ToList();
			}
		}

		public IEnumerable<CustomerModel> GetAll()
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var customers = dbConnection.Customer.ToList();
				return customers.Select(c => new CustomerModel(c)).ToList();
			}
		}

		public int PhoneCounter(string phone)
		{
			var intPhone = Convert.ToInt32(phone);

			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.Customer.Where(c => c.Phone == intPhone).Count();
			}
		}

		public CustomerModel GetById(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var customer = dbConnection.Customer.Where(u => u.Id == id).FirstOrDefault();
				return customer == null ? null : new CustomerModel(customer);
			}
		}

		public void ActivityInsert(Activity activity)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				dbConnection.Activity.Add(activity);
				dbConnection.SaveChanges();
			}
		}

		public ActivityModel GetActivityById(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var activity = dbConnection.Activity.Where(a => a.Id == id).FirstOrDefault();
				return activity == null ? null : new ActivityModel(activity);
			}
		}

		public IEnumerable<ActivityModel> GetActivityByCustomerId(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var customerId = dbConnection.Activity.Where(a => a.CustomerId == id).ToList();
				return customerId.Select(c => new ActivityModel(c)).ToList();
			}
		}

		public ActivityModel GetCustomerLastActivity(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var tempCustomerCount = dbConnection.Activity.Where(c => c.CustomerId == id).Count();

				if (tempCustomerCount == 0)
				{
					return null;
				}
				
				var activity = dbConnection.Activity.Where(a => a.CustomerId == id).LastOrDefault();
				return activity == null ? null : new ActivityModel(activity);
			}
		}
	}
}
