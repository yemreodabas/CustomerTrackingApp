using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using CustomerTrackingApp.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
    public class UserRepository : BaseEFRepository, IUserRepository
    {
		public void Insert(User user)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				dbConnection.Users.Add(user);
				dbConnection.SaveChanges();
			}
		}

		public IEnumerable<UserModel> GetUsersByPage(int pageNumber)
		{
			var skipCount = (pageNumber - 1) * 5;

			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
			 	var users = dbConnection.Users.Skip(skipCount).Take(5).ToList();
				return users.Select(u => new UserModel(u)).ToList();
			}
		}

		public IEnumerable<UserModel> GetAll()
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var users = dbConnection.Users.ToList();
				return users.Select(u => new UserModel(u)).ToList();
			}
		}

		public int UserCounter(string username)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.Users.Where(u => u.Username == username).Count();
			}
		}

		public int EmailCounter(string email)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.Users.Where(e => e.Email == email).Count();
			}
		}

		public UserModel GetById(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var user = dbConnection.Users.Where(u => u.Id == id);
				return user.Select(u => new UserModel(u)).ToModel(user);
			}
		}

		public IEnumerable<UserModel> GetManagerById()
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT * FROM User WHERE ManagerId == 0");
			}
		}

		public int GetUserIdByLogin(string username, string password)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				int userId = dbConnection.Query<int>("SELECT Id FROM User WHERE Username = @Username AND Password = @Password",
									new { Username = username, Password = password }).FirstOrDefault();

				return userId;
			}
		}
	}
}
