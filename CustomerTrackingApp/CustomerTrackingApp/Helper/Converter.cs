using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMvcExample.Helpers
{
	public static class Converter
	{
		public static UserModel ToModel(this User user)
		{
			return new UserModel(user);
		}

		/*
		public static User ToEntity(this UserModel userModel)
		{
			return new User()
			{
				Id = userModel.Id,
				Username = userModel.Username,
				Email = userModel.Email,
				BirthYear = userModel.BirthYear,
			};
		}*/
	}
}
