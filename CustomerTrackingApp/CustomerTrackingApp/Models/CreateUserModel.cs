using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class CreateUserModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public int BirthYear { get; set; }
		public int Phone { get; set; }
		public string Fullname { get; set; }
		public UserType Type { get; set; }
		public Gender Gender { get; set; }
		public int ManagerId { get; set; }
		public ActiveType IsActive { get; set; }
	}
}
