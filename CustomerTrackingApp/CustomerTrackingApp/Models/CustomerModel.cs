using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class CustomerModel
	{
		public int Id { get; set; }
		public string Fullname { get; set; }
		public int Phone { get; set; }

		public CustomerModel() { }

		public CustomerModel(Customer customer)
		{
			this.Id = customer.Id;
			this.Fullname = customer.Fullname;
			this.Phone = customer.Phone;
		}
	}
}
