using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.Dapper
{
    public class CustomerRepository : BaseSqliteRepository, ICustomerRepository
    {
		public void Insert(Customer customer)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Customer (Fullname, Phone) VALUES(@Fullname, @Phone)", customer);
				customer.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<CustomerModel> GetCustomersByPageNumber(int pageNumber)
		{
			pageNumber = (pageNumber - 1) * 5;

			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<CustomerModel>("SELECT * FROM Customer LIMIT 5 OFFSET @PageNumber ", new { PageNumber = pageNumber });
			}
		}

		public IEnumerable<CustomerModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<CustomerModel>("SELECT * FROM Customer");
			}
		}

		public int PhoneCounter(string phone)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM Customer WHERE Phone = @Phone", new { Phone = phone });
			}
		}

		public CustomerModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<CustomerModel>("SELECT * FROM Customer WHERE  Id = @Id", new { Id = id });
			}
		}

		public void ActivityInsert(Activity activity)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO Activity (UserId, Amount, CurrentDept, Description, ActivityType, CustomerId) VALUES(@UserId, @Amount, @CurrentDept, @Description, @ActivityType, @CustomerId)", activity);
				activity.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public ActivityModel GetActivityById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<ActivityModel>("SELECT * FROM Activity WHERE  Id = @Id", new { Id = id });
			}
		}

		public IEnumerable<ActivityModel> GetActivityByCustomerId(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<ActivityModel>("SELECT * FROM Activity WHERE  CustomerId = @CustomerId", new { CustomerId = id });
			}
		}

		/*public ActivityModel GetCustomerCurrentDept(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<ActivityModel>("SELECT * FROM Activity WHERE CustomerId = @Id AND Id = (SELECT MAX(ID)  FROM Activity)", new { Id = id });
			}
		}*/

		public ActivityModel GetCustomerLastActivity(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				var tempCustomerCount = dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM Activity WHERE CustomerId = @Id", new { Id = id });

				if (tempCustomerCount == 0)
				{
					return null;
				}

				return dbConnection.QuerySingle<ActivityModel>("SELECT * FROM Activity WHERE CustomerId = @Id AND Id = (SELECT MAX(ID) FROM Activity)", new { Id = id });

			}
		}
	}
}
