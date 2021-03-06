﻿using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.Dapper
{
    public class UserRepository : BaseSqliteRepository, IUserRepository
    {
		public void Insert(User user)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				dbConnection.Execute("INSERT INTO User (Username, Email, Password, BirthYear, Phone, Fullname, Type, Gender, ManagerId, IsActive) VALUES(@Username, @Email, @Password, @BirthYear, @Phone, @Fullname, @Type, @Gender, @ManagerId, @IsActive)", user);
				user.Id = dbConnection.ExecuteScalar<int>("SELECT last_insert_rowid()");
			}
		}

		public IEnumerable<UserModel> GetUsersByPage(int pageNumber)
		{
			pageNumber = (pageNumber - 1) * 5;

			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT * FROM User LIMIT 5 OFFSET @PageNumber", new { PageNumber = pageNumber });
			}
		}

		public IEnumerable<UserModel> GetAll()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT * FROM User");
			}
		}

		public int UserCounter(string username)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM User WHERE Username = @Username", new { Username = username });
			}
		}

		public int EmailCounter(string email)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<int>("SELECT COUNT(*) FROM User WHERE Email = @Email", new { Email = email });
			}
		}

		public UserModel GetById(int id)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.QuerySingle<UserModel>("SELECT * FROM User WHERE  Id = @Id", new { Id = id });
			}
		}

		public IEnumerable<UserModel> GetManagers()
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				return dbConnection.Query<UserModel>("SELECT * FROM User WHERE  ManagerId == 0");
			}
		}

		public int GetUserIdByLogin(string username, string password)
		{
			using (IDbConnection dbConnection = this.OpenConnection())
			{
				int userId = dbConnection.Query<int>("SELECT Id FROM User WHERE Username = @Username AND Password = @Password",
									new { Username = username, Password = password }).FirstOrDefault();

				return userId;
			}
		}
	}
}
