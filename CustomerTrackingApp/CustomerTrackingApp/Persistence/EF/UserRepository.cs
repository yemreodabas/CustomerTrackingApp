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
    public class UserRepository : BaseEFRepository, IUserRepository
    {
		public void Insert(User user)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				dbConnection.User.Add(user);
				dbConnection.SaveChanges();
			}
		}

		public IEnumerable<UserModel> GetUsersByPage(int pageNumber)
		{
			var skipCount = (pageNumber - 1) * 5;

			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
			 	var users = dbConnection.User.Skip(skipCount).Take(5).ToList();
				return users.Select(u => new UserModel(u)).ToList();
			}
		}

		public IEnumerable<UserModel> GetAll()
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var users = dbConnection.User.ToList();
				return users.Select(u => new UserModel(u)).ToList();
			}
		}

		public int UserCounter(string username)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.User.Where(u => u.Username == username).Count();
			}
		}

		public int EmailCounter(string email)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				return dbConnection.User.Where(e => e.Email == email).Count();
			}
		}

		public UserModel GetById(int id)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var user = dbConnection.User.Where(u => u.Id == id).FirstOrDefault();
				return user == null ? null : new UserModel(user);
			}
		}

		public IEnumerable<UserModel> GetManagers()
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var users = dbConnection.User.Where(u => u.ManagerId == 0).ToList();
				return users.Select(u => new UserModel(u)).ToList();
			}
		}

		public int GetUserIdByLogin(string username, string password)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				var userId = dbConnection.User.Where(u => u.Username == username && u.Password == password).Select(u => u.Id).FirstOrDefault();
				return userId;
			}
		}
	}
}
