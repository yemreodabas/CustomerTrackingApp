using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public int BirthYear { get; set; }
		public int Phone { get; set; }
		public string Fullname { get; set; }
		public UserType Type { get; set; }
		public Gender Gender { get; set; }
		public int ManagerId { get; set; }
		public ActiveType IsActive { get; set; }

		public UserModel() { }

		public UserModel(User user)
		{
			this.Id = user.Id;
			this.Username = user.Username;
			this.Email = user.Email;
			this.BirthYear = user.BirthYear;
		}
	}
}
